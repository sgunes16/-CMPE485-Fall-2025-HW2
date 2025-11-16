using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap Lists")] 
    public List<GameObject> spikeTraps = new List<GameObject>();
    public List<GameObject> swingTraps = new List<GameObject>();

    [Header("Animation Settings")] 
    public float spikeTrapSpeed = 1f;
    public float swingTrapSpeed = 1f;
    public float spikeOpenDuration = 2f;
    public float spikeCloseDuration = 2f;
    
    void Start()
    {
        SetupAllTraps();
    }

    void SetupAllTraps()
    {
        foreach (GameObject trapObj in spikeTraps)
        {
            if (trapObj != null)
            {
                SetupTrap(trapObj, spikeTrapSpeed, spikeOpenDuration, spikeCloseDuration);
            }
        }
        
        foreach (GameObject trapObj in swingTraps)
        {
            if (trapObj != null)
            {
                SetupTrap(trapObj, swingTrapSpeed, spikeOpenDuration, spikeCloseDuration);
            }
        }
    }

    void SetupTrap(GameObject trapObj, float speed, float openDuration, float closeDuration)
    {
        Animator anim = trapObj.GetComponent<Animator>();
        if (anim == null)
        {
            return;
        }

        TrapController controller = trapObj.GetComponent<TrapController>();
        if (controller == null)
        {
            controller = trapObj.AddComponent<TrapController>();
        }

        controller.Setup(anim, speed, openDuration, closeDuration);
    }
    
    public void AddCollisionHandlerToPlayer(GameObject playerObj, Vector3 spawnPos)
    {
        Rigidbody rb = playerObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = playerObj.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        PlayerCollisionHandler handler = playerObj.AddComponent<PlayerCollisionHandler>();
        handler.playerGameObject = playerObj;
        handler.spawnPosition = spawnPos;
    }
    
}

public class TrapController : MonoBehaviour
{
    private Animator animator;
    private float speed;
    private float openTime;
    private float closeTime;
    private int openHash;
    private int closeHash;
    private bool hasOpenParam;
    private bool hasCloseParam;

    public void Setup(Animator anim, float animSpeed, float openDuration, float closeDuration)
    {
        animator = anim;
        speed = animSpeed;
        openTime = openDuration;
        closeTime = closeDuration;

        if (animator != null)
        {
            animator.speed = speed;
            CheckAnimatorParameters();
            StartCoroutine(OpenCloseTrap());
        }
    }

    void CheckAnimatorParameters()
    {
        if (animator.runtimeAnimatorController == null)
        {
            hasOpenParam = false;
            hasCloseParam = false;
            return;
        }

        openHash = Animator.StringToHash("open");
        closeHash = Animator.StringToHash("close");

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.nameHash == openHash && param.type == AnimatorControllerParameterType.Trigger)
            {
                hasOpenParam = true;
            }
            if (param.nameHash == closeHash && param.type == AnimatorControllerParameterType.Trigger)
            {
                hasCloseParam = true;
            }
        }
    }

    IEnumerator OpenCloseTrap()
    {
        while (animator != null)
        {
            if (animator.runtimeAnimatorController != null && hasOpenParam && hasCloseParam)
            {
                if (hasOpenParam)
                {
                    animator.SetTrigger(openHash);
                }
                yield return new WaitForSeconds(openTime);
                
                if (hasCloseParam)
                {
                    animator.SetTrigger(closeHash);
                }
                yield return new WaitForSeconds(closeTime);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

public class PlayerCollisionHandler : MonoBehaviour
{
    public GameObject playerGameObject;
    public Vector3 spawnPosition;
    
    void OnCollisionEnter(Collision collision)
    {
        CheckTrapCollision(collision.gameObject);
    }
    
    void CheckTrapCollision(GameObject obj)
    {
        if (playerGameObject != null && (obj == playerGameObject || obj.transform.IsChildOf(playerGameObject.transform)))
        {
            return;
        }
        
        int objLayer = obj.layer;
        int spearLayer = LayerMask.NameToLayer("spear");
        int bladeLayer = LayerMask.NameToLayer("blade");
        
        if (objLayer == spearLayer || objLayer == bladeLayer)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null && playerGameObject != null)
            {
                gameManager.ResetPlayerPosition(playerGameObject, spawnPosition);
            }
        }
    }
}

