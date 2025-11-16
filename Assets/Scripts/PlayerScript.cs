using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed = 8f;
    public float sprintSpeed = 15f;
    public float rotationSpeed = 120f;
    public float acceleration = 20f;
    public float deceleration = 15f;

    private Rigidbody rb;
    private Animator animator;
    
    private bool isSprinting;
    private float currentSpeed;
    private Vector3 targetVelocity;

    private int animIDIsWalking;
    private int animIDIsRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animIDIsWalking = Animator.StringToHash("isWalking");
            animIDIsRunning = Animator.StringToHash("isRunning");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        
        HandleRotation();
    }
    
    void HandleRotation()
    {
        float rotationInput = 0f;
        
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rotationInput = 2f;
        }
        
        if (rotationInput != 0f)
        {
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }
    }
    
    void LateUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        bool wantsToMove = Input.GetKey(KeyCode.W);
        float targetSpeed = wantsToMove ? (isSprinting ? sprintSpeed : moveSpeed) : 0f;
        
        if (wantsToMove)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }
        
        if (currentSpeed > 0.1f)
        {
            Vector3 moveDirection = transform.forward;
            targetVelocity = moveDirection * currentSpeed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            currentSpeed = 0f;
        }

        bool isMoving = currentSpeed > 0.1f;
        UpdateAnimations(isMoving, isMoving && isSprinting);
    }

    void UpdateAnimations(bool isMoving, bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool(animIDIsWalking, isMoving);
            animator.SetBool(animIDIsRunning, isRunning);
        }
    }
}
