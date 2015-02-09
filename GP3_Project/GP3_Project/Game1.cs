using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GP3_Project
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        //Camera stuff
        Camera2D camera;

        KeyboardState previousKeyState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice.Viewport);
            previousKeyState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun01.txt"), @"Levels\dun01_WholeImage"));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun02.txt"), @"Levels\dun02_WholeImage"));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun03.txt"), @"Levels\dun03_WholeImage"));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun04.txt"), @"Levels\dun04_WholeImage"));

            for (int currentLevel = 0; currentLevel < Level.Levels.Count; currentLevel++)
            {
                if (currentLevel + 1 < Level.Levels.Count)
                    Level.Levels[currentLevel].NextLevel = Level.Levels[currentLevel + 1];
            }

            player = new Player(new Rectangle(0, 0, Tile.TileSize, Tile.TileSize), graphics);
            LevelLoader.LoadLevel(graphics, Content, Level.Levels[0], player);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            //Player updates
            player.InputListener(currentKeyState, previousKeyState, gameTime);
            player.Actions(gameTime);

            if (player.currentHealth <= 0)
            {
                LevelLoader.LoadLevel(graphics, Content, LevelLoader.LoadedLevel, player);
                player.Reset();
            }

            camera.Update(gameTime, player);
            
            //Enemy updates
            foreach(Enemy enemy in Enemy.Enemies)
            {
                if (enemy is EnemyWizard)
                    ((EnemyWizard)enemy).EnemyActions(graphics.GraphicsDevice, player, gameTime);
                else
                    enemy.EnemyActions(player, gameTime);
            }

            Enemy.RemoveDeadEnemies();

            //Projectile updates
            Projectile.UpdateProjectiles(player, gameTime);

            //Next level checking
            foreach (Tile tile in Tile.LevelTiles)
            {
                if (tile is NextLevelTile)
                {
                    ((NextLevelTile)(tile)).CheckExit(player);
                    break;
                }
            }

            previousKeyState = currentKeyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            if (LevelLoader.LoadedLevelTexture != null)
                spriteBatch.Draw(LevelLoader.LoadedLevelTexture, LevelLoader.LoadedLevelTexture.Bounds, Color.White);

            foreach (Enemy enemy in Enemy.Enemies)
            {
                spriteBatch.Draw(enemy.enemyTexture, enemy.Rect, Color.White);
            }

            foreach (Projectile projectile in Projectile.Projectiles)
            {
                spriteBatch.Draw(projectile.texture, projectile.Rect, Color.White);
            }

            spriteBatch.Draw(player.playerTexture, player.Rect, Color.White);
            if (player.IsAttacking)
                spriteBatch.Draw(player.playerTexture, player.AttackRect, Color.Gray);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
