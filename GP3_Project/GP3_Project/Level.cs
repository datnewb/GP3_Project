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
        public string levelTextureDirectory;
        public List<Level> NextLevels;
        public List<Rectangle> NextLevelStart;

        public Level(string[] levelTextFile, string levelTextureDirectory)
        {
            NextLevels = new List<Level>();
            NextLevelStart = new List<Rectangle>();
            this.levelTextFile = levelTextFile;
            this.levelTextureDirectory = levelTextureDirectory;
        }

        public Level()
        {
            NextLevels = new List<Level>();
            NextLevelStart = new List<Rectangle>();
            levelTextFile = new string[0];
            levelTextureDirectory = "";
        }
    }
}
