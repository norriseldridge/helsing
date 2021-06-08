using System.Collections.Generic;
using Helsing.Client.Fx.Api;
using UnityEngine;

namespace Helsing.Client.Fx
{
    public class ParticlePool : MonoBehaviour, IParticlePool
    {
        [SerializeField]
        [Min(1)]
        int maxSize;

        [SerializeField]
        Particle source;

        readonly Queue<IParticle> pool = new Queue<IParticle>();
        
        public IParticle Next()
        {
            IParticle temp;
            if (pool.Count < maxSize)
            {
                temp = Instantiate(source);
            }
            else
            {
                temp = pool.Dequeue();
                temp.Reset();
            }

            pool.Enqueue(temp);
            return temp;
        }
    }
}
