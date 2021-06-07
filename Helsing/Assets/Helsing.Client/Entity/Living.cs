using Helsing.Client.Entity.Api;
using UniRx;
using UnityEngine;

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

        private void Awake()
        {
            livesAsObservable = new ReactiveProperty<int>(lives);
            livesAsObservable
                .Where(l => l <= 0)
                .Subscribe(_ => OnDeath())
                .AddTo(this);
        }

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

        public void AddLife() => livesAsObservable.Value++;
    }
}
