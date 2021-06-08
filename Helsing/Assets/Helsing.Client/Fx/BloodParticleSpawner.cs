using Helsing.Client.Fx.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Fx
{
    public class BloodParticleSpawner : MonoBehaviour
    {
        [SerializeField]
        float lifeTime;

        [SerializeField]
        Color color;

        [SerializeField]
        int count;

        IParticlePool particlePool;

        [Inject]
        private void Inject(IParticlePool particlePool) =>
            this.particlePool = particlePool;

        private void Start()
        {
            for (var i = 0; i < count; ++i)
            {
                var particle = particlePool.Next();
                var randDir = (Vector2.up * 10) + new Vector2(Random.Range(-12, 12), Random.Range(-0.1f, 15f));
                var offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), -1);
                particle.Initialize(lifeTime, transform.position + offset, randDir, color);
            }
        }
    }
}
