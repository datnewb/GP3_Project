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
    }
}
