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
        public Level NextLevel;

        public Level(string[] levelTextFile, string levelTextureDirectory)
        {
            NextLevel = null;
            this.levelTextFile = levelTextFile;
            this.levelTextureDirectory = levelTextureDirectory;
        }
    }
}
