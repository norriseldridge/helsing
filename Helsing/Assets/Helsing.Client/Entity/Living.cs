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
        GameObject hitSource;

        [SerializeField]
        GameObject deathSource;

        public int Lives => livesAsObservable.Value;
        public IReadOnlyReactiveProperty<int> LivesAsObservable => livesAsObservable;
        IReactiveProperty<int> livesAsObservable;
        DiContainer container;

        [Inject]
        private void Inject(DiContainer container) =>
            this.container = container;

        private void Awake() =>
            livesAsObservable = new ReactiveProperty<int>(lives);

        private void Start() =>
            livesAsObservable
                .Pairwise((o, n) => o < n)
                .Subscribe(l =>
                {
                    if (livesAsObservable.Value > 0)
                        OnHit();
                    else
                        OnDeath();
                })
                .AddTo(this);

        private void OnHit()
        {
            if (hitSource != null)
            {
                var temp = Instantiate(hitSource);
                container.InjectGameObject(temp);
                temp.transform.position = transform.position;
            }
        }

        private void OnDeath()
        {
            if (deathSource != null)
            {
                var temp = Instantiate(deathSource);
                container.InjectGameObject(temp);
                temp.transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }

        public void DealDamage() => livesAsObservable.Value--;

        public void AddLife() => livesAsObservable.Value++;
    }
}
