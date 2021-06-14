using System.Threading.Tasks;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Enemy.Api
{
    public interface IEnemyLogic
    {
        Task<ITile> PickDestinationTile(ITile currentTile);
    }
}
