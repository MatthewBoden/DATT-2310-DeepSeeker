using Enums;
using UnityEngine;

namespace Fish
{
    public class FishController : MonoBehaviour
    {
        [SerializeField] private FishType type;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            
        }

        // Box collider
        private void OnCollisionEnter2D(Collision2D collision)
        {
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
            if (type == FishType.Passive)
            {
                
            }
            else if (type == FishType.Aggressive)
            {
                
            }
        }
    }
}