using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    enum LastDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    class Player
    {
        public Rectangle Rect;
        public Rectangle AttackRect;
        public LastDirection lastDirection;
        public static int Speed = 3;
        private int currentSpeedX;
        private int currentSpeedY;
        public Texture2D playerTexture;

        public bool AllowMove;
        public bool IsAttacking;

        private float AttackTime;
        private float currentAttackTime;

        public Player(Rectangle startPosition, GraphicsDeviceManager graphics)
        {
            Rect = startPosition;
            currentSpeedX = 0;
            currentSpeedY = 0;

            playerTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Color[] textureColor = new Color[1];
            textureColor[0] = Color.DarkGreen;
            playerTexture.SetData(textureColor);

            lastDirection = LastDirection.Down;
            AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 8, Rect.Bottom, Tile.TileSize / 4, Tile.TileSize);

            AllowMove = true;
            IsAttacking = false;

            AttackTime = 0.2f;
            currentAttackTime = 0;
        }

        public void InputListener(KeyboardState currentKeyState, KeyboardState previousKeyState, GameTime gameTime)
        {
            if (AllowMove)
            {
                if (currentKeyState.IsKeyDown(Keys.Left))
                {
                    currentSpeedX = -((int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                    lastDirection = LastDirection.Left;
                    currentSpeedY = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Right))
                {
                    currentSpeedX = (int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                    lastDirection = LastDirection.Right;
                    currentSpeedY = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Up))
                {
                    currentSpeedY = -((int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                    lastDirection = LastDirection.Up;
                    currentSpeedX = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Down))
                {
                    currentSpeedY = (int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                    lastDirection = LastDirection.Down;
                    currentSpeedX = 0;
                }
                else
                {
                    currentSpeedX = 0;
                    currentSpeedY = 0;
                }
            }
            else
            {
                currentSpeedX = 0;
                currentSpeedY = 0;
            }

            //For single-press controls
            if (currentKeyState != previousKeyState)
            {
                if (currentKeyState.IsKeyDown(Keys.Z))
                {
                    IsAttacking = true;
                }
            }
        }

        public void Actions(GameTime gameTime)
        {
            UpdateAttackRect();
            Attack(gameTime);

            Physics.WallDetection(ref Rect, ref currentSpeedX, ref currentSpeedY);

            Rect.X += currentSpeedX;
            Rect.Y += currentSpeedY;
        }

        public void UpdateAttackRect()
        {
            switch (lastDirection)
            {
                case LastDirection.Left:
                    AttackRect = new Rectangle(Rect.Left - Tile.TileSize, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize, Tile.TileSize / 2);
                    break;
                case LastDirection.Right:
                    AttackRect = new Rectangle(Rect.Right, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize, Tile.TileSize / 2);
                    break;
                case LastDirection.Up:
                    AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Top - Tile.TileSize, Tile.TileSize / 2, Tile.TileSize);
                    break;
                case LastDirection.Down:
                    AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Bottom, Tile.TileSize / 2, Tile.TileSize);
                    break;
            }
        }

        public void Attack(GameTime gameTime)
        {
            if (IsAttacking)
            {
                AllowMove = false;
                currentAttackTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (currentAttackTime >= AttackTime)
                {
                    currentAttackTime = 0;
                    AllowMove = true;
                    IsAttacking = false;
                }

                foreach(Enemy enemy in Enemy.Enemies)
                {
                    if (!enemy.damaged)
                    {
                        if (AttackRect.Intersects(enemy.Rect))
                        {
                            enemy.damaged = true;
                            enemy.Damage();
                        }
                    }
                }
            }
            else
            {
                foreach(Enemy enemy in Enemy.Enemies)
                {
                    if (enemy.damaged)
                        enemy.damaged = false;
                }
            }
        }
    }
}
