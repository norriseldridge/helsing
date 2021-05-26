using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Helsing.Client.Light
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField]
        new Light2D light;

        [SerializeField]
        float range;

        [SerializeField]
        float speed;

        [SerializeField]
        Vector2 flickerDelay;

        float intensity;
        float radius;
        float nextFlicker;

        private void Start()
        {
            intensity = light.intensity;
            radius = light.pointLightOuterRadius;
            nextFlicker = Random.Range(flickerDelay.x, flickerDelay.y);
        }

        // Update is called once per frame
        void Update()
        {
            light.intensity = intensity * (1 + (range * Mathf.Sin(speed * Time.time)));
            light.pointLightOuterRadius = radius * (1 + (range * Mathf.Sin(speed * Time.time)));
            nextFlicker -= Time.deltaTime;
            if (nextFlicker <= 0)
            {
                nextFlicker = Random.Range(flickerDelay.x, flickerDelay.y);
                light.intensity = intensity * (1 - range);
            }
        }
    }
}