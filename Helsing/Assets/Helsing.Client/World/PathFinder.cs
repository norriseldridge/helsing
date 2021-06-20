using System.Collections.Generic;
using System.Threading.Tasks;
using Helsing.Client.World.Api;
using UnityEngine;

namespace Helsing.Client.World
{
    public class PathFinder : IPathFinder
    {
        public IList<ITile> Ignore { get; set; }
        public bool OnlyFloors { get; set; }

        Dictionary<ITile, TransientPathNodeData> pathNodes = new Dictionary<ITile, TransientPathNodeData>();

        public async Task<TransientPathNodeData> FindNextPath(ITile start, ITile end)
        {
            var (next, _) = await SolvePath(start, end);
            return next;
        }

        public async Task<(TransientPathNodeData data, int distance)> FindNextPathAndDistance(ITile start, ITile end) =>
            await SolvePath(start, end);

        private Task<(TransientPathNodeData data, int distance)> SolvePath(ITile start, ITile end)
        {
            Reset();

            var startNode = GetPathNode(start);
            startNode.localGoal = 0;
            startNode.globalGoal = Heuristic(start, end);

            if (start == end)
                return Task.FromResult<(TransientPathNodeData data, int distance)>((startNode, 0));

            var endNode = GetPathNode(end);

            var toTest = new List<TransientPathNodeData>();
            toTest.Add(startNode);

            while (toTest.Count > 0 && endNode.parent == null)
            {
                toTest.Sort((TransientPathNodeData l, TransientPathNodeData r) => (l.globalGoal < r.globalGoal) ? -1 : 1);

                // confirm the first is less than less
                if (toTest[0].globalGoal > toTest[toTest.Count - 1].globalGoal)
                    throw new System.Exception($"Element at 0 was greater than element at {toTest.Count - 1}! Sort is wrong!");

                // don't double check nodes we've looked at before
                if (toTest[0].isVisited)
                {
                    toTest.RemoveAt(0);
                    continue;
                }

                var current = toTest[0];
                current.isVisited = true;

                foreach (var n in current.Tile.Neighbors)
                {
                    if (Ignore != null && Ignore.Contains(n))
                        continue;

                    var neighbor = GetPathNode(n);
                    if (!neighbor.isVisited && (!OnlyFloors || (OnlyFloors && n.IsFloor)))
                        toTest.Add(neighbor);

                    float possibleLowerLocalGoal = current.localGoal + Heuristic(current.Tile, neighbor.Tile);

                    if (possibleLowerLocalGoal < neighbor.localGoal)
                    {
                        neighbor.parent = current;
                        neighbor.localGoal = possibleLowerLocalGoal;
                        neighbor.globalGoal = neighbor.localGoal + Heuristic(neighbor.Tile, end);
                    }
                }
            }

            // we didn't find a path so there is no next step
            if (endNode.parent == null)
                return Task.FromResult<(TransientPathNodeData data, int distance)>((null, 0));

            var count = 0;
            var next = endNode;
            while (next.parent != startNode)
            {
                next = next.parent;

                ++count;
            }

            return Task.FromResult<(TransientPathNodeData data, int distance)>((next, count));
        }

        private float Heuristic(ITile current, ITile target)
        {
            // TODO do we account for other goals? how? dangerous tiles? etc.
            return Vector2.Distance(current.Position, target.Position);
        }

        private TransientPathNodeData GetPathNode(ITile tile)
        {
            if (!pathNodes.ContainsKey(tile))
                pathNodes[tile] = new TransientPathNodeData(false, tile);
            return pathNodes[tile];
        }

        private void Reset()
        {
            foreach (var v in pathNodes.Values)
                v.Reset();
        }
    }
}
