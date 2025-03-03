using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        private Light2D _light;

        private void Start()
        {
            _light = GetComponent<Light2D>();
            if (_light == null) throw new MissingComponentException("No Light2D component attached");
        }

        private void Update()
        {
            
        }
    }
}