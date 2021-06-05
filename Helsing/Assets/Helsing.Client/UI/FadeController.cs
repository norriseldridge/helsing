using Helsing.Client.UI.Api;
using System.Collections;
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

        [SerializeField]
        [Min(0.1f)]
        float speed;

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
                StartCoroutine(Fade(0, 1));
            }
            else
            {
                // fade out the image to 0
                StartCoroutine(Fade(1, 0));
            }
        }

        private IEnumerator Fade(float start, float end)
        {
            Color alpha = fade.color;
            alpha.a = start;
            while (!Mathf.Approximately(alpha.a, end))
            {
                alpha.a = Mathf.MoveTowards(alpha.a, end, speed * Time.deltaTime);
                fade.color = alpha;
                yield return null;
            }
            alpha.a = end;
            broker.Publish(new FadeCompleteMessage());
        }
    }
}
