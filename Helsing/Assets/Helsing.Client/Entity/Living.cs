using Helsing.Client.Entity.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity
{
    public class Living : MonoBehaviour, ILiving
    {
        [SerializeField]
        int lives;

        public int Lives => lives;

        IMessageBroker broker;

        [Inject]
        public void Inject(IMessageBroker broker) =>
            this.broker = broker;

        private void Start() =>
            broker.Receive<HealthPickUpMessage>()
                .Subscribe(m => lives += m.lives)
                .AddTo(this);

        public void DealDamage() => lives--;
    }
}
