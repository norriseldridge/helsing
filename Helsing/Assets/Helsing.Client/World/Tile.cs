using System.Collections.Generic;
using Helsing.Client.World.Api;
using UnityEngine;

namespace Helsing.Client.World
{
    [ExecuteAlways]
    public class Tile : MonoBehaviour, ITile
    {
        const float NEIGHBOR_RANGE = 1f;

        [SerializeField]
        bool isFloor;

        public Vector3 Position => transform.position;
        public IEnumerable<ITile> Neighbors => neighbors.Values;
        Dictionary<Direction, Tile> neighbors;

        public bool IsFloor
        {
            get => isFloor;
            set => isFloor = value;
        }

        public void FindNeighbors(IEnumerable<ITile> tiles) => FindNeighbors(tiles as List<Tile>);

        public void FindNeighbors(IEnumerable<Tile> tiles)
        {
            if (neighbors != null) neighbors.Clear();
            neighbors = new Dictionary<Direction, Tile>();

            foreach (var tile in tiles)
            {
                if (tile != this && Vector3.Distance(transform.position, tile.transform.position) <= NEIGHBOR_RANGE)
                {
                    Vector2 dir = tile.transform.position - transform.position;
                    neighbors[dir.ToDirection()] = tile;
                }
            }
        }

        public ITile GetNeighbor(Direction direction) =>
            neighbors.ContainsKey(direction) ? neighbors[direction] : null;

#if UNITY_EDITOR
        private void Update()
        {
            // snap to grid
            var x = Mathf.RoundToInt(transform.position.x);
            var y = Mathf.RoundToInt(transform.position.y);
            var z = Mathf.RoundToInt(transform.position.z);
            transform.position = new Vector3(x, y, z);
        }

        private void OnDrawGizmosSelected()
        {
            if (Neighbors == null)
                return;

            // highlight self
            Gizmos.color = new Color(isFloor ? 0 : 1, isFloor ? 1 : 0, 0, 0.25f);
            Gizmos.DrawCube(transform.position, Vector3.one);

            // outline neighbors
            if (Application.isPlaying)
                Gizmos.color = new Color(0, 1, 0, 0.15f + Mathf.Sin(Time.time * 4) * 0.15f);
            foreach (var neighbor in neighbors.Values)
            {
                Gizmos.DrawWireCube(neighbor.transform.position, Vector3.one);
            }
        }
#endif
    }
}