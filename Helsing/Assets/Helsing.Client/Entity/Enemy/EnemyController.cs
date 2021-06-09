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
    public class EnemyController : MonoBehaviour, IEnemy
    {
        [SerializeField]
        EntityView view;

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
            for (var i = 0; i < moveCount; ++i)
            {
                target = null;
                if (await CanSeePlayer())
                {
                    var dest = await pathFinder.FindNextPath(tileMover.CurrentTile.Value, playerController.CurrentTile, enemyBlackboard.WillBeOccupied);
                    if (dest != null)
                    {
                        target = dest.Tile;
                    }
                }

                if (target == null)
                    target = tileMover.CurrentTile.Value.GetRandomNeighbor(true);

                enemyBlackboard.SetWillBeOccupied(target);
                view.FlipX = target.Position.x < transform.position.x;
                view.State = EntityState.Walk;
                await tileMover.MoveTo(target);
                view.State = EntityState.Idle;
                enemyBlackboard.ClearWillBeOccupied(target);
            }

            if (target != null)
                enemyBlackboard.SetWillBeOccupied(target);
        }

        private async Task<bool> CanSeePlayer()
        {
            if (playerController != null && !playerController.IsHidden)
            {
                var (path, dist) = await pathFinder.FindNextPathAndDistance(tileMover.CurrentTile.Value, playerController.CurrentTile);
                if (path != null)
                {
                    return dist < 5;
                }
            }

            return false;
        }
    }
}