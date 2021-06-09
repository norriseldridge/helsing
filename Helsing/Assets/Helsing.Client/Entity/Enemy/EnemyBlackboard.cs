using System.Collections.Generic;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy
{
    public class EnemyBlackboard : IEnemyBlackboard
    {
        public IList<ITile> WillBeOccupied => occupied;
        List<ITile> occupied = new List<ITile>();

        public void Clear()
        {
            occupied.Clear();
        }

        public void SetWillBeOccupied(ITile tile)
        {
            if (!occupied.Contains(tile))
                occupied.Add(tile);
        }

        public void ClearWillBeOccupied(ITile tile) => occupied.Remove(tile);
    }
}
