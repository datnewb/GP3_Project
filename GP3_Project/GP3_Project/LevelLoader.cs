using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GP3_Project
{
    class LevelLoader
    {
        public static Level LoadedLevel = new Level();
        public static bool ScrollingLevelX = false;
        public static bool ScrollingLevelY = false;
        public static int LevelCenterX = 0;
        public static int LevelCenterY = 0;

        public static void LoadLevel(GraphicsDeviceManager graphics, ContentManager Content, Level level, Player player)
        {
            Tile.LevelTiles.Clear();

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
                                graphics.GraphicsDevice,
                                new Vector2(tileXCoord, tileYCoord),
                                Content.Load<Texture2D>(@"Textures\Wall"),
                                TileType.Empty));
                            break;
                        case 'X': //Solid Tile
                            Tile.LevelTiles.Add(new Tile(
                                graphics.GraphicsDevice,
                                new Vector2(tileXCoord, tileYCoord),
                                Content.Load<Texture2D>(@"Textures\Floor"),
                                TileType.Wall));
                            break;
                        case 'F': //Next Level Tile
                            break;
                    }
                    tileXCoord += Tile.TileSize;
                }
                tileYCoord += Tile.TileSize;
            }

            LoadedLevel = level;
        }
    }
}
