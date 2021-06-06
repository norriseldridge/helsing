using System.Collections.Generic;
using UnityEngine;

namespace Helsing.Client.World.Api
{
    public interface ITile
    {
        IEnumerable<ITile> Neighbors { get; }
        void FindNeighbors(IEnumerable<ITile> tiles);
        ITile GetNeighbor(Direction direction);
        Vector3 Position { get; }
        bool IsFloor { get; }
        IEnumerable<GameObject> GetGameObjectsOnTile();
    }
}