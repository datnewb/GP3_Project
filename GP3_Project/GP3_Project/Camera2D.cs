using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class Camera2D
    {
        public Matrix Transform;
        public Viewport View;
        Vector2 Center;

        public Camera2D(Viewport view)
        {
            View = view;
            Center = Vector2.Zero;
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (LevelLoader.ScrollingLevelX)
            {
                Center.X = player.Rect.Center.X - View.Width / 2;
            }
            else
            {
                Center.X = LevelLoader.LevelCenterX - View.Width / 2;
            }

            if (LevelLoader.ScrollingLevelY)
            {
                Center.Y = player.Rect.Center.Y - View.Height / 2;
            }
            else
            {
                Center.Y = LevelLoader.LevelCenterY - View.Height / 2;
            }

            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-Center.X, -Center.Y, 0));
        }
    }
}
