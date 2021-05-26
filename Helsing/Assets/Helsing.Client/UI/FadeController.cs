using Helsing.Client.UI.Api;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Helsing.Client.UI
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField]
        Image fade;

        IMessageBroker broker;

        [Inject]
        void Inject(IMessageBroker broker) => this.broker = broker;

        private void Start()
        {
            broker.Receive<FadeData>()
                .Subscribe(OnFadeData)
                .AddTo(this);
        }

        private void OnFadeData(FadeData fadeData)
        {
            if (fadeData.fade)
            {
                // fade in the image to 1
            }
            else
            {
                // fade out the image to 0
            }
        }
    }
}
