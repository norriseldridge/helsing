using Helsing.Client.Entity.Enemy.Api;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class EnemyLogicPlaceholderFactory : PlaceholderFactory<EnemyLogicType, IEnemyLogic> { }

    public class EnemyLogicFactory : IEnemyLogicFactory
    {
        private DiContainer container;

        public EnemyLogicFactory(DiContainer container) => this.container = container;

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
