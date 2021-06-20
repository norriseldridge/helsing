using Helsing.Client.Core.Api;
using Helsing.Client.Entity.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemy : ITurnTaker
    {
        EnemyLogicType EnemyLogicType { get; }
        ITileMover TileMover { get; }
        int MaxMoves { get; }
        IEnemyControllerBlackboard Blackboard { get; }
    }
}