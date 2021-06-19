using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class GhostLogic : IEnemyLogic
    {
        IPlayerController playerController;
        IPathFinder pathFinder;
        IEnemyBlackboard enemyBlackboard;

        [Inject]
        private void Inject(IPlayerController playerController, IPathFinder pathFinder, IEnemyBlackboard enemyBlackboard) =>
            (this.playerController, this.pathFinder, this.enemyBlackboard) = (playerController, pathFinder, enemyBlackboard);

        public async Task<ITile> PickDestinationTile(ITile currentTile)
        {
            var dest = await pathFinder.FindNextPath(currentTile, playerController.CurrentTile, enemyBlackboard.WillBeOccupied, false);
            return dest.Tile;
        }
    }
}
