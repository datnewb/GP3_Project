﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    enum Direction
    {
        Down,
        Left,
        Right,
        Up
    }

    class Player
    {
        public Rectangle Rect;
        public Rectangle AttackRect;
        public Direction lastDirection;
        public static int Speed = 3;
        private int currentSpeedX;
        private int currentSpeedY;

        public bool AllowMove;
        public bool IsAttacking;

        private float AttackTime;
        private float currentAttackTime;

        public int health;
        public int currentHealth;

        public Enemy lastEnemyAttacker;

        private bool damaged;
        private float invulnerableTime;
        private float currentInvulnerableTime;

        private float knockbackSpeed;
        public Direction knockbackDirection;

        public static Texture2D PlayerSpriteSheet;
        public static Texture2D PlayerSwordTexture;
        public static Texture2D HeartTexture;
        public Rectangle textureSourceRectangle;
        public int textureWidthInterval;
        public int textureHeightInterval;
        private int currentFrameX = 0;

        private int frameDelay;
        private int currentFrameDelay;

        public Player(Rectangle startPosition)
        {
            Rect = startPosition;
            currentSpeedX = 0;
            currentSpeedY = 0;

            lastDirection = Direction.Down;
            AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 8, Rect.Bottom, Tile.TileSize / 4, Tile.TileSize);

            AllowMove = true;
            IsAttacking = false;

            AttackTime = 0.2f;
            currentAttackTime = 0;

            health = 3;
            currentHealth = health;

            damaged = false;
            invulnerableTime = 0.25f;
            currentInvulnerableTime = 0;

            knockbackSpeed = 5;
            knockbackDirection = Direction.Left;

            frameDelay = 5;
            currentFrameDelay = 0;
        }

        public void Reset()
        {
            currentHealth = health;

            currentSpeedX = 0;
            currentSpeedY = 0;

            lastDirection = Direction.Down;
            AllowMove = true;

            IsAttacking = false;
            currentAttackTime = 0;

            damaged = false;
            currentInvulnerableTime = 0;
        }

        public void InputListener(KeyboardState currentKeyState, KeyboardState previousKeyState, GameTime gameTime)
        {
            if (AllowMove)
            {
                if (currentKeyState.IsKeyDown(Keys.Left))
                {
                    currentSpeedX = -((int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                    lastDirection = Direction.Left;
                    currentSpeedY = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Right))
                {
                    currentSpeedX = (int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                    lastDirection = Direction.Right;
                    currentSpeedY = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Up))
                {
                    currentSpeedY = -((int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                    lastDirection = Direction.Up;
                    currentSpeedX = 0;
                }
                else if (currentKeyState.IsKeyDown(Keys.Down))
                {
                    currentSpeedY = (int)(float)(Speed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                    lastDirection = Direction.Down;
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
                if (!damaged)
                {
                    if (currentKeyState.IsKeyDown(Keys.Z))
                    {
                        IsAttacking = true;
                    }
                }
            }
        }

        public void Actions(GameTime gameTime)
        {
            if (!damaged)
            {
                UpdateAttackRect();
                Attack(gameTime);

                Physics.WallDetection(ref Rect, ref currentSpeedX, ref currentSpeedY);

                Rect.X += currentSpeedX;
                Rect.Y += currentSpeedY;
            }
            else
            {
                currentInvulnerableTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (currentInvulnerableTime >= invulnerableTime)
                {
                    damaged = false;
                    lastEnemyAttacker = null;
                }
                Knockback(gameTime);
            }

            Animate();
        }

        private void Animate()
        {
            if (IsAttacking)
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

            textureSourceRectangle = new Rectangle(textureWidthInterval * currentFrameX, textureHeightInterval * (int)lastDirection, textureWidthInterval, textureHeightInterval);
        }

        public void Damage(Enemy enemy, GameTime gameTime)
        {
            if (!damaged)
            {
                lastEnemyAttacker = enemy;
                IsAttacking = false;
                currentInvulnerableTime = 0;
                currentHealth--;
                damaged = true;

                if (currentSpeedX == 0 && currentSpeedY == 0)
                {
                    knockbackDirection = enemy.movementDirection;
                }

                else
                {
                    switch (lastDirection)
                    {
                        case Direction.Left:
                            knockbackDirection = Direction.Right;
                            break;
                        case Direction.Right:
                            knockbackDirection = Direction.Left;
                            break;
                        case Direction.Up:
                            knockbackDirection = Direction.Down;
                            break;
                        case Direction.Down:
                            knockbackDirection = Direction.Up;
                            break;
                    }
                }
            }
        }

        private void Knockback(GameTime gameTime)
        {
            if (lastEnemyAttacker != null)
            {
                int knockbackSpeedX = 0;
                int knockbackSpeedY = 0;

                switch (knockbackDirection)
                {
                    case Direction.Left:
                        knockbackSpeedX = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        lastDirection = Direction.Right;
                        break;
                    case Direction.Right:
                        knockbackSpeedX = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        lastDirection = Direction.Left;
                        break;
                    case Direction.Up:
                        knockbackSpeedY  = -((int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f)));
                        lastDirection = Direction.Down;
                        break;
                    case Direction.Down:
                        knockbackSpeedY = (int)(float)(knockbackSpeed * ((float)gameTime.ElapsedGameTime.Milliseconds / 10f));
                        lastDirection = Direction.Up;
                        break;
                }

                Physics.WallDetection(ref Rect, ref knockbackSpeedX, ref knockbackSpeedY);

                Rect.X += knockbackSpeedX;
                Rect.Y += knockbackSpeedY;
            }
        }

        private void UpdateAttackRect()
        {
            switch (lastDirection)
            {
                case Direction.Left:
                    AttackRect = new Rectangle(Rect.Left - Tile.TileSize, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize, Tile.TileSize / 2);
                    break;
                case Direction.Right:
                    AttackRect = new Rectangle(Rect.Right, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize, Tile.TileSize / 2);
                    break;
                case Direction.Up:
                    AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Top - Tile.TileSize, Tile.TileSize / 2, Tile.TileSize);
                    break;
                case Direction.Down:
                    AttackRect = new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Bottom, Tile.TileSize / 2, Tile.TileSize);
                    break;
            }
        }

        private void Attack(GameTime gameTime)
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
                            enemy.Damage(this);
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
