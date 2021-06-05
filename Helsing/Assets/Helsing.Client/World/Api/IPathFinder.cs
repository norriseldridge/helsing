namespace Helsing.Client.World.Api
{
    public interface IPathFinder
    {
        TransientPathNodeData FindNextPath(ITile start, ITile end);
    }
}
