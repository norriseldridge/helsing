using Helsing.Client.Fx.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Fx
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField]
        float delay;

        float currentDelay = 0;
        IParticlePool particlePool;

        [Inject]
        private void Inject(IParticlePool particlePool) =>
            this.particlePool = particlePool;

        private void Update()
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                currentDelay = delay;
                var particle = particlePool.Next();
                particle.Initialize(1.0f, transform.position, new Vector2(Random.Range(-2, 2), Random.Range(3, 5)), Color.red);
            }
        }
    }
}
