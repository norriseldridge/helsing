using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    [RequireComponent(typeof(ILiving))]
    [RequireComponent(typeof(ITileMover))]
    public class VampireController : MonoBehaviour, IEnemy
    {
        [SerializeField]
        VampireView view;

        [SerializeField]
        [Min(0)]
        int turnDelay;

        [SerializeField]
        [Min(1)]
        int moveCount;

        int turnIndex = 0;
        ITileMover tileMover;
        IPathFinder pathFinder;
        IEnemyBlackboard enemyBlackboard;
        IPlayerController playerController;

        [Inject]
        private void Inject(IPathFinder pathFinder, IEnemyBlackboard enemyBlackboard, IPlayerController playerController) =>
            (this.pathFinder, this.enemyBlackboard, this.playerController) = (pathFinder, enemyBlackboard, playerController);

        private void Awake()
        {
            tileMover = GetComponent<ITileMover>();
        }

        public async Task TakeTurn()
        {
            ++turnIndex;
            if (turnIndex < turnDelay) return;

            turnIndex = 0;

            ITile target = null;
            if (CanSeePlayer())
            {
                for (var i = 0; i < moveCount; ++i)
                {
                    var dest = pathFinder.FindNextPath(tileMover.CurrentTile.Value, playerController.CurrentTile, enemyBlackboard.WillBeOccupied);
                    if (dest != null)
                    {
                        target = dest.Tile;
                    }
                }
            }

            if (target == null)
                target = tileMover.CurrentTile.Value.GetRandomNeighbor(true);

            view.FlipX = target.Position.x < transform.position.x;
            enemyBlackboard.SetWillBeOccupied(target);
            view.State = EntityState.Walk;
            await tileMover.MoveTo(target);
            view.State = EntityState.Idle;
        }

        private bool CanSeePlayer()
        {
            if (playerController != null && !playerController.IsHidden)
            {
                var (path, dist) = pathFinder.FindNextPathAndDistance(tileMover.CurrentTile.Value, playerController.CurrentTile);
                if (path != null)
                {
                    return dist < 5;
                }
            }

            return false;
        }
    }
}