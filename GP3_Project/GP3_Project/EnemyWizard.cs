using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class EnemyWizard : Enemy
    {
        private int detectionDistance;
        private bool isAttacking;

        private float attackCooldownTime;
        private float currentAttackCooldownTime;

        public EnemyWizard (GraphicsDevice graphicsDevice, Rectangle Rect) 
            : base (graphicsDevice, Rect)
        {
            movementSpeed = 1;
            movementTime = 3;
            currentMovementTime = movementTime;

            idleTime = 3;
            currentIdleTime = randomizer.Next(0, (int)idleTime);

            detectionDistance = 400;
            isAttacking = false;

            attackCooldownTime = 1.5f;
            currentAttackCooldownTime = 0;
        }

        public void EnemyActions(GraphicsDevice graphicsDevice, Player player, GameTime gameTime)
        {
            if (!damaged)
            {
                EnemyAttack(player, gameTime);
                EnemyMovement(gameTime);
                if (!isAttacking)
                {
                    EnemyAttackRanged(graphicsDevice, player);
                }
                else
                {
                    EnemyAttackCooldown(gameTime);
                }
            }
            else
            {
                EnemyKnockback(player, gameTime);
            }
        }

        private void EnemyAttackRanged(GraphicsDevice graphicsDevice, Player player)
        {
            int currentRayDistance = 0;
            int rayDistanceIncrement = 10;
            int rayStartX = 0;
            int rayStartY = 0;
            switch (movementDirection)
            {
                case Direction.Left:
                    rayDistanceIncrement = -rayDistanceIncrement;
                    rayStartX = Rect.Left;
                    rayStartY = Rect.Center.Y;
                    break;
                case Direction.Right:
                    rayStartX = Rect.Right;
                    rayStartY = Rect.Center.Y;
                    break;
                case Direction.Up:
                    rayDistanceIncrement = -rayDistanceIncrement;
                    rayStartX = Rect.Center.X;
                    rayStartY = Rect.Top;
                    break;
                case Direction.Down:
                    rayStartX = Rect.Center.X;
                    rayStartY = Rect.Bottom;
                    break;
            }

            switch (movementDirection)
            {
                case Direction.Left:
                    goto case Direction.Right;
                case Direction.Right:
                    for (; currentRayDistance <= detectionDistance; currentRayDistance += rayDistanceIncrement)
                    {
                        Rectangle detectionRectangle = new Rectangle(rayStartX + currentRayDistance, rayStartY, 1, 1);
                        foreach (Tile tile in Tile.LevelTiles)
                        {
                            if (detectionRectangle.Intersects(tile.Rect))
                            {
                                return;
                            }
                        }
                        if (detectionRectangle.Intersects(player.Rect))
                        {
                            //ATTACK
                            if (movementDirection == Direction.Left)
                            {
                                Projectile.Projectiles.Add(new Projectile(
                                    graphicsDevice,
                                    new Rectangle(Rect.Left - Tile.TileSize / 2, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize / 2, Tile.TileSize / 2),
                                    movementDirection,
                                    this));
                            }
                            else if (movementDirection == Direction.Right)
                            {
                                Projectile.Projectiles.Add(new Projectile(
                                    graphicsDevice,
                                    new Rectangle(Rect.Right, Rect.Center.Y - Tile.TileSize / 4, Tile.TileSize / 2, Tile.TileSize / 2),
                                    movementDirection,
                                    this));
                            }
                            isAttacking = true;
                            return;
                        }
                    }
                    break;

                case Direction.Up:
                    goto case Direction.Down;
                case Direction.Down:
                    for (; currentRayDistance <= detectionDistance; currentRayDistance += rayDistanceIncrement)
                    {
                        Rectangle detectionRectangle = new Rectangle(rayStartX, rayStartY + currentRayDistance, 1, 1);
                        foreach (Tile tile in Tile.LevelTiles)
                        {
                            if (detectionRectangle.Intersects(tile.Rect))
                            {
                                return;
                            }
                        }
                        if (detectionRectangle.Intersects(player.Rect))
                        {
                            //ATTACK
                            if (movementDirection == Direction.Up)
                            {
                                Projectile.Projectiles.Add(new Projectile(
                                    graphicsDevice,
                                    new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Top - Tile.TileSize / 2, Tile.TileSize / 2, Tile.TileSize / 2),
                                    movementDirection,
                                    this));
                            }
                            if (movementDirection == Direction.Down)
                            {
                                Projectile.Projectiles.Add(new Projectile(
                                    graphicsDevice,
                                    new Rectangle(Rect.Center.X - Tile.TileSize / 4, Rect.Bottom, Tile.TileSize / 2, Tile.TileSize / 2),
                                    movementDirection,
                                    this));
                            }
                            isAttacking = true;
                            return;
                        }
                    }
                    break;
            }
        }

        private void EnemyAttackCooldown(GameTime gameTime)
        {
            currentAttackCooldownTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (currentAttackCooldownTime >= attackCooldownTime)
            {
                isAttacking = false;
                currentAttackCooldownTime = 0;
            }
        }

        internal void Damage(Player player)
        {
            Damage();

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
    }
}
