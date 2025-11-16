using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonManager : MonoBehaviour
{
    [Header("Skeleton Settings")]
    public List<GameObject> skeletons = new List<GameObject>();
    public float sphereColliderRadius = 1f;
    
    void Start()
    {
        SetupSkeletonColliders();
    }
    
    void SetupSkeletonColliders()
    {
        foreach (GameObject skeleton in skeletons)
        {
            if (skeleton != null)
            {
                AddSphereColliderToSkeleton(skeleton);
            }
        }
    }
    
    void AddSphereColliderToSkeleton(GameObject skeleton)
    {
        SphereCollider sphereCollider = skeleton.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = skeleton.AddComponent<SphereCollider>();
        }
        
        sphereCollider.radius = sphereColliderRadius;
        sphereCollider.center = new Vector3(0, 1.42f, 0);
        sphereCollider.isTrigger = true;
        
        SkeletonCollisionHandler handler = skeleton.GetComponent<SkeletonCollisionHandler>();
        if (handler == null)
        {
            handler = skeleton.AddComponent<SkeletonCollisionHandler>();
        }
    }
}

public class SkeletonCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        CheckRioCollision(other.gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        CheckRioCollision(collision.gameObject);
    }
    
    void CheckRioCollision(GameObject obj)
    {
        if (obj.name.Contains("Rio") || obj.CompareTag("Player") || obj.GetComponent<PlayerScript>() != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                Vector3 spawnPos = gameManager.spawnPosition;
                gameManager.ResetRioPosition(obj, spawnPos);
            }
        }
    }
}

