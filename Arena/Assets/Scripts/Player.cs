using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 _moveDirection;
    public InputActionReference move;
    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        
        
        
        
        
    }
    void FixedUpdate()
    {
        /*rb.linearVelocity = new Vector3( _moveDirection.x*moveSpeed,0,_moveDirection.y*moveSpeed); # command line for typical wasd movement*/
        rb.transform.Rotate(Vector3.up*_moveDirection.x*rotateSpeed*Time.deltaTime);
        rb.transform.Translate(Vector3.forward*_moveDirection.y*moveSpeed*Time.deltaTime);
    }

}
