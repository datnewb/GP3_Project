using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GP3_Project
{
    static class Physics
    {
        public static void WallDetection(ref Rectangle Rect, ref int currentSpeedX, ref int currentSpeedY)
        {
            foreach (Tile tile in Tile.LevelTiles)
            {
                if (tile.tileType == TileType.Wall)
                {
                    if (currentSpeedX != 0)
                    {
                        if (Rect.Top <= tile.Rect.Bottom &&
                            Rect.Bottom >= tile.Rect.Top)
                        {
                            if (currentSpeedX > 0 && Rect.Right <= tile.Rect.Left)
                            {
                                if (Rect.Right + currentSpeedX >= tile.Rect.Left)
                                {
                                    Rect.X = tile.Rect.X - Rect.Width - 1;
                                    currentSpeedX = 0;
                                    break;
                                }
                            }
                            else if (currentSpeedX < 0 && Rect.Left >= tile.Rect.Right)
                            {
                                if (Rect.Left + currentSpeedX <= tile.Rect.Right)
                                {
                                    Rect.X = tile.Rect.Right + 1;
                                    currentSpeedX = 0;
                                    break;
                                }
                            }
                        }
                    }

                    else if (currentSpeedY != 0)
                    {
                        if (Rect.Left <= tile.Rect.Right &&
                            Rect.Right >= tile.Rect.Left)
                        {
                            if (currentSpeedY > 0 && Rect.Bottom <= tile.Rect.Top)
                            {
                                if (Rect.Bottom + currentSpeedY >= tile.Rect.Top)
                                {
                                    Rect.Y = tile.Rect.Y - Rect.Height - 1;
                                    currentSpeedY = 0;
                                    break;
                                }
                            }
                            else if (currentSpeedY < 0 && Rect.Top >= tile.Rect.Bottom)
                            {
                                if (Rect.Top + currentSpeedY <= tile.Rect.Bottom)
                                {
                                    Rect.Y = tile.Rect.Bottom + 1;
                                    currentSpeedY = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void EnemyBlocking(ref Rectangle Rect, ref int currentSpeedX, ref int currentSpeedY)
        {
            foreach (Enemy enemy in Enemy.Enemies)
            {
                if (currentSpeedX != 0)
                {
                    if (Rect.Top <= enemy.Rect.Bottom &&
                        Rect.Bottom >= enemy.Rect.Top)
                    {
                        if (currentSpeedX > 0 && Rect.Right <= enemy.Rect.Left)
                        {
                            if (Rect.Right + currentSpeedX >= enemy.Rect.Left)
                            {
                                Rect.X = enemy.Rect.X - Rect.Width - 1;
                                currentSpeedX = 0;
                                break;
                            }
                        }
                        else if (currentSpeedX < 0 && Rect.Left >= enemy.Rect.Right)
                        {
                            if (Rect.Left + currentSpeedX <= enemy.Rect.Right)
                            {
                                Rect.X = enemy.Rect.Right + 1;
                                currentSpeedX = 0;
                                break;
                            }
                        }
                    }
                }

                else if (currentSpeedY != 0)
                {
                    if (Rect.Left <= enemy.Rect.Right &&
                        Rect.Right >= enemy.Rect.Left)
                    {
                        if (currentSpeedY > 0 && Rect.Bottom <= enemy.Rect.Top)
                        {
                            if (Rect.Bottom + currentSpeedY >= enemy.Rect.Top)
                            {
                                Rect.Y = enemy.Rect.Y - Rect.Height - 1;
                                currentSpeedY = 0;
                                break;
                            }
                        }
                        else if (currentSpeedY < 0 && Rect.Top >= enemy.Rect.Bottom)
                        {
                            if (Rect.Top + currentSpeedY <= enemy.Rect.Bottom)
                            {
                                Rect.Y = enemy.Rect.Bottom + 1;
                                currentSpeedY = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void EnemyExitBlocking(ref Rectangle Rect, ref int currentSpeedX, ref int currentSpeedY)
        {
            foreach (Tile tile in Tile.LevelTiles)
            {
                if (tile.tileType == TileType.Exit)
                {
                    if (currentSpeedX != 0)
                    {
                        if (Rect.Top <= tile.Rect.Bottom &&
                            Rect.Bottom >= tile.Rect.Top)
                        {
                            if (currentSpeedX > 0 && Rect.Right <= tile.Rect.Left)
                            {
                                if (Rect.Right + currentSpeedX >= tile.Rect.Left)
                                {
                                    Rect.X = tile.Rect.X - Rect.Width - 1;
                                    currentSpeedX = 0;
                                    break;
                                }
                            }
                            else if (currentSpeedX < 0 && Rect.Left >= tile.Rect.Right)
                            {
                                if (Rect.Left + currentSpeedX <= tile.Rect.Right)
                                {
                                    Rect.X = tile.Rect.Right + 1;
                                    currentSpeedX = 0;
                                    break;
                                }
                            }
                        }
                    }

                    else if (currentSpeedY != 0)
                    {
                        if (Rect.Left <= tile.Rect.Right &&
                            Rect.Right >= tile.Rect.Left)
                        {
                            if (currentSpeedY > 0 && Rect.Bottom <= tile.Rect.Top)
                            {
                                if (Rect.Bottom + currentSpeedY >= tile.Rect.Top)
                                {
                                    Rect.Y = tile.Rect.Y - Rect.Height - 1;
                                    currentSpeedY = 0;
                                    break;
                                }
                            }
                            else if (currentSpeedY < 0 && Rect.Top >= tile.Rect.Bottom)
                            {
                                if (Rect.Top + currentSpeedY <= tile.Rect.Bottom)
                                {
                                    Rect.Y = tile.Rect.Bottom + 1;
                                    currentSpeedY = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
