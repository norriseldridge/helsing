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

        public bool CanShareTile => true;

        [Inject]
        private void Inject(IPlayerController playerController, IPathFinder pathFinder)
        {
            this.playerController = playerController;
            this.pathFinder = pathFinder;
            this.pathFinder.OnlyFloors = false;
        }

        public Task EveryTurn(IEnemy enemy) => Task.CompletedTask;

        public async Task<ITile> PickDestinationTile(ITile currentTile)
        {
            var dest = await pathFinder.FindNextPath(currentTile, playerController.CurrentTile);
            return dest.Tile;
        }
    }
}
