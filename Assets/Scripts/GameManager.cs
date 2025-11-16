using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerManagerObject;
    public Vector3 spawnPosition = new Vector3(427f, 2.28f, 463f);
    
    private GameObject playerInstance;
    
    void Start()
    {
        SpawnPlayer();
    }
    
    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            
            PlayerStatus playerStatus = playerInstance.GetComponent<PlayerStatus>();
            if (playerStatus == null)
            {
                playerStatus = playerInstance.AddComponent<PlayerStatus>();
            }
            playerStatus.isAlive = true;
            
            Trap trapManager = FindObjectOfType<Trap>();
            if (trapManager != null)
            {
                trapManager.AddCollisionHandlerToPlayer(playerInstance, spawnPosition);
            }   
        }
    }

    
    public void ResetPlayerPosition(GameObject playerInstance, Vector3 spawnPosition)
    {
        if (playerInstance != null)
        {
            SetPlayerAlive(playerInstance, false);
            playerInstance.transform.position = spawnPosition;
        }
    }
    
    void SetPlayerAlive(GameObject playerInstance, bool isAlive)
    {
        if (playerInstance != null)
        {
            PlayerStatus playerStatus = playerInstance.GetComponent<PlayerStatus>();
            if (playerStatus == null)
            {
                playerStatus = playerInstance.AddComponent<PlayerStatus>();
            }
            playerStatus.isAlive = isAlive;
        }
    }
}

