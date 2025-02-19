using Enums;
using UnityEngine;

namespace Fish
{
    public class FishController : MonoBehaviour
    {
        [SerializeField] private FishType type;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        
        private bool _isMoving;
        private Vector2 _movementDirection;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, _movementDirection * movementSpeed, acceleration * Time.deltaTime);

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
                
            }
        }

        // Circle collider
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _isMoving = true;
            if (type == FishType.Passive)
            {
                _movementDirection = (transform.position - collision.transform.position).normalized;
            }
            else if (type == FishType.Aggressive)
            {
                
            }
        }

        // Circle collider
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _isMoving = false;
        }
    }
}