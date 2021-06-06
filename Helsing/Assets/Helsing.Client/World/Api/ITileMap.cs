using System.Collections.Generic;
using UnityEngine;

namespace Helsing.Client.World.Api
{
    public interface ITileMap
    {
        IEnumerable<ITile> Tiles { get; }
        ITile TileAt(Vector2 position);
    }
}