using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject rioPrefab;
    public GameObject rioManagerObject;
    public Vector3 spawnPosition = new Vector3(427f, 2.28f, 463f);
    
    private GameObject rioInstance;
    
    void Start()
    {
        SpawnRio();
    }
    
    void SpawnRio()
    {
        if (rioPrefab != null)
        {
            rioInstance = Instantiate(rioPrefab, spawnPosition, Quaternion.identity);
            AddCollisionHandlerToAll(rioInstance);
            
            if (rioManagerObject != null)
            {
                follow followScript = rioManagerObject.GetComponent<follow>();
                if (followScript != null)
                {
                    followScript.player = rioInstance.transform;
                }
            }
        }
    }
    
    void AddCollisionHandlerToAll(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        RioCollisionHandler handler = obj.AddComponent<RioCollisionHandler>();
        handler.gameManager = this;
        handler.rioGameObject = rioInstance;
    }
    
    public void ResetRioPosition()
    {
        if (rioInstance != null)
        {
            rioInstance.transform.position = spawnPosition;
        }
    }
}

public class RioCollisionHandler : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject rioGameObject;
    
    void OnCollisionEnter(Collision collision)
    {
        CheckSpearCollision(collision.gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        CheckSpearCollision(other.gameObject);
    }
    
    void CheckSpearCollision(GameObject obj)
    {
        if (rioGameObject != null && (obj == rioGameObject || obj.transform.IsChildOf(rioGameObject.transform)))
        {
            return;
        }
        
        if (obj.layer == LayerMask.NameToLayer("spear") || obj.CompareTag("spear"))
        {
            gameManager.ResetRioPosition();
        }
    }
}

