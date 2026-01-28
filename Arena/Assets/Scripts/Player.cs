using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpForce;
    private Vector2 _moveDirection;
    public InputActionReference move;

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    private CapsuleCollider _col;

    public GameObject bullet;
    public float bulletSpeed = 100f;

    void Start()
    {
        _col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
        /*rb.linearVelocity = new Vector3( _moveDirection.x*moveSpeed,0,_moveDirection.y*moveSpeed); # command line for typical wasd movement*/
        rb.transform.Rotate(Vector3.up * _moveDirection.x * rotateSpeed * Time.deltaTime);
        rb.transform.Translate(Vector3.forward * _moveDirection.y * moveSpeed * Time.deltaTime);
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            bulletRb.linearVelocity = this.transform.forward * bulletSpeed;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

}
