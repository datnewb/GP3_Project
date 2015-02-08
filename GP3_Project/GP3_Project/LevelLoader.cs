using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GP3_Project
{
    static class LevelLoader
    {
        public static Level LoadedLevel = new Level();
        public static Texture2D LoadedLevelTexture = null;
        public static bool ScrollingLevelX = false;
        public static bool ScrollingLevelY = false;
        public static int LevelCenterX = 0;
        public static int LevelCenterY = 0;
        public static int LevelWidth = 0;
        public static int LevelHeight = 0;

        public static void LoadLevel(GraphicsDeviceManager graphics, ContentManager Content, Level level, Player player)
        {
            if (LoadedLevelTexture != null)
                LoadedLevelTexture.Dispose();

            Tile.LevelTiles = new List<Tile>();
            Enemy.Enemies = new List<Enemy>();

            //Check size of level
            int levelWidth = 0;
            int longestLevelWidth = 0;
            int levelHeight = 0;

            foreach (string line in level.levelTextFile)
            {
                levelWidth = 0;
                foreach (char tile in line)
                    levelWidth += Tile.TileSize;
                if (levelWidth > longestLevelWidth)
                {
                    longestLevelWidth = levelWidth;
                    LevelCenterX = longestLevelWidth / 2;
                }
                levelHeight += Tile.TileSize;
            }
            LevelCenterY = levelHeight / 2;

            LevelWidth = longestLevelWidth;
            LevelHeight = levelHeight;

            int startX = 0;
            int startY = 0;

            if (longestLevelWidth > graphics.PreferredBackBufferWidth)
            {
                startX = (graphics.PreferredBackBufferWidth - longestLevelWidth) / 2;
                ScrollingLevelX = true;
            }
            else
            {
                ScrollingLevelX = false;
            }

            if (levelHeight > graphics.PreferredBackBufferHeight)
            {
                startY = (graphics.PreferredBackBufferHeight - levelHeight) / 2;
                ScrollingLevelY = true;
            }
            else
            {
                ScrollingLevelY = false;
            }

            //Create tiles
            int tileXCoord = 0;
            int tileYCoord = 0;
            foreach (string line in level.levelTextFile)
            {
                tileXCoord = 0;
                foreach (char tile in line)
                {
                    switch (tile)
                    {
                        case 'S': //Start Tile
                            player.Rect = new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize);
                            goto case 'O';
                        case 'E':
                            Enemy.Enemies.Add(new Enemy(graphics.GraphicsDevice, new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize)));
                            goto case 'O';
                        case 'O': //Empty Tile
                            Tile.LevelTiles.Add(new Tile(
                                new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize),
                                TileType.Empty));
                            break;
                        case 'X': //Single Wall Tile
                            Tile.LevelTiles.Add(new Tile(
                                new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize),
                                TileType.Wall));
                            break;
                        case 'x': //1.5 Wall Tile
                            Tile.LevelTiles.Add(new Tile(
                                new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize * 3 / 2),
                                TileType.Wall));
                            break;
                        default:
                            if (char.IsDigit(tile))
                            {
                                int nextLevelIndex = int.Parse(char.ToString(tile));
                                if (nextLevelIndex < level.NextLevels.Count)
                                {
                                    Tile.LevelTiles.Add(new NextLevelTile(
                                        graphics, 
                                        Content, 
                                        level.NextLevels[nextLevelIndex], 
                                        new Rectangle(tileXCoord, tileYCoord, Tile.TileSize, Tile.TileSize), 
                                        TileType.Exit));
                                }
                            }
                            break;

                    }
                    tileXCoord += Tile.TileSize;
                }
                tileYCoord += Tile.TileSize;
            }

            LoadedLevel = level;
            LoadedLevelTexture = Content.Load<Texture2D>(level.levelTextureDirectory);
        }
    }
}
