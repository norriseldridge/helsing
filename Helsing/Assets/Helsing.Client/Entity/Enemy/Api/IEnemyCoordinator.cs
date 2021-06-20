using System.Collections.Generic;
using System.Threading.Tasks;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyCoordinator
    {
        public Task<IEnumerable<ITile>> GetMoves(EnemyLogicType logicType, int maxMoves, ITile startingTile);
    }
}
