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

        private void Start()
        {
            _light = GetComponent<Light2D>();
            if (_light == null) throw new MissingComponentException("No Light2D component attached");
        }

        private void Update()
        {
            var depth = Mathf.Abs(player.position.y);
            var normalizedDepth = Mathf.Clamp01(depth / depthLimit);
            _light.pointLightOuterRadius = Mathf.Lerp(baseMaxOuterRadius, baseMinOuterRadius, normalizedDepth);
            _light.pointLightInnerRadius = _light.pointLightOuterRadius * innerToOuterRadiusRatio;
        }
    }
}