using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
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
        ITileMap tileMap;

        [Inject]
        private void Inject(IPathFinder pathFinder, ITileMap tileMap) =>
            (this.pathFinder, this.tileMap) = (pathFinder, tileMap);

        private void Awake()
        {
            tileMover = GetComponent<ITileMover>();
        }

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