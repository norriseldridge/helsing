using UnityEngine;

namespace Helsing.Client.Fx.Api
{
    public interface IParticle
    {
        void Initialize(float lifeTime, Vector3 position, Vector2 velocity, Color color);
        void Reset();
    }
}
