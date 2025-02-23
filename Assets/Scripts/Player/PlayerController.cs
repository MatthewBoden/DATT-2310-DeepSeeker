using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

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

    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private GameObject attackPosition;
    [SerializeField] private Vector2 attackCapsuleSize;

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

        if (Input.GetKeyDown(KeyCode.J)) animator.SetBool("IsAttacking", true);
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

        animator.SetBool("IsSprinting", isSprinting);

        // Check if moving and update animation
        bool isMoving = rb.velocity.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        // Set animation speed based on movement
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        // Flip sprite based on movement direction
        if (rb.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void TakeDamage(float damage)
    {
        animator.SetBool("IsHurt", true);
        CancelInvoke(nameof(ResetHurt));
        Invoke(nameof(ResetHurt), 1f);
    }

    private void ResetHurt()
    {
        animator.SetBool("IsHurt", false);
    }

    private void StartAttack()
    {
        var damageables = Physics2D.OverlapCapsuleAll(
            attackPosition.transform.position,
            attackCapsuleSize,
            CapsuleDirection2D.Horizontal,
            0f,
            damageableLayer);

        foreach (var damageable in damageables)
        {
            damageable.GetComponent<IDamageable>().Damage(5); // TODO: Set up appropriate impact values
        }
    }

    private void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPosition.transform.position, attackCapsuleSize);
    }
}