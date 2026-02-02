using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
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

    private GameBehavior _gameManager;

    private IEventHandler test;
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
    async Task<string> Shoot(float between)
    {   
        isShooting = true;
        GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = this.transform.forward * bulletSpeed;
        await Awaitable.WaitForSecondsAsync(between);
        isShooting=false;
        return "fired";
    }
    void FixedUpdate()
    {
        /*rotate.action.activeControl.path!="/Mouse/delta/right"                   code for conditional for future mouse movement for rotation*/
        rb.transform.Rotate(Vector3.up * _rotateDirection * rotateSpeed * Time.deltaTime);
        rb.transform.Translate(Vector3.forward * _moveDirection.y * moveSpeed * Time.deltaTime);
        rb.transform.Translate(Vector3.right * _moveDirection.x * moveSpeed * Time.deltaTime);
        if (IsGrounded() && jump.action.WasPerformedThisFrame())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (shoot.action.IsPressed()&& ! isShooting)
        {
            _ = Shoot(fireRate);
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
