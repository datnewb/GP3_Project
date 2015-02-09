using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class Enemy
    {
        public static List<Enemy> Enemies = new List<Enemy>();
        public static List<Enemy> EnemiesToBeRemoved = new List<Enemy>();

        public Rectangle Rect;
        public int Health = 3;
        public int currentHealth;
        public bool damaged;

        public int movementSpeed;
        private int currentSpeedX;
        private int currentSpeedY;
        public Direction movementDirection;
        public int knockbackSpeed;

        internal float movementTime;
        internal float currentMovementTime;

        internal float idleTime;
        internal float currentIdleTime;

        internal Random randomizer;

        public static Texture2D EnemySpriteSheet;
        public static int textureWidthInterval;
        public static int textureHeightInterval;
        public Rectangle textureSourceRectangle;

        private int currentFrameX;
        internal int frameDelay;
        private int currentFrameDelay;

        public Enemy(Rectangle Rect)
        {
            this.Rect = Rect;

            currentHealth = Health;
            damaged = false;

            movementSpeed = 2;
            currentSpeedX = 0;
            currentSpeedY = 0;
            randomizer = new Random(this.GetHashCode());
            movementDirection = (Direction)(randomizer.Next(0, 4));
            knockbackSpeed = 5;

            movementTime = 0.4f;
            currentMovementTime = movementTime;

            idleTime = 1.5f;
            currentIdleTime = randomizer.Next(0, (int)(Math.Ceiling(idleTime))) + (float)(randomizer.NextDouble());

            frameDelay = 3;
            currentFrameDelay = 0;
            currentFrameX = 0;
        }

        public void EnemyActions(Player player, GameTime gameTime)
        {
            if (!damaged)
            {
                EnemyAttack(player, gameTime);
                EnemyMovement(gameTime);
            }
            else
            {
                EnemyKnockback(player, gameTime);
            }

            Animate();
        }

        internal void Animate()
        {
            if (damaged)
            {
                currentFrameX = 0;
            }
            else
            {
                if (currentSpeedX != 0 || currentSpeedY != 0)
                {
                    currentFrameDelay++;
                    if (currentFrameDelay >= frameDelay)
                    {
                        currentFrameDelay = 0;
                        currentFrameX++;
                        if (currentFrameX > 2)
                            currentFrameX = 0;
                    }
                }
                else
                {
                    currentFrameX = 1;
                }
            }

            textureSourceRectangle = new Rectangle(textureWidthInterval * currentFrameX, textureHeightInterval * (int)movementDirection, textureWidthInterval, textureHeightInterval);
        }

        public void Damage(Player player)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                EnemiesToBeRemoved.Add(this);
                return;
            }

            damaged = true;
            currentIdleTime = 0;

            switch (player.lastDirection)
            {
                case Direction.Left:
                    movementDirection = Direction.Right;
                    break;
                case Direction.Right:
                    movementDirection = Direction.Left;
                    break;
                case Direction.Up:
                    movementDirection = Direction.Down;
                    break;
                case Direction.Down:
                    movementDirection = Direction.Up;
                    break;
            }
        }

        public static void RemoveDeadEnemies()
        {
            if (EnemiesToBeRemoved.Count != 0)
            {
                foreach (Enemy enemyToBeRemoved in EnemiesToBeRemoved)
                {
                    Enemies.Remove(enemyToBeRemoved);
                }
                EnemiesToBeRemoved.Clear();
            }
        }

        internal void EnemyKnockback(Player player, GameTime gameTime)
        {
            if (damaged)
            {
                int KnockbackSpeedX = 0;
                int KnockbackSpeedY = 0;

                switch (player.lastDirection)
                {
                    case Direction.Left:
                        KnockbackSpeedX = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        break;
                    case Direction.Right:
                        KnockbackSpeedX = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        break;
                    case Direction.Up:
                        KnockbackSpeedY = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        break;
                    case Direction.Down:
                        KnockbackSpeedY = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        break;
                }

                Physics.WallDetection(ref Rect, ref KnockbackSpeedX, ref KnockbackSpeedY);
                Physics.EnemyBlocking(ref Rect, ref KnockbackSpeedX, ref KnockbackSpeedY);
                Physics.EnemyExitBlocking(ref Rect, ref KnockbackSpeedX, ref KnockbackSpeedY);

                Rect.X += KnockbackSpeedX;
                Rect.Y += KnockbackSpeedY;
            }
        }

        internal void EnemyMovement(GameTime gameTime)
        {
            if (currentIdleTime == idleTime)
            {
                currentSpeedX = 0;
                currentSpeedY = 0;

                currentMovementTime -= gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (currentMovementTime <= 0)
                {
                    currentMovementTime = movementTime;
                    currentIdleTime = 0;
                }

                switch (movementDirection)
                {
                    case Direction.Left:
                        currentSpeedX = -((int)(float)(movementSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        goto default;
                    case Direction.Right:
                        currentSpeedX = (int)(float)(movementSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        goto default;
                    case Direction.Up:
                        currentSpeedY = -((int)(float)(movementSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        goto default;
                    case Direction.Down:
                        currentSpeedY = (int)(float)(movementSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        goto default;
                    default:
                        break;
                }

                Physics.WallDetection(ref Rect, ref currentSpeedX, ref currentSpeedY);
                Physics.EnemyBlocking(ref Rect, ref currentSpeedX, ref currentSpeedY);
                Physics.EnemyExitBlocking(ref Rect, ref currentSpeedX, ref currentSpeedY);

                Rect.X += currentSpeedX;
                Rect.Y += currentSpeedY;
            }
            else
            {
                currentSpeedX = 0;
                currentSpeedY = 0;

                currentIdleTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (currentIdleTime >= idleTime)
                {
                    currentIdleTime = idleTime;
                    movementDirection = (Direction)(randomizer.Next(0, 4));
                    currentMovementTime = movementTime;
                }
            }
        }

        internal void EnemyAttack(Player player, GameTime gameTime)
        {
            if (player.Rect.Intersects(Rect))
            {
                player.Damage(this, gameTime);
                return;
            }
        }
    }
}
