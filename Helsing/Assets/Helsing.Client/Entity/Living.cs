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

        [SerializeField]
        GameObject deathSource;

        public int Lives => livesAsObservable.Value;
        public IReadOnlyReactiveProperty<int> LivesAsObservable => livesAsObservable;
        IReactiveProperty<int> livesAsObservable;

        IMessageBroker broker;

        [Inject]
        public void Inject(IMessageBroker broker) =>
            this.broker = broker;

        private void Awake()
        {
            livesAsObservable = new ReactiveProperty<int>(lives);
            livesAsObservable
                .Where(l => l <= 0)
                .Subscribe(_ => OnDeath())
                .AddTo(this);
        }

        private void Start() =>
            broker.Receive<HealthPickUpMessage>()
                .Subscribe(m => livesAsObservable.Value += m.lives)
                .AddTo(this);

        private void OnDeath()
        {
            if (deathSource != null)
            {
                var temp = Instantiate(deathSource);
                temp.transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }

        public void DealDamage() => livesAsObservable.Value--;
    }
}
