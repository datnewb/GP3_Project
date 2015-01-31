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

        public int knockbackSpeed;

        public Enemy(GraphicsDevice graphicsDevice, Rectangle Rect)
        {
            this.Rect = Rect;

            Color[] textureColor = new Color[1];
            textureColor[0] = Color.Red;
            enemyTexture = new Texture2D(graphicsDevice, 1, 1);
            enemyTexture.SetData(textureColor);

            currentHealth = Health;
            damaged = false;

            knockbackSpeed = 5;
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

        public void EnemyKnockback(Player player, GameTime gameTime)
        {
            if (damaged)
            {
                int KnockbackSpeedX = 0;
                int KnockbackSpeedY = 0;

                switch (player.lastDirection)
                {
                    case LastDirection.Left:
                        KnockbackSpeedX = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        break;
                    case LastDirection.Right:
                        KnockbackSpeedX = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        break;
                    case LastDirection.Up:
                        KnockbackSpeedY = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        break;
                    case LastDirection.Down:
                        KnockbackSpeedY = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        break;
                }

                Physics.WallDetection(ref Rect, ref KnockbackSpeedX, ref KnockbackSpeedY);

                Rect.X += KnockbackSpeedX;
                Rect.Y += KnockbackSpeedY;
            }
        }
    }
}
