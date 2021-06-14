using Helsing.Client.Entity.Enemy.Api;
using Zenject;

namespace Helsing.Client.Entity.Enemy
{
    public class EnemyLogicFactory : IFactory<IEnemyLogic>
    {
        private DiContainer container;

        public EnemyLogicFactory(DiContainer container) => this.container = container;

        // TODO pass in the "type"
        public IEnemyLogic Create()
        {
            IEnemyLogic enemyLogic = new BasicEnemyLogic();
            container.Inject(enemyLogic);
            return enemyLogic;
        }
    }
}
