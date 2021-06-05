using Helsing.Client.UI.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client
{
    public class SceneController : MonoBehaviour
    {
        [Inject]
        void Inject(IMessageBroker broker) =>
            this.broker = broker;

        private void Start() =>
            broker.Publish(new FadeData(false)); // fade in from black

        IMessageBroker broker;
    }
}
