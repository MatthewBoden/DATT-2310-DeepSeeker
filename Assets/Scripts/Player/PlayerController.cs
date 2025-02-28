using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float sprintMultiplier = 1.75f; // Multiplier applied to the player's speed when sprinting

        [Header("Stats")]
        [SerializeField] private float health; // The player's overall durability
        [SerializeField] private float stamina; // Duration the player can sprint
        [SerializeField] private float strength; // Damage dealt per attack

        [Header("Attack")]
        [SerializeField] private LayerMask damageableLayer; // Layers that can be hit by attacks
        [SerializeField] private GameObject attackPosition; // Reference point for the weapon's point of contact
        [SerializeField] private Vector2 attackCapsuleSize; // Dimensions of the capsule used to detect hits

        // State properties
        public bool IsSprinting { get; private set; }
        public bool IsHurt { get; private set; }
        public bool IsAttacking { get; private set; }
        
        // Animation parameters
        private static readonly int AnimatorParamAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int AnimatorParamIsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int AnimatorParamIsMoving = Animator.StringToHash("IsMoving");
        private static readonly int AnimatorParamXVelocity = Animator.StringToHash("xVelocity");
        private static readonly int AnimatorParamIsHurt = Animator.StringToHash("IsHurt");
        
        private Rigidbody2D _rigidbody;
        private Vector2 _moveInput;
        private Animator _animator;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            _moveInput = new Vector2(moveX, moveY).normalized;
            if (Input.GetKeyDown(KeyCode.J)) _animator.SetBool(AnimatorParamAttacking, IsAttacking = true);
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }

        private void FixedUpdate()
        {
            var currentSpeed = IsSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
            var targetVelocity = _moveInput * currentSpeed;
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, acceleration * Time.deltaTime);

            // Check if player is sprinting
            if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)) ||
                (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)))
                IsSprinting = true;
            else if (!Input.GetKey(KeyCode.LeftShift))
                IsSprinting = false;

            _animator.SetBool(AnimatorParamIsSprinting, IsSprinting);

            // Check if moving and update animation
            var isMoving = _rigidbody.velocity.magnitude > 0.1f;
            _animator.SetBool(AnimatorParamIsMoving, isMoving);
            _animator.SetFloat(AnimatorParamXVelocity, Mathf.Abs(_rigidbody.velocity.x));

            // Flip game object based on movement direction
            transform.localScale = _rigidbody.velocity.x switch
            {
                > 0.1f => new Vector3(1, 1, 1),
                < -0.1f => new Vector3(-1, 1, 1),
                _ => transform.localScale
            };
        }

        public void TakeDamage(float impact)
        {
            _animator.SetBool(AnimatorParamIsHurt, IsHurt = true);
            CancelInvoke(nameof(ResetHurt));
            Invoke(nameof(ResetHurt), 1f);
        }

        private void ResetHurt()
        {
            _animator.SetBool(AnimatorParamIsHurt, IsHurt = false);
        }

        private void StartAttack()
        {
            IsAttacking = true;

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
            _animator.SetBool(AnimatorParamAttacking, IsAttacking = false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPosition.transform.position, attackCapsuleSize);
        }
    }
}