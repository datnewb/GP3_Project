using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class Level
    {
        public static List<Level> Levels = new List<Level>();

        public string[] levelTextFile;
        public List<Level> NextLevels;
        public List<Rectangle> NextLevelStart;
        public Texture2D levelTexture;

        public Level(string[] levelTextFile, Texture2D levelTexture)
        {
            NextLevels = new List<Level>();
            NextLevelStart = new List<Rectangle>();
            this.levelTextFile = levelTextFile;
            this.levelTexture = levelTexture;
        }

        public Level()
        {
            NextLevels = new List<Level>();
            NextLevelStart = new List<Rectangle>();
            levelTextFile = new string[0];
            levelTexture = null;
        }
    }
}
