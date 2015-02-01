using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GP3_Project
{
    class NextLevelTile : Tile
    {
        public Level nextLevel;
        public bool offNextLevelTile;
        public GraphicsDeviceManager graphics;
        public ContentManager Content;

        public NextLevelTile(GraphicsDeviceManager graphics, ContentManager Content, Level nextLevel, Rectangle Rect, TileType tileType) 
            : base (Rect, tileType)
        {
            this.graphics = graphics;
            this.Content = Content;

            offNextLevelTile = false;

            this.nextLevel = nextLevel;
        }

        public void CheckExit(Player player, ref bool shouldBreak)
        {
            if (player.Rect.Intersects(Rect))
            {
                LevelLoader.LoadLevel(graphics, Content, nextLevel, player);
                shouldBreak = true;
            }
        }
    }
}
