using Zenject;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyLogicFactory : IFactory<EnemyLogicType, IEnemyLogic>
    {
    }
}
