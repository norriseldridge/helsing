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
        [Min(0)]
        int turnDelay;

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
            var dest = pathFinder.FindNextPath(tileMover.CurrentTile.Value, tileMap.TileAt(new Vector2(6, 0)));
            if (dest != null)
                await tileMover.MoveTo(dest.Tile);
        }
    }
}