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
    public float visionRange = 10f;
    public float visionAngle = 45f;
    public int visionRayCount = 5;
    public float visionCheckInterval = 0.5f;
    
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool canCollide = true;
    private int mazeLayer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        mazeLayer = LayerMask.NameToLayer("maze");
        UpdateMoveDirection();
        StartCoroutine(VisionCheck());
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
        if (rb == null) return;
        
        Vector3 newVelocity = moveDirection * moveSpeed;
        newVelocity.y = 0f;
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
    
    GameObject GetPlayerObject(GameObject hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            return hitObject.transform.root.gameObject;
        }
        
        Transform parent = hitObject.transform.parent;
        while (parent != null)
        {
            if (parent.CompareTag("Player"))
            {
                return parent.gameObject;
            }
            parent = parent.parent;
        }
        
        return null;
    }
    
    IEnumerator VisionCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(visionCheckInterval);
            
            Vector3 rayOrigin = transform.position + Vector3.up * 5f;
            Vector3 forward = transform.forward;
            
            float angleStep = visionAngle / (visionRayCount - 1);
            float startAngle = -visionAngle / 2f;
            
            for (int i = 0; i < visionRayCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * forward;
                
                RaycastHit hit;
                bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out hit, visionRange);
                
                if (hasHit)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    
                    if (hitObject.layer == mazeLayer)
                    {
                        continue;
                    }
                    
                    GameObject playerObject = GetPlayerObject(hitObject);
                    if (playerObject != null)
                    {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        if (gameManager != null)
                        {
                            gameManager.ResetPlayerPosition(playerObject, gameManager.spawnPosition);  
                            break;
                        }
                    }
                }
            }
        }
    }
    
    void Update()
    {
        Move();
    }
}
