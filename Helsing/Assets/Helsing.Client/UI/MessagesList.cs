using Helsing.Client.UI.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.UI
{
    public class MessagesList : MonoBehaviour
    {
        [SerializeField]
        Message source;

        IMessageBroker broker;

        [Inject]
        private void Inject(IMessageBroker broker) => this.broker = broker;

        private void Start() =>
            broker.Receive<MessageData>()
                .Subscribe(data => DisplayMessage(data.message))
                .AddTo(this);

        private void DisplayMessage(string message)
        {
            var temp = Instantiate(source, transform);
            temp.Text = message;
        }
    }
}