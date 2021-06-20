using System.Collections.Generic;
using System.Threading.Tasks;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyCoordinator
    {
        public Task EveryTurn(EnemyLogicType logicType, IEnemy enemy);
        public Task<IEnumerable<ITile>> GetMoves(EnemyLogicType logicType, IEnemy enemy);
    }
}
