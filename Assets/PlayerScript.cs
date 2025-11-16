using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed = 8f;
    public float sprintSpeed = 15f;
    public float rotationSpeed = 90f;
    public float strafeSpeedMultiplier = 0.5f;

    private Rigidbody rb;
    private Animator animator;
    
    private float horizontal;
    private float vertical;
    private bool isSprinting;

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

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        
        HandleRotation();
    }
    
    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.up, rotationSpeed * 2f * Time.deltaTime);
        }
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        bool isMoving = Input.GetKey(KeyCode.W);
        
        if (isMoving)
        {
            Vector3 moveDirection = transform.forward;
            float speed = isSprinting ? sprintSpeed : moveSpeed;
            Vector3 velocity = moveDirection * speed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

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
