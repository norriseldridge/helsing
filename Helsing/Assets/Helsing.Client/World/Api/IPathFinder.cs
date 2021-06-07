using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helsing.Client.World.Api
{
    public interface IPathFinder
    {
        Task<TransientPathNodeData> FindNextPath(ITile start, ITile end, IList<ITile> ignore = null);
        Task<(TransientPathNodeData data, int distance)> FindNextPathAndDistance(ITile start, ITile end, IList<ITile> ignore = null);
    }
}
