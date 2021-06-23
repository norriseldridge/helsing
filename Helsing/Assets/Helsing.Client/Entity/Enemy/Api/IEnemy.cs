using Helsing.Client.Core.Api;
using Helsing.Client.Entity.Api;
using Helsing.Client.Item.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemy : ITurnTaker
    {
        EnemyLogicType EnemyLogicType { get; }
        ITileMover TileMover { get; }
        int MaxMoves { get; }
        IItemData KillItem { get; }
    }
}