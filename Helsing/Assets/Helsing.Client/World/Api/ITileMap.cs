using System.Collections.Generic;
using UnityEngine;

namespace Helsing.Client.World.Api
{
    public interface ITileMap
    {
        IEnumerable<Tile> Tiles { get; }
        Tile TileAt(Vector2 position);
    }
}