using Fish;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float sprintMultiplier = 1.75f;

        [Header("Stats")]
        [SerializeField] private float health = 20f;
        [SerializeField] private float maxHealth = 20f;
        private float healthRegenRate = 0.25f;  // How much stamina regenerates per second
        private float healthRegenDelay = 3f; // Delay before stamina starts regenerating
        [SerializeField] private float strength;
        [SerializeField] private float stamina = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float fortune = 1.0f;
        [SerializeField] private float flashlightStat;
        private float staminaDrainRate = 10f; // How much stamina drains per second when sprinting
        private float staminaRegenRate = 5f;  // How much stamina regenerates per second
        private float staminaRegenDelay = 2f; // Delay before stamina starts regenerating

        private float lastSprintTime;

        [Header("UI")]
        [SerializeField] private Image healthBar;
        [SerializeField] private Image staminaBar;

        [Header("Attack")]
        [SerializeField] private LayerMask damageableLayer;
        [SerializeField] private GameObject attackPosition;
        [SerializeField] private Vector2 attackCapsuleSize;

        // State properties
        public bool IsSprinting { get; private set; }
        public bool IsHurt { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsMining { get; private set; }
        
        // Animation parameters
        private static readonly int AnimatorParamAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int AnimatorParamMining = Animator.StringToHash("IsMining");
        private static readonly int AnimatorParamIsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int AnimatorParamIsMoving = Animator.StringToHash("IsMoving");
        private static readonly int AnimatorParamXVelocity = Animator.StringToHash("xVelocity");
        private static readonly int AnimatorParamIsHurt = Animator.StringToHash("IsHurt");

        private Rigidbody2D _rigidbody;
        private Vector2 _moveInput;
        private Animator _animator;
        private InventoryManager _inventoryManager;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _inventoryManager = FindObjectOfType<InventoryManager>();

            // Ensure health starts at max
            health = maxHealth;
            UpdateHealthBar();

            stamina = maxStamina;
            UpdateStaminaBar();
        }

        private void Update()
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            _moveInput = new Vector2(moveX, moveY).normalized;

            if (Input.GetKeyDown(KeyCode.J)) _animator.SetBool(AnimatorParamAttacking, IsAttacking = true);
            if (Input.GetKeyDown(KeyCode.K)) _animator.SetBool(AnimatorParamMining, IsMining = true);
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

            if (health <= 0) Die();

            if (IsSprinting && stamina > 0)
            {
                UseStamina(staminaDrainRate * Time.deltaTime);
                lastSprintTime = Time.time; // Store last sprint time
            }
            else if (!IsSprinting && Time.time > lastSprintTime + staminaRegenDelay)
            {
                RegenerateStamina();
            }
            Heal(); 
        }

        private void FixedUpdate()
        {
            var currentSpeed = IsSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
            var targetVelocity = _moveInput * currentSpeed;
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, acceleration * Time.deltaTime);

            // Sprinting logic
            if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) && stamina > 0) ||
                (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && stamina > 0))
            {
                IsSprinting = true;
            }
            else
            {
                IsSprinting = false;
            }

            // Stop sprint animation if stamina is 0
            if (stamina <= 0)
            {
                IsSprinting = false;
            }

            // Update animator
            _animator.SetBool(AnimatorParamIsSprinting, IsSprinting);

            // Movement animations
            var isMoving = _rigidbody.velocity.magnitude > 0.1f;
            _animator.SetBool(AnimatorParamIsMoving, isMoving);
            _animator.SetFloat(AnimatorParamXVelocity, Mathf.Abs(_rigidbody.velocity.x));

            // Flip player sprite
            transform.localScale = _rigidbody.velocity.x switch
            {
                > 0.1f => new Vector3(1, 1, 1),
                < -0.1f => new Vector3(-1, 1, 1),
                _ => transform.localScale
            };
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthBar();

            Debug.Log("Damage Taken: " + damage + " | Health Remaining: " + health);

            _animator.SetBool(AnimatorParamIsHurt, IsHurt = true);
            CancelInvoke(nameof(ResetHurt));
            Invoke(nameof(ResetHurt), 1f);
        }

        private void Heal()
        {
            if (health < maxHealth)
            {
                health += healthRegenRate * Time.deltaTime;
                health = Mathf.Clamp(health, 0, maxHealth);
                UpdateHealthBar();
            }
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = health / maxHealth;
            }
        }

        private void UseStamina(float amount)
        {
            stamina -= amount;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            UpdateStaminaBar();
        }

        private void RegenerateStamina()
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
                UpdateStaminaBar();
            }
        }

        private void UpdateStaminaBar()
        {
            if (staminaBar != null)
            {
                staminaBar.fillAmount = stamina / maxStamina;
            }
        }

        private void Die()
        {
            Debug.Log("Player Died! Restarting Level...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void ResetHurt()
        {
            _animator.SetBool(AnimatorParamIsHurt, IsHurt = false);
        }

        private void StartAttack()
        {
            IsAttacking = true;

            var detectedColliders = Physics2D.OverlapCapsuleAll(
                attackPosition.transform.position,
                attackCapsuleSize,
                CapsuleDirection2D.Horizontal,
                0f,
                damageableLayer);

            foreach (var detectedCollider in detectedColliders)
            {
                var damageable = detectedCollider.GetComponent<IDamageable>();
                if (damageable is FishController fish)
                {
                    damageable.Damage(5, fortune);
                }
                else
                {
                    // Leave it for now
                }
            }
        }

        private void EndAttack()
        {
            _animator.SetBool(AnimatorParamAttacking, IsAttacking = false);
        }

        private void StartMine()
        {
            IsMining = true;

            var detectedColliders = Physics2D.OverlapCapsuleAll(
                attackPosition.transform.position,
                attackCapsuleSize,
                CapsuleDirection2D.Horizontal,
                0f,
                damageableLayer);

            foreach (var detectedCollider in detectedColliders)
            {
                var damageable = detectedCollider.GetComponent<IDamageable>();
                if (damageable is FishController fish)
                {
                    // Leave it for now
                }
                else
                {
                    damageable.Damage(5, fortune);
                }
            }
        }

        private void EndMine()
        {
            _animator.SetBool(AnimatorParamMining, IsMining = false);
        }

        public bool UpgradeStat(string statName, int gemCost)
        {
            if (_inventoryManager == null)
            {
                Debug.LogError("InventoryManager not found!");
                return false;
            }

            if (_inventoryManager.RemoveGems(gemCost))
            {
                switch (statName)
                {
                    case "strength":
                        strength += 2f; // Values will probably need to be finalized
                        break;
                    case "maxHealth":
                        maxHealth += 5f;
                        health = maxHealth;
                        UpdateHealthBar();
                        break;
                    case "maxStamina":
                        maxStamina += 25f;
                        stamina = maxStamina;
                        UpdateStaminaBar();
                        break;
                    case "flashlightStat":
                        flashlightStat += 0.5f;
                        Debug.Log("Flashlight stat upgraded! New value: " + flashlightStat);
                        break;
                    case "fortune":
                        fortune += 0.5f;
                        break;
                    default:
                        Debug.LogError("Invalid stat name: " + statName);
                        return false;
                }

                Debug.Log($"{statName} upgraded!");
                return true;
            }

            Debug.Log("Not enough gems!");
            return false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPosition.transform.position, attackCapsuleSize);
        }

        public float GetFlashlightStat()
        {
            return flashlightStat;
        }
    }
}
