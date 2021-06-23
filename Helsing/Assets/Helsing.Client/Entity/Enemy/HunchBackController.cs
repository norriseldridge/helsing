using UnityEngine;
using Helsing.Client.Audio.Api;
using Helsing.Client.Entity.Enemy.Api;
using UniRx;
using Zenject;
using Helsing.Client.Entity.Api;
using Helsing.Client.Item;
using Helsing.Client.Item.Api;

namespace Helsing.Client.Entity.Enemy
{
    public class HunchBackController : MonoBehaviour
    {
        [SerializeField]
        EnemyController controller;

        [SerializeField]
        AudioClip playerSeen;

        [SerializeField]
        ItemData dropItem;

        IAudioPool audioPool;
        IMessageBroker broker;
        IInventory inventory;
        ILiving living;

        [Inject]
        private void Inject(IAudioPool audioPool, IMessageBroker broker, IInventory inventory) =>
            (this.audioPool, this.broker, this.inventory) = (audioPool, broker, inventory);

        private void Awake()
        {
            living = GetComponent<ILiving>();
        }

        private void Start()
        {
            broker.Receive<PlayerSpottedMessage>()
                .Where(m => m.spotter == (IEnemy)controller)
                .Take(1)
                .Subscribe(_ => audioPool.Next().PlayOneShot(playerSeen))
                .AddTo(this);

            living.LivesAsObservable.Where(l => l <= 0)
                .Subscribe(_ => OnDeath())
                .AddTo(this);
        }

        private void OnDeath() => inventory.AddItem(dropItem, 1);
    }
}
