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
        SetupRioManager();
    }
    
    void SpawnRio()
    {
        if (rioPrefab != null)
        {
            rioInstance = Instantiate(rioPrefab, spawnPosition, Quaternion.identity);
            
            RioStatus rioStatus = rioInstance.GetComponent<RioStatus>();
            if (rioStatus == null)
            {
                rioStatus = rioInstance.AddComponent<RioStatus>();
            }
            rioStatus.isAlive = true;
            
            Trap trapManager = FindObjectOfType<Trap>();
            if (trapManager != null)
            {
                trapManager.AddCollisionHandlerToRio(rioInstance, spawnPosition);
            }
        }
    }
    
    void SetupRioManager()
    {
        if (rioInstance == null) return;
        
        if (rioManagerObject != null)
        {
            Follow followScript = rioManagerObject.GetComponent<Follow>();
            if (followScript != null)
            {
                followScript.player = rioInstance.transform;
            }
        }
        else
        {
            Follow followScript = FindObjectOfType<Follow>();
            if (followScript != null)
            {
                followScript.player = rioInstance.transform;
            }
        }
    }
    
    public void ResetRioPosition(GameObject rioInstance, Vector3 spawnPosition)
    {
        if (rioInstance != null)
        {
            SetRioAlive(rioInstance, false);
            rioInstance.transform.position = spawnPosition;
        }
    }
    
    void SetRioAlive(GameObject rioInstance, bool isAlive)
    {
        if (rioInstance != null)
        {
            RioStatus rioStatus = rioInstance.GetComponent<RioStatus>();
            if (rioStatus == null)
            {
                rioStatus = rioInstance.AddComponent<RioStatus>();
            }
            rioStatus.isAlive = isAlive;
        }
    }
}

