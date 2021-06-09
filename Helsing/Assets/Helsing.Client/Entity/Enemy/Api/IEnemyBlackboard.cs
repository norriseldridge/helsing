using System.Collections.Generic;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyBlackboard
    {
        IList<ITile> WillBeOccupied { get; }
        void Clear();
        void SetWillBeOccupied(ITile tile);
        void ClearWillBeOccupied(ITile tile);
    }
}
