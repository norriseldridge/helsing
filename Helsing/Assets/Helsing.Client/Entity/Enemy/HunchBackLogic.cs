using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UniRx;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class HunchBackLogic : IEnemyLogic
    {
        public bool CanShareTile => false;

        IPlayerController playerController;
        IPathFinder pathFinder;
        IMessageBroker broker;

        bool playerSeen = false;

        [Inject]
        private void Inject(IPlayerController playerController, IPathFinder pathFinder, IMessageBroker broker)
        {
            this.playerController = playerController;
            this.pathFinder = pathFinder;
            this.pathFinder.OnlyFloors = true;
            this.broker = broker;
        }

        public async Task EveryTurn(IEnemy enemy)
        {
            if (playerSeen) return;

            var canSeePlayer = await CanSeePlayer(enemy.TileMover.CurrentTile.Value);
            if (canSeePlayer)
            {
                broker.Publish(new PlayerSpottedMessage(enemy));
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
