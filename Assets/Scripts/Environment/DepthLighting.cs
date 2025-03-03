using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Environment
{
    public class DepthLighting : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float minIntensity;
        [SerializeField] private float depthLimit;

        private Light2D _globalLight;

        private void Start()
        {
            _globalLight = GetComponent<Light2D>();
            if (_globalLight == null) throw new MissingComponentException("No Light2D component attached");
        }

        private void Update()
        {
            var depth = Mathf.Abs(playerPosition.transform.position.y);
            var normalizedDepth = Mathf.Clamp01(depth / depthLimit); // Range: [0, 1]
            _globalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, normalizedDepth);
        }
    }
}