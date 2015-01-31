using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GP3_Project
{
    class Level
    {
        public static List<Level> Levels = new List<Level>();

        public string[] levelTextFile;
        public List<Level> NextLevels;
        public List<Rectangle> NextLevelStart;

        public Level(string[] levelTextFile)
        {
            NextLevels = new List<Level>();
            NextLevelStart = new List<Rectangle>();
            this.levelTextFile = levelTextFile;
        }
    }
}
