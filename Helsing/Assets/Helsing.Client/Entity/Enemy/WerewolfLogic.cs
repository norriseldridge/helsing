using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class WerewolfLogic : IEnemyLogic
    {
        IPlayerController playerController;
        Direction lastDirection;
        int directionCountDown;

        public bool CanShareTile => false;

        [Inject]
        private void Inject(IPlayerController playerController) =>
            (this.playerController) = (playerController);

        public Task EveryTurn(IEnemy enemy) => Task.CompletedTask;

        public Task<ITile> PickDestinationTile(ITile currentTile)
        {
            if (playerController.Living.Lives <= 0)
                return Task.FromResult(currentTile);

            directionCountDown--;
            if (directionCountDown <= 0)
            {
                directionCountDown = 3;
                lastDirection = GetDirectionTo(currentTile, playerController.CurrentTile);
            }
            
            var tile = currentTile.GetNeighbor(lastDirection);
            if (!tile.IsFloor)
            {
                if (directionCountDown < 3)
                {
                    tile = currentTile;
                }
                else
                {
                    foreach (var neighbor in currentTile.Neighbors)
                    {
                        if (neighbor.IsFloor)
                        {
                            tile = neighbor;
                            lastDirection = GetDirectionTo(currentTile, tile);
                            break;
                        }
                    }
                }
            }

            return Task.FromResult(tile);
        }

        private Direction GetDirectionTo(ITile currentTile, ITile target)
        {
            Vector2 dirVector = target.Position - currentTile.Position;
            dirVector.Normalize();
            if (Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y))
            {
                dirVector.y = 0;
            }
            else
            {
                dirVector.x = 0;
            }
            return dirVector.ToDirection();
        }
    }
}
