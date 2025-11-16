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
            anim = trapObj.AddComponent<Animator>();
        }

        TrapController controller = trapObj.GetComponent<TrapController>();
        if (controller == null)
        {
            controller = trapObj.AddComponent<TrapController>();
        }

        controller.Setup(anim, speed, openDuration, closeDuration);
    }
    
    public void AddCollisionHandlerToRio(GameObject rioObj, Vector3 spawnPos)
    {
        Rigidbody rb = rioObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = rioObj.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        RioCollisionHandler handler = rioObj.AddComponent<RioCollisionHandler>();
        handler.rioGameObject = rioObj;
        handler.spawnPosition = spawnPos;
    }
    
}

public class TrapController : MonoBehaviour
{
    private Animator animator;
    private float speed;
    private float openTime;
    private float closeTime;

    public void Setup(Animator anim, float animSpeed, float openDuration, float closeDuration)
    {
        animator = anim;
        speed = animSpeed;
        openTime = openDuration;
        closeTime = closeDuration;

        if (animator != null)
        {
            animator.speed = speed;
            StartCoroutine(OpenCloseTrap());
        }
    }

    IEnumerator OpenCloseTrap()
    {
        while (animator != null)
        {
            if (animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger("open");
                yield return new WaitForSeconds(openTime);
                animator.SetTrigger("close");
                yield return new WaitForSeconds(closeTime);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

public class RioCollisionHandler : MonoBehaviour
{
    public GameObject rioGameObject;
    public Vector3 spawnPosition;
    
    void OnCollisionEnter(Collision collision)
    {
        CheckTrapCollision(collision.gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        CheckTrapCollision(other.gameObject);
    }
    
    void CheckTrapCollision(GameObject obj)
    {
        if (rioGameObject != null && (obj == rioGameObject || obj.transform.IsChildOf(rioGameObject.transform)))
        {
            return;
        }
        
        int objLayer = obj.layer;
        int spearLayer = LayerMask.NameToLayer("spear");
        int bladeLayer = LayerMask.NameToLayer("blade");
        
        if (objLayer == spearLayer || objLayer == bladeLayer || obj.CompareTag("spear") || obj.CompareTag("blade"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null && rioGameObject != null)
            {
                gameManager.ResetRioPosition(rioGameObject, spawnPosition);
            }
        }
    }
}

