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

            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun01_Start.txt"), Content.Load<Texture2D>(@"Textures\dun01_WholeImage")));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun02_From01.txt"), Content.Load<Texture2D>(@"Textures\dun02_WholeImage")));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun01_From02.txt"), Content.Load<Texture2D>(@"Textures\dun01_WholeImage")));

            Level.Levels[0].NextLevels.Add(Level.Levels[1]);

            Level.Levels[1].NextLevels.Add(Level.Levels[2]);

            Level.Levels[2].NextLevels.Add(Level.Levels[1]);

            player = new Player(new Rectangle(0, 0, Tile.TileSize, Tile.TileSize), graphics);
            LevelLoader.LoadLevel(graphics, Content, Level.Levels[0], player);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState currentKeyState = Keyboard.GetState();

            player.InputListener(currentKeyState, previousKeyState, gameTime);
            player.Actions(gameTime);

            camera.Update(gameTime, player);
            
            foreach(Enemy enemy in Enemy.Enemies)
            {
                enemy.EnemyKnockback(player, gameTime);
                enemy.EnemyMovement(gameTime);
            }

            foreach (Tile tile in Tile.LevelTiles)
            {
                if (tile.GetType() == typeof(NextLevelTile))
                {
                    bool shouldBreak = false;
                    ((NextLevelTile)tile).CheckExit(player, ref shouldBreak);
                    if (shouldBreak)
                        break;
                }
            }

            Enemy.RemoveDeadEnemies();

            previousKeyState = currentKeyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            spriteBatch.Draw(LevelLoader.LoadedLevelTexture, LevelLoader.LoadedLevelTexture.Bounds, Color.White);

            foreach (Enemy enemy in Enemy.Enemies)
            {
                spriteBatch.Draw(enemy.enemyTexture, enemy.Rect, Color.White);
            }

            spriteBatch.Draw(player.playerTexture, player.Rect, Color.White);
            if (player.IsAttacking)
                spriteBatch.Draw(player.playerTexture, player.AttackRect, Color.Gray);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
