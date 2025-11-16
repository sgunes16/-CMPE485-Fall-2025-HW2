using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public enum MovementAxis
    {
        X,
        Z
    }
    
    public MovementAxis movementAxis = MovementAxis.Z;
    public float moveSpeed = 10f;
    
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool canCollide = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateMoveDirection();
    }
    
    void UpdateMoveDirection()
    {
        if (movementAxis == MovementAxis.X)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.forward;
        }
    }
    
    void Move()
    {
        Vector3 newVelocity = moveDirection * moveSpeed;
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
        
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }   
    
    void OnCollisionEnter(Collision collision)
    {
        if (!canCollide) return;
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("maze"))
        {
            moveDirection = -moveDirection;
            StartCoroutine(CollisionCooldown());
        }
    }
    
    IEnumerator CollisionCooldown()
    {
        canCollide = false;
        yield return new WaitForSeconds(0.2f);
        canCollide = true;
    }
    
    void Update()
    {
        Move();
    }
}
