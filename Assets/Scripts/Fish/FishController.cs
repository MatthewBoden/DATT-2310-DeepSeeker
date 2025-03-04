using System;
using Enums;
using Player;
using UI;
using UnityEngine;

namespace Fish
{
    public class FishController : MonoBehaviour
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
        private PlayerController _player;
        private Rigidbody2D _playerRigidbody;
        private StatusBarController _statusBarController;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _player = FindObjectOfType<PlayerController>();
            _playerRigidbody = _player.GetComponent<Rigidbody2D>();
            _statusBarController = GetComponentInChildren<StatusBarController>();

        }

        private void Update()
        {
            
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
            if (!collision.gameObject.CompareTag("Player") && _player.IsAttacking) return;
            if (type == FishType.Passive)
            {
                Destroy(gameObject);
            }
            else if (type == FishType.Aggressive)
            {
                var direction = (_player.transform.position - transform.position).normalized;
                _playerRigidbody.AddForce(direction * pushForce, ForceMode2D.Impulse);
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
    }
}