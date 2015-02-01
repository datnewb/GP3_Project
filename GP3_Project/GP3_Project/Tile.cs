using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    public enum TileType
    {
        Empty,
        Wall,
        Exit
    }

    class Tile
    {
        public static int TileSize = 42;
        public static List<Tile> LevelTiles = new List<Tile>();

        public Rectangle Rect;
        public TileType tileType;

        public Tile(Rectangle Rect, TileType tileType)
        {
            this.Rect = Rect;
            
            LevelTiles.Add(this);
            this.tileType = tileType;
        }
    }
}
