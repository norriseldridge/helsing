using UnityEngine;
using Helsing.Client.Audio.Api;
using Helsing.Client.Entity.Enemy.Api;
using UniRx;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class HunchBackController : MonoBehaviour
    {
        [SerializeField]
        EnemyController controller;

        [SerializeField]
        AudioClip playerSeen;

        IAudioPool audioPool;
        IMessageBroker broker;

        [Inject]
        private void Inject(IAudioPool audioPool, IMessageBroker broker) =>
            (this.audioPool, this.broker) = (audioPool, broker);

        private void Start()
        {
            broker.Receive<PlayerSpottedMessage>()
                .Where(m => m.spotter == (IEnemy)controller)
                .Take(1)
                .Subscribe(_ => audioPool.Next().PlayOneShot(playerSeen))
                .AddTo(this);
        }
    }
}
