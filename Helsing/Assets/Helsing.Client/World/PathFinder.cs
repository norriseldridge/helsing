using System.Collections.Generic;
using Helsing.Client.World.Api;
using UnityEngine;

namespace Helsing.Client.World
{
    public class PathFinder : IPathFinder
    {
        Dictionary<ITile, TransientPathNodeData> pathNodes = new Dictionary<ITile, TransientPathNodeData>();

        public TransientPathNodeData FindNextPath(ITile start, ITile end)
        {
            Reset();

            var startNode = GetPathNode(start);
            startNode.localGoal = 0;
            startNode.globalGoal = Heuristic(start, end);

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
                    var neighbor = GetPathNode(n);
                    if (!neighbor.isVisited && n.IsFloor)
                        toTest.Add(neighbor);

                    float possibleLowerLocalGoal = current.localGoal + Vector2.Distance(current.Tile.Position, neighbor.Tile.Position);

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
                return null;

            var next = endNode;
            while (next.parent != startNode)
            {
                next = next.parent;
            }

            return next;
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
