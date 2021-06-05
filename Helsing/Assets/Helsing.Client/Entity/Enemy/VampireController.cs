using System.Threading.Tasks;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.World.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
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

        [SerializeField]
        TileMover tileMover;

        int turnIndex = 0;
        IPathFinder pathFinder;
        ITileMap tileMap;

        [Inject]
        private void Inject(IPathFinder pathFinder, ITileMap tileMap) =>
            (this.pathFinder, this.tileMap) = (pathFinder, tileMap);

        public async Task TakeTurn()
        {
            ++turnIndex;
            if (turnIndex < turnDelay) return;

            turnIndex = 0;

            for (var i = 0; i < moveCount; ++i)
            {
                var dest = pathFinder.FindNextPath(tileMover.CurrentTile.Value, tileMap.TileAt(new Vector2(6, 0)));
                if (dest != null)
                {
                    view.State = EntityState.Walk;
                    await tileMover.MoveTo(dest.Tile);
                    view.State = EntityState.Idle;
                }
            }
        }
    }
}