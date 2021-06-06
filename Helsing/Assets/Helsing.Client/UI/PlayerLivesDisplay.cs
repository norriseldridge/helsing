using Helsing.Client.Entity;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Helsing.Client.UI
{
    public class PlayerLivesDisplay : MonoBehaviour
    {
        [SerializeField]
        Text text;

        [SerializeField]
        Living player;

        private void Start() =>
            player.LivesAsObservable
                .Subscribe(l => text.text = $"{l}")
                .AddTo(this);
    }
}
