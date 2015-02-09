using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class Projectile
    {
        public static List<Projectile> Projectiles = new List<Projectile>();

        public Rectangle Rect;
        public Direction direction;
        public Texture2D texture;
        public Enemy caster;

        private int Speed;

        public Projectile (GraphicsDevice graphicsDevice, Rectangle StartRect, Direction direction, Enemy caster)
        {
            Rect = StartRect;
            this.direction = direction;

            Color[] textureColor = new Color[1];
            textureColor[0] = Color.Red;
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(textureColor);

            this.caster = caster;

            Speed = 5;
        }

        public static void UpdateProjectiles(Player player, GameTime gameTime)
        {
            List<Projectile> ProjectilesToBeRemoved = new List<Projectile>();
            foreach (Projectile projectile in Projectiles)
            {
                int speedX = 0;
                int speedY = 0;
                switch (projectile.direction)
                {
                    case Direction.Left:
                        speedX = -projectile.Speed;
                        break;
                    case Direction.Right:
                        speedX = projectile.Speed;
                        break;
                    case Direction.Up:
                        speedY = -projectile.Speed;
                        break;
                    case Direction.Down:
                        speedY = projectile.Speed;
                        break;
                }

                Physics.WallDetection(ref projectile.Rect, ref speedX, ref speedY);
                Physics.EnemyExitBlocking(ref projectile.Rect, ref speedX, ref speedY);

                projectile.Rect.X += speedX;
                projectile.Rect.Y += speedY;

                if (speedX == 0 && speedY == 0)
                {
                    ProjectilesToBeRemoved.Add(projectile);
                    break;
                }

                if (projectile.Rect.Intersects(player.Rect))
                {
                    player.Damage(projectile.caster, gameTime);
                    player.knockbackDirection = projectile.direction;
                    ProjectilesToBeRemoved.Add(projectile);
                }
            }

            foreach (Projectile projectile in ProjectilesToBeRemoved)
            {
                Projectiles.Remove(projectile);
            }
        }
    }
}
