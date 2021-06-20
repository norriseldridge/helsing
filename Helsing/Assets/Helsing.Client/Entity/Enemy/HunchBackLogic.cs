using System.Threading.Tasks;
using Helsing.Client.Audio.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class HunchBackLogic : IEnemyLogic
    {
        const string PLAYER_SEEN_SFX = "playerSeenSFX";

        public bool CanShareTile => false;

        IPlayerController playerController;
        IPathFinder pathFinder;
        IAudioPool audioPool;

        bool playerSeen = false;

        [Inject]
        private void Inject(IPlayerController playerController, IPathFinder pathFinder, IAudioPool audioPool)
        {
            this.playerController = playerController;
            this.pathFinder = pathFinder;
            this.pathFinder.OnlyFloors = true;
            this.audioPool = audioPool;
        }

        public async Task EveryTurn(IEnemy enemy)
        {
            if (playerSeen) return;

            var canSeePlayer = await CanSeePlayer(enemy.TileMover.CurrentTile.Value);
            if (canSeePlayer)
            {
                // TODO fire a "seen" message for near by enemies to use?
                try
                {
                    var playerSeenSfx = enemy.Blackboard.Get<AudioClip>(PLAYER_SEEN_SFX);
                    audioPool.Next().PlayOneShot(playerSeenSfx);
                }
                catch
                {
                    Debug.LogError($"Failed to play SFX for HunchBack ({PLAYER_SEEN_SFX})!");
                }
                playerSeen = true;
            }
        }

        public Task<ITile> PickDestinationTile(ITile currentTile) => Task.FromResult(currentTile);

        private async Task<bool> CanSeePlayer(ITile currentTile)
        {
            if (playerController != null && !playerController.IsHidden)
            {
                var (path, dist) = await pathFinder.FindNextPathAndDistance(currentTile, playerController.CurrentTile);
                if (path != null)
                {
                    return dist < 2;
                }
            }

            return false;
        }
    }
}
