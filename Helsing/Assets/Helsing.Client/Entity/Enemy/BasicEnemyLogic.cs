using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class BasicEnemyLogic : IEnemyLogic
    {
        IPlayerController playerController;
        IPathFinder pathFinder;
        IEnemyBlackboard enemyBlackboard;

        [Inject]
        private void Inject(IPlayerController playerController, IPathFinder pathFinder, IEnemyBlackboard enemyBlackboard) =>
            (this.playerController, this.pathFinder, this.enemyBlackboard) = (playerController, pathFinder, enemyBlackboard);

        private async Task<bool> CanSeePlayer(ITile currentTile)
        {
            if (playerController != null && !playerController.IsHidden)
            {
                var (path, dist) = await pathFinder.FindNextPathAndDistance(currentTile, playerController.CurrentTile);
                if (path != null)
                {
                    return dist < 5;
                }
            }

            return false;
        }

        public async Task<ITile> PickDestinationTile(ITile currentTile)
        {
            ITile target = null;
            if (await CanSeePlayer(currentTile))
            {
                var dest = await pathFinder.FindNextPath(currentTile, playerController.CurrentTile, enemyBlackboard.WillBeOccupied);
                if (dest != null)
                {
                    target = dest.Tile;
                }
            }

            if (target == null)
                target = currentTile.GetRandomNeighbor(true);
            return target;
        }
    }
}
