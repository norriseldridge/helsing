using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Extensions;
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
        EnemyLogicType enemyType;

        [SerializeField]
        [Min(0)]
        int turnDelay;

        [SerializeField]
        [Min(1)]
        int moveCount;

        public EnemyLogicType EnemyLogicType => enemyType;

        int turnIndex = 0;
        ITileMover tileMover;
        IEnemyCoordinator enemyCoordinator;

        [Inject]
        private void Inject(IEnemyCoordinator enemyCoordinator) =>
            this.enemyCoordinator = enemyCoordinator;

        private void Awake()
        {
            tileMover = GetComponent<ITileMover>();
        }

        public async Task TakeTurn()
        {
            ++turnIndex;
            if (turnIndex <= turnDelay) return;
            turnIndex = 0;

            var moves = await enemyCoordinator.GetMoves(enemyType, moveCount, tileMover.CurrentTile.Value);
            view.State = EntityState.Walk;
            await moves.AsyncForEach(async m =>
            {
                view.FlipX = m.Position.x < transform.position.x;
                await tileMover.MoveTo(m);
            });
            view.State = EntityState.Idle;
        }
    }
}