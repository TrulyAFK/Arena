
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpForce;
    private Vector2 _moveDirection;
    private float _rotateDirection;
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference shoot;
    public InputActionReference rotate;
    public InputActionReference Mlock;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    private CapsuleCollider _col;

    public GameObject bullet;
    public float bulletSpeed = 100f;
    public float fireRate=.5f;
    public bool isShooting=false;

    public bool mouseLock=true;
    
    private GameBehavior _gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _gameManager=GameObject.Find("GameManger").GetComponent<GameBehavior>();
    }

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        _rotateDirection = rotate.action.ReadValue<float>();
    }
    async void Shoot(float between)
    {   
        isShooting = true;
        GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = this.transform.forward * bulletSpeed;
        await Awaitable.WaitForSecondsAsync(between);
        isShooting=false;
    }
    void FixedUpdate()
    {
        try
        {
            if (rotate.action.activeControl.path == "/Keyboard/q" || rotate.action.activeControl.path == "/Keyboard/e"||UnityEngine.Cursor.lockState==CursorLockMode.Locked)
            {
                rb.transform.Rotate(Vector3.up * _rotateDirection * rotateSpeed * Time.deltaTime);
            }
        } catch (NullReferenceException){}
        rb.transform.Translate(Vector3.forward * _moveDirection.y * moveSpeed * Time.deltaTime);
        rb.transform.Translate(Vector3.right * _moveDirection.x * moveSpeed * Time.deltaTime);
        jump.action.performed+=context=>{Jump();};
        if (shoot.action.IsPressed()&& ! isShooting)
        {
            Shoot(fireRate);
        }
        Mlock.action.performed+=context => {toggleMouse();};
    }
    void toggleMouse()
    {
        if (Mlock.action.triggered)
        {
            mouseLock =!mouseLock;
            if (mouseLock){
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }
    bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            _gameManager.HP -=1;
        }
    }

}
