using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Player;
using UI;
using UnityEngine;

namespace Fish
{
    public class FishController : MonoBehaviour, IDamageable
    {
        [SerializeField] private FishType type;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float pushForce;

        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private Item itemToLoot; // Assigned in Inspector

        private bool _isMoving;
        private Vector2 _movementDirection;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private PlayerController _player;
        private Animator _playerAnimator;
        private Rigidbody2D _playerRigidbody;
        private StatusBarController _statusBarController;
        private Queue<Vector2> _velocityHistory = new Queue<Vector2>();
        private float _trackDuration = 0.1f;
        private Vector2 _delayedVelocity;
        private GameObject statusBar;
        
        private static readonly int PlayerAnimatorParamAttacking = Animator.StringToHash("IsAttacking");

        public bool IsHurting { get; private set; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            _player = FindObjectOfType<PlayerController>();
            _playerAnimator = _player.GetComponent<Animator>();
            _playerRigidbody = _player.GetComponent<Rigidbody2D>();
            _statusBarController = GetComponentInChildren<StatusBarController>();
            statusBar = GetComponentInChildren<Transform>(true)?.Find("StatusBar")?.gameObject;

            InvokeRepeating(nameof(UpdateVelocityHistory), 0f, Time.fixedDeltaTime);

            if (inventoryManager == null)
            {
                inventoryManager = FindObjectOfType<InventoryManager>();
            }

            statusBar.SetActive(false);

        }
        
        private void UpdateVelocityHistory()
        {
            _velocityHistory.Enqueue(_rigidbody.velocity);

            // Remove old velocity values beyond 0.5 seconds
            if (_velocityHistory.Count > _trackDuration / Time.fixedDeltaTime)
            {
                _delayedVelocity = _velocityHistory.Dequeue();
            }
        }
        private void Update()
        {
            if (statusBar != null)
            {
                statusBar.transform.rotation = Quaternion.identity; // Keep rotation fixed
            }
        }

        private void FixedUpdate()
        {
            if (!_isMoving)
            {
                _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, deceleration * Time.deltaTime);
                return;
            }

            var directionMultiplier = type switch
            {
                FishType.Passive => 1,
                FishType.Aggressive => -1,
                _ => throw new ArgumentOutOfRangeException()
            };
            Vector2 direction = ((transform.position - _player.transform.position) * directionMultiplier).normalized;
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, direction * movementSpeed, acceleration * Time.deltaTime);

            // Flip sprite based on movement direction
            _spriteRenderer.flipX = _rigidbody.velocity.x switch
            {
                > 0.1f => false,
                < -0.1f => true,
                _ => _spriteRenderer.flipX
            };
        }

        // Box collider
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player") || IsHurting) return;
            
            var direction = (_player.transform.position - transform.position).normalized;
            _playerAnimator.SetBool(PlayerAnimatorParamAttacking, false);
            _playerRigidbody.AddForce(direction * pushForce, ForceMode2D.Impulse);

            if (type == FishType.Aggressive)
            {
                _player.TakeDamage(5.0f);
            }
                
        }

        // Circle collider
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _isMoving = true;
        }

        // Circle collider
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _isMoving = false;
        }

        public void Damage(float impact, float fortuneMultiplier)
        {
            if (IsHurting) return;
            statusBar.SetActive(true);
            IsHurting = true;
            _spriteRenderer.color = Color.red;
            
            var directionX = _delayedVelocity.x > 0 ? -1 : 1;
            if (type == FishType.Aggressive) _rigidbody.velocity = new Vector2(directionX, _rigidbody.velocity.y);
            _isMoving = false;
            _statusBarController.Decrease(impact); // TODO: Assign proper impact value
            Debug.Log($"[Build] Damage called on {name}. Amount before: {_statusBarController.Amount}, impact: {impact}");


            if (_statusBarController.Amount <= 0)
            {
                Destroy(gameObject);
                bool result = inventoryManager.AddItem(itemToLoot);
                Debug.Log("Fish is destoryed and added to inventory");

            }
            
            StartCoroutine(UnHurt());
        }

        private IEnumerator UnHurt()
        {
            yield return new WaitForSeconds(type == FishType.Aggressive ? 1.5f : 0.5f);
            _spriteRenderer.color = _originalColor;
            IsHurting = false;
            _isMoving = true;
        }
    }
}