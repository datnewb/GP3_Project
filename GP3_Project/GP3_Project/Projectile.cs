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
        public Enemy caster;

        private int Speed;

        public static Texture2D ProjectileSpriteSheet;
        public Rectangle textureSourceRectangle;
        public static int textureWidthInterval;
        public static int textureHeightInterval;

        private int currentFrameX;
        private int frameDelay;
        private int currentFrameDelay;

        public Projectile (Rectangle StartRect, Direction direction, Enemy caster)
        {
            Rect = StartRect;
            this.direction = direction;

            this.caster = caster;

            Speed = 5;

            frameDelay = 10;
            currentFrameDelay = 0;
            currentFrameX = 0;
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

                projectile.Animate();
            }

            foreach (Projectile projectile in ProjectilesToBeRemoved)
            {
                Projectiles.Remove(projectile);
            }
        }

        private void Animate()
        {
            currentFrameDelay++;
            if (currentFrameDelay > frameDelay)
            {
                currentFrameDelay = 0;
                currentFrameX++;
                if (currentFrameX > 2)
                    currentFrameX = 0;
            }

            textureSourceRectangle = new Rectangle(textureWidthInterval * currentFrameX, textureHeightInterval * (int)direction, textureWidthInterval, textureHeightInterval);
        }
    }
}
