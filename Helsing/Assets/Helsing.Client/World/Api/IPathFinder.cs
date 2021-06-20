using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helsing.Client.World.Api
{
    public interface IPathFinder
    {
        IList<ITile> Ignore { get; set; }
        bool OnlyFloors { get; set; }
        Task<TransientPathNodeData> FindNextPath(ITile start, ITile end);
        Task<(TransientPathNodeData data, int distance)> FindNextPathAndDistance(ITile start, ITile end);
    }
}
