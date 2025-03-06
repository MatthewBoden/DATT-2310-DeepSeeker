using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float innerToOuterRadiusRatio;
        [SerializeField] private float baseMaxOuterRadius;
        [SerializeField] private float baseMinOuterRadius;
        [SerializeField] private float depthLimit;

        private Light2D _light;
        private PlayerController _playerController; // For accessing flashlight stat

        private void Start()
        {
            _light = GetComponent<Light2D>();
            if (_light == null) throw new MissingComponentException("No Light2D component attached");

            _playerController = FindObjectOfType<PlayerController>();
            if (_playerController == null) Debug.LogError("❌ PlayerController not found in scene!");
            else Debug.Log("PlayerController found: " + _playerController.gameObject.name);
        }

        private void Update()
        {
            if (_playerController == null) return;

            var depth = Mathf.Abs(player.position.y);
            var normalizedDepth = Mathf.Clamp01(depth / depthLimit);

            // Apply flashlightStat multiplier
            float maxRadius = baseMaxOuterRadius * (_playerController.GetFlashlightStat());
            float minRadius = baseMinOuterRadius * (_playerController.GetFlashlightStat());

            _light.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, normalizedDepth);
            _light.pointLightInnerRadius = _light.pointLightOuterRadius * innerToOuterRadiusRatio;
        }
    }
}