using Fish;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        [SerializeField] private int baseGemCost = 5;
        [SerializeField] private float costMultiplier = 1.5f;
        private int gemCount;
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
        [SerializeField] private GameObject minePosition;
        [SerializeField] private Vector2 mineCapsuleSize;
        [SerializeField] private Vector2 attackCapsuleSize;
        [SerializeField] private GameObject inventoryMenu;
        [SerializeField] private GameObject upgradeMenu;

        private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>()
        {
            {"strength", 0},
            {"maxHealth", 0},
            {"maxStamina", 0},
            {"flashlightStat", 0},
            {"fortune", 0}
        };

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

            // Only Load Stats in Level 2
            if (GameManager.instance != null && SceneManager.GetActiveScene().name == "Level2")
            {
                Debug.Log("Loading saved stats for Level 2...");
                GameManager.instance.LoadPlayerStats(this);
            }

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
            if (Input.GetKeyDown(KeyCode.I)) {
                inventoryMenu.SetActive(!inventoryMenu.activeSelf); 
            }
            if (Input.GetKeyDown(KeyCode.U)) {
                upgradeMenu.SetActive(!upgradeMenu.activeSelf); 
            }

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
            Debug.Log("Player Died!");

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "MainScene")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else if (currentScene == "Level2")
            {
                SceneManager.LoadScene("GameOverScene2");
            }
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
                if (detectedCollider.isTrigger) continue;
                
                var damageable = detectedCollider.GetComponent<IDamageable>();
                if (damageable is FishController fish)
                {
                    damageable.Damage(3, fortune);
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
                minePosition.transform.position,
                mineCapsuleSize,
                CapsuleDirection2D.Horizontal,
                0f,
                damageableLayer);

            foreach (var detectedCollider in detectedColliders)
            {
                if (detectedCollider.isTrigger) continue;

                var damageable = detectedCollider.GetComponent<IDamageable>();
                if (damageable is FishController fish)
                {
                    damageable.Damage(1, fortune);
                }
                else
                {
                    damageable.Damage(3, fortune);
                }
            }
        }

        private void EndMine()
        {
            _animator.SetBool(AnimatorParamMining, IsMining = false);
        }

        public bool UpgradeStat(string statName)
        {
            if (_inventoryManager == null)
            {
                Debug.LogError("InventoryManager not found!");
                return false;
            }

            // Get current upgrade level
            if (!upgradeLevels.ContainsKey(statName))
            {
                Debug.LogError($"Invalid stat name: {statName}");
                return false;
            }

            int currentLevel = upgradeLevels[statName];
            int scaledGemCost = Mathf.RoundToInt(baseGemCost * Mathf.Pow(costMultiplier, currentLevel)); // Increase cost exponentially

            Debug.Log($"Trying to upgrade {statName}. Current Level: {currentLevel}, Cost: {scaledGemCost}");

            if (_inventoryManager.RemoveGems(scaledGemCost))
            {
                switch (statName)
                {
                    case "strength":
                        strength += 2f;
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
                }

                // Increase upgrade level for this stat
                upgradeLevels[statName]++;

                Debug.Log($"{statName} upgraded to level {upgradeLevels[statName]}! New Cost: {Mathf.RoundToInt(baseGemCost * Mathf.Pow(costMultiplier, upgradeLevels[statName]))}");
                return true;
            }

            Debug.Log("Not enough gems!");
            return false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPosition.transform.position, attackCapsuleSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(attackPosition.transform.position, mineCapsuleSize);
        }

        // Method to Set Stats (Used by GameManager)
        public void SetStats(float health, float maxHealth, float strength, float stamina, float fortune, float flashlightStat, int gems, Dictionary<string, int> upgrades)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.strength = strength;
            this.stamina = stamina;
            this.fortune = fortune;
            this.flashlightStat = flashlightStat;
            this.gemCount = gems;
            this.upgradeLevels = new Dictionary<string, int>(upgrades);

            Debug.Log("Player stats updated after load!");
        }

        public float GetHealth() => health;
        public float GetMaxHealth() => maxHealth;
        public float GetStrength() => strength;
        public float GetStamina() => stamina;
        public float GetFortune() => fortune;
        public float GetFlashlightStat() => flashlightStat;
        public int GetGemCount() => gemCount;
        public Dictionary<string, int> GetUpgradeLevels() => upgradeLevels;
    }
}
