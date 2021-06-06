using Helsing.Client.Entity.Player.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        float speed;

        IPlayerController playerController;

        [Inject]
        private void Inject(IPlayerController playerController) =>
            this.playerController = playerController;

        private void Update()
        {
            if (playerController != null)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    playerController.CurrentTile.Position + (Vector3.back * 10),
                    Time.deltaTime * speed);
            }
        }
    }
}
