using Helsing.Client.Core.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemy : ITurnTaker
    {
        EnemyLogicType EnemyLogicType { get; }
    }
}