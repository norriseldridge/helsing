namespace Helsing.Client.World.Api
{
    public class TransientPathNodeData
    {
        public bool isVisited;
        public float localGoal;
        public float globalGoal;
        public TransientPathNodeData parent;
        public ITile Tile { get; private set; }

        public TransientPathNodeData(bool visited, ITile tile) =>
            (isVisited, Tile, localGoal, globalGoal, parent) = (visited, tile, float.PositiveInfinity, 0, null);

        public void Reset()
        {
            localGoal = float.PositiveInfinity;
            globalGoal = 0;
            isVisited = false;
            parent = null;
        }
    }
}
