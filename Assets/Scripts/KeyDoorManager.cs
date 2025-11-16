using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorManager : MonoBehaviour
{
    [Header("Door and Key Settings")]
    public List<GameObject> doors = new List<GameObject>();
    public GameObject key;
    
    [Header("Door Animation Settings")]
    public float rotationAngle = 90f;
    public float moveDistance = 6f;
    public float animationDuration = 1f;
    
    [Header("Win Screen")]
    public string winMessage = "You Won";
    
    private Dictionary<GameObject, bool> doorOpened = new Dictionary<GameObject, bool>();
    private bool gameWon = false;
    
    void Start()
    {
        foreach (GameObject door in doors)
        {
            if (door != null)
            {
                doorOpened[door] = false;
                SetupDoorCollision(door);
            }
        }
        
        if (key != null)
        {
            SetupKeyCollision();
        }
    }
    
    void SetupDoorCollision(GameObject door)
    {
        DoorCollisionHandler handler = door.GetComponent<DoorCollisionHandler>();
        if (handler == null)
        {
            handler = door.AddComponent<DoorCollisionHandler>();
        }
        handler.keyDoorManager = this;
    }
    
    void SetupKeyCollision()
    {
        KeyCollisionHandler handler = key.GetComponent<KeyCollisionHandler>();
        if (handler == null)
        {
            handler = key.AddComponent<KeyCollisionHandler>();
        }
        handler.keyDoorManager = this;
    }
    
    public void HandleKeyDoorCollision(GameObject door, GameObject keyObject)
    {
        if (doorOpened.ContainsKey(door) && !doorOpened[door])
        {
            doorOpened[door] = true;
            StartCoroutine(OpenDoor(door));
            gameWon = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void OnGUI()
    {
        if (gameWon)
        {
            GUI.backgroundColor = Color.black;
            GUI.color = Color.white;
            
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 72;
            style.normal.textColor = Color.white;
            
            GUI.Label(new Rect(0, Screen.height / 2 - 50, Screen.width, 100), winMessage, style);
        }
    }
    
    IEnumerator OpenDoor(GameObject door)
    {
        Vector3 startPosition = door.transform.position;
        Quaternion startRotation = door.transform.rotation;
        
        Vector3 targetPosition = startPosition + new Vector3(moveDistance, 0, moveDistance);
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            
            door.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            door.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            
            yield return null;
        }
        
        door.transform.position = targetPosition;
        door.transform.rotation = targetRotation;
    }
}

public class DoorCollisionHandler : MonoBehaviour
{
    public KeyDoorManager keyDoorManager;
    
    void OnCollisionEnter(Collision collision)
    {
        CheckKeyCollision(collision.gameObject);
    }
    
    void CheckKeyCollision(GameObject obj)
    {
        if (keyDoorManager != null && keyDoorManager.key != null)
        {
            if (obj == keyDoorManager.key || obj.transform.IsChildOf(keyDoorManager.key.transform))
            {
                keyDoorManager.HandleKeyDoorCollision(gameObject, obj);
            }
        }
    }
}

public class KeyCollisionHandler : MonoBehaviour
{
    public KeyDoorManager keyDoorManager;
    
    void OnCollisionEnter(Collision collision)
    {
        CheckDoorCollision(collision.gameObject);
    }
    
    void CheckDoorCollision(GameObject obj)
    {
        if (keyDoorManager != null && keyDoorManager.doors != null)
        {
            foreach (GameObject door in keyDoorManager.doors)
            {
                if (door != null && (obj == door || obj.transform.IsChildOf(door.transform)))
                {
                    keyDoorManager.HandleKeyDoorCollision(door, gameObject);
                }
            }
        }
    }
}

