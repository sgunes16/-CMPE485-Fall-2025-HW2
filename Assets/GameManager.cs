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
            
            Trap trapManager = FindObjectOfType<Trap>();
            if (trapManager != null)
            {
                trapManager.AddCollisionHandlerToRio(rioInstance, spawnPosition);
            }
        }
    }
    
    void SetupRioManager()
    {
        if (rioManagerObject != null && rioInstance != null)
        {
            follow followScript = rioManagerObject.GetComponent<follow>();
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
            StartCoroutine(SetRioAliveAfterDelay(rioInstance, true, 0.1f));
        }
    }
    
    void SetRioAlive(GameObject rioInstance, bool isAlive)
    {
        if (rioInstance != null)
        {
            MonoBehaviour[] scripts = rioInstance.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                var field = script.GetType().GetField("isAlive");
                if (field != null && field.FieldType == typeof(bool))
                {
                    field.SetValue(script, isAlive);
                    break;
                }
            }
        }
    }
    
    IEnumerator SetRioAliveAfterDelay(GameObject rioInstance, bool isAlive, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetRioAlive(rioInstance, isAlive);
    }
}

