using Helsing.Client.Entity.Enemy.Api;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class EnemyLogicPlaceholderFactory : PlaceholderFactory<EnemyLogicType, IEnemyLogic> { }

    public class EnemyLogicFactory : IFactory<EnemyLogicType, IEnemyLogic>
    {
        private DiContainer container;

        public EnemyLogicFactory(DiContainer container) => this.container = container;

        // TODO pass in the "type"
        public IEnemyLogic Create(EnemyLogicType type)
        {
            IEnemyLogic enemyLogic;

            switch (type)
            {
                case EnemyLogicType.Werewolf:
                    enemyLogic = new WerewolfLogic();
                    break;

                case EnemyLogicType.Ghost:
                    enemyLogic = new GhostLogic();
                    break;

                case EnemyLogicType.Basic:
                default:
                    enemyLogic = new BasicEnemyLogic();
                    break;
            }

            container.Inject(enemyLogic);
            return enemyLogic;
        }
    }
}
