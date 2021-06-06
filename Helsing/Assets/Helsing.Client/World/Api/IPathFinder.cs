using System.Collections.Generic;

namespace Helsing.Client.World.Api
{
    public interface IPathFinder
    {
        TransientPathNodeData FindNextPath(ITile start, ITile end, IList<ITile> ignore = null);
        (TransientPathNodeData data, int distance) FindNextPathAndDistance(ITile start, ITile end, IList<ITile> ignore = null);
    }
}
