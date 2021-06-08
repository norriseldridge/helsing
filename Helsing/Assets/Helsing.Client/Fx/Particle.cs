using Helsing.Client.Fx.Api;
using UnityEngine;

namespace Helsing.Client.Fx
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Particle : MonoBehaviour, IParticle
    {
        public bool IsDone => lifeTime <= 0;

        float lifeTime;
        float initialLifeTime;
        SpriteRenderer spriteRenderer;
        Rigidbody2D rb;
        Color alpha;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float lifeTime, Vector3 position, Vector2 velocity, Color color)
        {
            if (!IsDone)
            {
                Debug.LogError("You are initializing a particle which is not yet done! This might mean the particle pool is too small.");
            }

            gameObject.SetActive(true);
            alpha = color;
            transform.position = position;
            this.lifeTime = lifeTime;
            initialLifeTime = lifeTime;
            rb.AddForce(velocity);
            spriteRenderer.color = color;
        }

        public void Reset()
        {
            rb.velocity = Vector2.zero;
            spriteRenderer.color = Color.white;
        }

        private void Update()
        {
            if (IsDone)
                return;

            lifeTime -= Time.deltaTime;
            alpha.a = lifeTime / initialLifeTime;
            spriteRenderer.color = alpha;

            if (IsDone)
                gameObject.SetActive(false);
        }
    }
}
