using System.Collections.Generic;
using System.Threading.Tasks;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Extensions;
using Helsing.Client.Item;
using Helsing.Client.Item.Api;
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

        [SerializeField]
        ItemData killItem;

        public EnemyLogicType EnemyLogicType => enemyType;
        public ITileMover TileMover => tileMover;
        public int MaxMoves => moveCount;
        public IItemData KillItem => killItem;

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
            await enemyCoordinator.EveryTurn(enemyType, this);

            ++turnIndex;
            if (turnIndex <= turnDelay) return;
            turnIndex = 0;

            var moves = await enemyCoordinator.GetMoves(enemyType, this);
            view.State = EntityState.Walk;
            await moves.AsyncForEach(async m =>
            {
                if (this == null)
                    return;
                if (view != null)
                    view.FlipX = m.Position.x < transform.position.x;
                if (tileMover != null)
                    await tileMover.MoveTo(m);
            });
            view.State = EntityState.Idle;
        }
    }
}