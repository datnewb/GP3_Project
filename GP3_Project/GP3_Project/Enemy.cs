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
        public Texture2D enemyTexture;
        public int Health = 3;
        public int currentHealth;
        public bool damaged;

        public int movementSpeed;
        public Direction movementDirection;
        public int knockbackSpeed;

        private float movementTime;
        private float currentMovementTime;

        private float idleTime;
        private float currentIdleTime;

        private Random randomizer;

        public Enemy(GraphicsDevice graphicsDevice, Rectangle Rect)
        {
            this.Rect = Rect;

            Color[] textureColor = new Color[1];
            textureColor[0] = Color.Red;
            enemyTexture = new Texture2D(graphicsDevice, 1, 1);
            enemyTexture.SetData(textureColor);

            currentHealth = Health;
            damaged = false;

            movementSpeed = 2;
            randomizer = new Random(this.GetHashCode());
            movementDirection = (Direction)(randomizer.Next(0, 4));
            knockbackSpeed = 5;

            movementTime = 0.1f;
            currentMovementTime = movementTime;

            idleTime = 1;
            currentIdleTime = idleTime;
        }

        public void EnemyActions(Player player, GameTime gameTime)
        {
            EnemyAttack(player, gameTime);
            EnemyKnockback(player, gameTime);
            EnemyMovement(gameTime);
        }

        public void Damage()
        {
            currentHealth--;
            damaged = true;
            if (currentHealth <= 0)
                EnemiesToBeRemoved.Add(this);
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

        private void EnemyKnockback(Player player, GameTime gameTime)
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

                Rect.X += KnockbackSpeedX;
                Rect.Y += KnockbackSpeedY;
            }
        }

        private void EnemyMovement(GameTime gameTime)
        {
            if (currentIdleTime == idleTime)
            {
                int currentSpeedX = 0;
                int currentSpeedY = 0;

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

                Rect.X += currentSpeedX;
                Rect.Y += currentSpeedY;
            }
            else
            {
                currentIdleTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (currentIdleTime >= idleTime)
                {
                    currentIdleTime = idleTime;
                    movementDirection = (Direction)(randomizer.Next(0, 4));
                    currentMovementTime = movementTime;
                }
            }
        }

        private void EnemyAttack(Player player, GameTime gameTime)
        {
            if (player.Rect.Intersects(Rect))
            {
                player.Damage(this, gameTime);
            }
        }
    }
}
