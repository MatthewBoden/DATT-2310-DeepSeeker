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
        
        private bool _isMoving;
        private Vector2 _movementDirection;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private PlayerController _player;
        private Rigidbody2D _playerRigidbody;
        private StatusBarController _statusBarController;
        private Queue<Vector2> _velocityHistory = new Queue<Vector2>();
        private float _trackDuration = 0.1f;
        private Vector2 _delayedVelocity;

        public bool IsHurting { get; private set; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            _player = FindObjectOfType<PlayerController>();
            _playerRigidbody = _player.GetComponent<Rigidbody2D>();
            _statusBarController = GetComponentInChildren<StatusBarController>();

            InvokeRepeating(nameof(UpdateVelocityHistory), 0f, Time.fixedDeltaTime);
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
            if (!collision.gameObject.CompareTag("Player")) return;
            if (type == FishType.Passive)
            {
                Destroy(gameObject);
            }
            else if (type == FishType.Aggressive)
            {
                if (IsHurting) return;
                var direction = (_player.transform.position - transform.position).normalized;
                if (IsHurting) _playerRigidbody.AddForce(direction * pushForce, ForceMode2D.Impulse);
                _player.TakeDamage(5.0f); // damages player
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
            IsHurting = true;
            _spriteRenderer.color = Color.red;
            
            var directionX = _delayedVelocity.x > 0 ? -1 : 1;
            // _rigidbody.AddForce(new Vector2(directionX * 2.5f, 0), ForceMode2D.Impulse);
            _rigidbody.velocity = new Vector2(directionX, _rigidbody.velocity.y);
            _isMoving = false;
            _statusBarController.Decrease(3); // TODO: Assign proper impact value

            if (_statusBarController.Amount <= 0)
            {
                Destroy(gameObject);
            }
            
            StartCoroutine(UnHurt());
        }

        private IEnumerator UnHurt()
        {
            yield return new WaitForSeconds(1f);
            _spriteRenderer.color = _originalColor;
            IsHurting = false;
            _isMoving = true;
        }
    }
}