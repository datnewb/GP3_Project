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
        public static int TileSize = 32;
        public static List<Tile> LevelTiles = new List<Tile>();

        public Texture2D tileTexture;
        public Vector2 screenPosition;
        public Rectangle Rect;
        public TileType tileType;

        public Tile(GraphicsDevice graphicsDevice, Vector2 position, Color tileColor, TileType tileType)
        {
            Color[] textureColor = new Color[1];
            textureColor[0] = tileColor;
            tileTexture = new Texture2D(graphicsDevice, 1, 1);
            tileTexture.SetData(textureColor);

            screenPosition = position;
            Rect = new Rectangle((int)(position.X), (int)(position.Y), TileSize, TileSize);
            
            LevelTiles.Add(this);
            this.tileType = tileType;
        }

        public Tile(GraphicsDevice graphicsDevice, Vector2 position, Texture2D tileTexture, TileType tileType)
        {
            this.tileTexture = tileTexture;

            screenPosition = position;
            Rect = new Rectangle((int)(position.X), (int)(position.Y), TileSize, TileSize);
            this.tileType = tileType;
            LevelTiles.Add(this);
        }
    }
}
