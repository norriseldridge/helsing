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
        int oldLives;
        DiContainer container;

        [Inject]
        private void Inject(DiContainer container) =>
            this.container = container;

        private void Awake() =>
            livesAsObservable = new ReactiveProperty<int>(lives);

        private void Start()
        {
            oldLives = lives;

            livesAsObservable.Skip(1)
                .Subscribe(l =>
                {
                    if (l > 0 && l < oldLives)
                        OnHit();
                    else if (l <= 0)
                        OnDeath();
                    oldLives = l;
                }).AddTo(this);
        }

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
