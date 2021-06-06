﻿using System.Collections.Generic;
using Helsing.Client.World.Api;
using UnityEngine;

namespace Helsing.Client.World
{
    [ExecuteAlways]
    public class WorldController : MonoBehaviour, ITileMap
    {
        [Header("Debug")]
        [SerializeField]
        bool editorDrawGizmos = false;

#if UNITY_EDITOR // only here to let rendering of the map happen in edit mode
        int frames = 0;

        public IEnumerable<ITile> Tiles => tiles;
        private List<Tile> tiles;

        private void Update()
        {
            if (editorDrawGizmos == false)
                return;

            if (frames == 0)
                InitializeTiles();
            else
            {
                frames++;

                if (frames > 15)
                    frames = 0;
            }
        }

        private void OnValidate()
        {
            if (editorDrawGizmos == false)
            {
                var tiles = FindObjectsOfType<Tile>();
                Tile[] empty = new Tile[0];
                foreach (var tile in tiles)
                {
                    tile.FindNeighbors(empty);
                }
            }
        }
#endif

        private void Start() => InitializeTiles();

        private void InitializeTiles()
        {
#if UNITY_EDITOR
            if (editorDrawGizmos == false)
                return;
#endif
            tiles = new List<Tile>(FindObjectsOfType<Tile>());
            foreach (var tile in Tiles)
            {
                tile.FindNeighbors(Tiles);
            }
        }

        public ITile TileAt(Vector2 position)
        {
            foreach (var tile in tiles)
            {
                if (Vector2.Distance(tile.transform.position, position) < 1)
                    return tile;
            }

            return null;
        }
    }
}