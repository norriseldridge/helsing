using System.Collections.Generic;
using UnityEngine;

namespace Helsing.Client.World.Api
{
    public interface ITile
    {
        IEnumerable<ITile> Neighbors { get; }
        void FindNeighbors(IEnumerable<ITile> tiles);
        ITile GetNeighbor(Direction direction);
        ITile GetRandomNeighbor(bool floorsOnly);
        Vector3 Position { get; }
        bool IsFloor { get; }
        bool IsHidingSpot { get; }
        IEnumerable<GameObject> GetGameObjectsOnTile();
    }
}