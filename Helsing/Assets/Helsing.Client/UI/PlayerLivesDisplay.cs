using Helsing.Client.Entity;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Helsing.Client.Entity.Player.Api;
using Zenject;

namespace Helsing.Client.UI
{
    public class PlayerLivesDisplay : MonoBehaviour
    {
        [SerializeField]
        Text text;

        IPlayerController playerController;

        [Inject]
        void Inject(IPlayerController playerController) =>
            this.playerController = playerController;

        private void Start() =>
            playerController.Living
                .LivesAsObservable
                .Subscribe(l => text.text = $"{l}")
                .AddTo(this);
    }
}
