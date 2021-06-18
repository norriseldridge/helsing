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

        int turnIndex = 0;
        ITileMover tileMover;
        EnemyLogicFactory enemyLogicFactory;
        IEnemyLogic enemyLogic;
        IEnemyBlackboard enemyBlackboard;

        [Inject]
        private void Inject(EnemyLogicFactory enemyLogicFactory, IEnemyBlackboard enemyBlackboard) =>
            (this.enemyLogicFactory, this.enemyBlackboard) = (enemyLogicFactory, enemyBlackboard);

        private void Awake()
        {
            tileMover = GetComponent<ITileMover>();
            enemyLogic = enemyLogicFactory.Create(enemyType);
        }

        public async Task TakeTurn()
        {
            ++turnIndex;
            if (turnIndex < turnDelay) return;
            turnIndex = 0;

            ITile target = null;
            for (var i = 0; i < moveCount; ++i)
            {
                target = await enemyLogic.PickDestinationTile(tileMover.CurrentTile.Value);
                view.FlipX = target.Position.x < transform.position.x;
                view.State = EntityState.Walk;
                await tileMover.MoveTo(target);
                view.State = EntityState.Idle;
            }

            if (target != null)
                enemyBlackboard.SetWillBeOccupied(target);
        }
    }
}