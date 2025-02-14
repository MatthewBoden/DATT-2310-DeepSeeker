using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;  // Movement speed
    public float acceleration = 2f; 
    public float sprintMultiplier = 1.75f; // When player presses shift, speed is multiplied by this value
    public float health; // Health points
    public float stamina; // How long player sprints for
    public float strength; // How much damage player deals

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isSprinting;
    private bool isHurt;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        animator = GetComponent<Animator>(); // Get Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Correctly assign SpriteRenderer
    }

    void Update()
    {
        // Get movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Determine movement speed
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // Apply velocity for smooth movement
        Vector2 targetVelocity = moveInput * currentSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.deltaTime);

        // Check if player is sprinting
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)))
            isSprinting = true;
        else if (!Input.GetKey(KeyCode.LeftShift))
            isSprinting = false;

        animator.SetBool("isSprinting", isSprinting);

        // Check if moving and update animation
        bool isMoving = rb.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);

        // Set animation speed based on movement
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        // Flip sprite based on movement direction
        if (rb.velocity.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (rb.velocity.x < -0.1f)
            spriteRenderer.flipX = true;
    }
}