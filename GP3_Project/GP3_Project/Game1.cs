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
    enum GameState
    {
        MainMenu,
        Game,
        Pause
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        GameState currentGameState;

        //Camera stuff
        Camera2D camera;

        KeyboardState previousKeyState;

        Texture2D testTexture;
        Texture2D titleTexture;
        List<MenuItem> mainMenuItems;
        List<MenuItem> pauseMenuItems;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice.Viewport);
            previousKeyState = Keyboard.GetState();

            currentGameState = GameState.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Main menu initialization
            titleTexture = Content.Load<Texture2D>(@"MainMenu\Title_GUI");

            mainMenuItems = new List<MenuItem>();
            mainMenuItems.Add(new MenuItem(Content.Load<Texture2D>(@"MainMenu\start_normal"), Content.Load<Texture2D>(@"MainMenu\start_active"), MainMenuStartSelected));
            mainMenuItems.Add(new MenuItem(Content.Load<Texture2D>(@"MainMenu\exit_normal"), Content.Load<Texture2D>(@"MainMenu\exit_active"), MainMenuExitSelected));
            mainMenuItems[0].isSelected = true;

            pauseMenuItems = new List<MenuItem>();
            pauseMenuItems.Add(new MenuItem(Content.Load<Texture2D>(@"MainMenu\resume_normal"), Content.Load<Texture2D>(@"MainMenu\resume_active"), PauseMenuResumeSelected));
            pauseMenuItems.Add(new MenuItem(Content.Load<Texture2D>(@"MainMenu\backTotitle_normal"), Content.Load<Texture2D>(@"MainMenu\backTotitle_active"), PauseMenuExitSelected));
            pauseMenuItems[0].isSelected = true;

            testTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Color[] testColor = new Color[1];
            testColor[0] = Color.Green;
            testTexture.SetData(testColor);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    if (currentKeyState != previousKeyState)
                    {
                        if (currentKeyState.IsKeyDown(Keys.Down))
                        {
                            for (int currentItem = 0; currentItem < mainMenuItems.Count; currentItem++)
                            {
                                if (mainMenuItems[currentItem].isSelected)
                                {
                                    mainMenuItems[currentItem].isSelected = false;
                                    if (currentItem + 1 >= mainMenuItems.Count)
                                    {
                                        currentItem = 0;
                                        mainMenuItems[currentItem].isSelected = true;
                                        break;
                                    }
                                    else
                                    {
                                        mainMenuItems[currentItem + 1].isSelected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (currentKeyState.IsKeyDown(Keys.Up))
                        {
                            for (int currentItem = 0; currentItem < mainMenuItems.Count; currentItem++)
                            {
                                if (mainMenuItems[currentItem].isSelected)
                                {
                                    mainMenuItems[currentItem].isSelected = false;
                                    if (currentItem - 1 < 0)
                                    {
                                        currentItem = mainMenuItems.Count - 1;
                                        mainMenuItems[currentItem].isSelected = true;
                                        break;
                                    }
                                    else
                                    {
                                        mainMenuItems[currentItem - 1].isSelected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (currentKeyState.IsKeyDown(Keys.Z))
                        {
                            foreach (MenuItem item in mainMenuItems)
                            {
                                if (item.isSelected)
                                {
                                    item.onSelectEvent();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case GameState.Game:
                    //Pause checking
                    if (currentKeyState != previousKeyState)
                    {
                        if (currentKeyState.IsKeyDown(Keys.Escape))
                        {
                            currentGameState = GameState.Pause;
                            break;
                        }
                    }

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
                    foreach (Enemy enemy in Enemy.Enemies)
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
                            ((NextLevelTile)(tile)).CheckExit(player, ref currentGameState);
                            break;
                        }
                    }
                    break;

                case GameState.Pause:
                    if (currentKeyState != previousKeyState)
                    {
                        if (currentKeyState.IsKeyDown(Keys.Down))
                        {
                            for (int currentItem = 0; currentItem < pauseMenuItems.Count; currentItem++)
                            {
                                if (pauseMenuItems[currentItem].isSelected)
                                {
                                    pauseMenuItems[currentItem].isSelected = false;
                                    if (currentItem + 1 >= pauseMenuItems.Count)
                                    {
                                        currentItem = 0;
                                        pauseMenuItems[currentItem].isSelected = true;
                                        break;
                                    }
                                    else
                                    {
                                        pauseMenuItems[currentItem + 1].isSelected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (currentKeyState.IsKeyDown(Keys.Up))
                        {
                            for (int currentItem = 0; currentItem < pauseMenuItems.Count; currentItem++)
                            {
                                if (pauseMenuItems[currentItem].isSelected)
                                {
                                    pauseMenuItems[currentItem].isSelected = false;
                                    if (currentItem - 1 < 0)
                                    {
                                        currentItem = pauseMenuItems.Count - 1;
                                        pauseMenuItems[currentItem].isSelected = true;
                                        break;
                                    }
                                    else
                                    {
                                        pauseMenuItems[currentItem - 1].isSelected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (currentKeyState.IsKeyDown(Keys.Z))
                        {
                            foreach (MenuItem item in pauseMenuItems)
                            {
                                if (item.isSelected)
                                {
                                    item.onSelectEvent();
                                    break;
                                }
                            }
                        }
                        else if (currentKeyState.IsKeyDown(Keys.Escape))
                        {
                            PauseMenuResumeSelected();
                        }
                    }
                    break;
            }
            previousKeyState = currentKeyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(
                        titleTexture, 
                        new Rectangle((graphics.PreferredBackBufferWidth - titleTexture.Bounds.Width) / 2, 0, titleTexture.Bounds.Width, titleTexture.Bounds.Height), 
                        Color.White);
                    int currentY = graphics.PreferredBackBufferHeight / 2 + 100;
                    foreach (MenuItem item in mainMenuItems)
                    {
                        if (item.isSelected)
                            spriteBatch.Draw(
                                item.selectedState, 
                                new Rectangle((graphics.PreferredBackBufferWidth - item.selectedState.Bounds.Width / 2) / 2, currentY, item.selectedState.Bounds.Width / 2, item.selectedState.Bounds.Height / 2), 
                                Color.White);
                        else
                            spriteBatch.Draw(
                                item.normalState,
                                new Rectangle((graphics.PreferredBackBufferWidth - item.selectedState.Bounds.Width / 2) / 2, currentY, item.selectedState.Bounds.Width / 2, item.selectedState.Bounds.Height / 2),
                                Color.White);
                        currentY += item.selectedState.Bounds.Height - 50;
                    }

                    spriteBatch.End();
                    break;

                case GameState.Game:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                    if (LevelLoader.LoadedLevelTexture != null)
                        spriteBatch.Draw(LevelLoader.LoadedLevelTexture, LevelLoader.LoadedLevelTexture.Bounds, Color.White);

                    foreach (Enemy enemy in Enemy.Enemies)
                    {
                        if (enemy is EnemyWizard)
                            spriteBatch.Draw(EnemyWizard.EnemyWizardSpriteSheet, enemy.Rect, enemy.textureSourceRectangle, Color.White);
                        else
                            spriteBatch.Draw(Enemy.EnemySpriteSheet, enemy.Rect, enemy.textureSourceRectangle, Color.White);
                    }

                    foreach (Projectile projectile in Projectile.Projectiles)
                    {
                        spriteBatch.Draw(Projectile.ProjectileSpriteSheet, projectile.Rect, projectile.textureSourceRectangle, Color.White);
                    }

                    if (player.IsAttacking)
                    {
                        switch (player.lastDirection)
                        {
                            case Direction.Left:
                                spriteBatch.Draw(
                                    Player.PlayerSwordTexture,
                                    new Rectangle(player.Rect.Left - Tile.TileSize / 8, player.Rect.Center.Y - Tile.TileSize, player.AttackRect.Height, player.AttackRect.Width), 
                                    null, 
                                    Color.White, 
                                    (float)(Math.PI * 3 / 2), 
                                    new Vector2(Tile.TileSize / 4 * 3, Tile.TileSize * 3 / 2), 
                                    SpriteEffects.None, 
                                    0);
                                break;
                            case Direction.Right:
                                spriteBatch.Draw(
                                    Player.PlayerSwordTexture,
                                    new Rectangle(player.Rect.Right - Tile.TileSize / 4, player.Rect.Center.Y - Tile.TileSize, player.AttackRect.Height, player.AttackRect.Width),
                                    null,
                                    Color.White,
                                    (float)(Math.PI / 2),
                                    new Vector2(-Tile.TileSize * 9 / 21, Tile.TileSize * 15 / 7),
                                    SpriteEffects.None,
                                    0);
                                break;
                            case Direction.Up:
                                spriteBatch.Draw(Player.PlayerSwordTexture, player.AttackRect, Color.White);
                                break;
                            case Direction.Down:
                                spriteBatch.Draw(Player.PlayerSwordTexture, player.AttackRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                                break;
                        }
                
                    }

                    spriteBatch.Draw(Player.PlayerSpriteSheet, player.Rect, player.textureSourceRectangle, Color.White);

                    spriteBatch.End();

                    spriteBatch.Begin();
                    for (int health = 0; health < player.currentHealth; health++)
                    {
                        spriteBatch.Draw(
                            Player.HeartTexture, 
                            new Rectangle(health * Player.HeartTexture.Bounds.Width / 5, 0, Player.HeartTexture.Bounds.Width / 5, Player.HeartTexture.Bounds.Height / 5), 
                            Color.White);
                    }
                    spriteBatch.End();
                    break;

                case GameState.Pause:
                    spriteBatch.Begin();

                    int currentPauseY = graphics.PreferredBackBufferHeight / 2;
                    foreach (MenuItem item in pauseMenuItems)
                    {
                        if (item.isSelected)
                            spriteBatch.Draw(
                                item.selectedState,
                                new Rectangle((graphics.PreferredBackBufferWidth - item.selectedState.Bounds.Width / 2) / 2, currentPauseY, item.selectedState.Bounds.Width / 2, item.selectedState.Bounds.Height / 2),
                                Color.White);
                        else
                            spriteBatch.Draw(
                                item.normalState,
                                new Rectangle((graphics.PreferredBackBufferWidth - item.selectedState.Bounds.Width / 2) / 2, currentPauseY, item.selectedState.Bounds.Width / 2, item.selectedState.Bounds.Height / 2),
                                Color.White);

                        currentPauseY += item.selectedState.Bounds.Height - 50;
                    }
                    spriteBatch.End();
                    break;

            }

            base.Draw(gameTime);
        }

        private void MainMenuStartSelected()
        {
            currentGameState = GameState.Game;

            //Game initialization
            Level.Levels = new List<Level>();
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun01.txt"), @"Levels\dun01_WholeImage"));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun02.txt"), @"Levels\dun02_WholeImage"));
            Level.Levels.Add(new Level(File.ReadAllLines(Content.RootDirectory + @"\Levels\Dun03.txt"), @"Levels\dun03_WholeImage"));

            for (int currentLevel = 0; currentLevel < Level.Levels.Count; currentLevel++)
            {
                if (currentLevel + 1 < Level.Levels.Count)
                    Level.Levels[currentLevel].NextLevel = Level.Levels[currentLevel + 1];
            }

            player = new Player(new Rectangle(0, 0, Tile.TileSize, Tile.TileSize));

            //Load textures
            Player.PlayerSpriteSheet = Content.Load<Texture2D>(@"Textures\Character_SpriteSheet");
            player.textureWidthInterval = Player.PlayerSpriteSheet.Width / 3;
            player.textureHeightInterval = Player.PlayerSpriteSheet.Height / 4;

            Player.PlayerSwordTexture = Content.Load<Texture2D>(@"Textures\Sword");

            Player.HeartTexture = Content.Load<Texture2D>(@"GameUI\Life");

            Enemy.EnemySpriteSheet = Content.Load<Texture2D>(@"Textures\Enemy_Sprite");
            EnemyWizard.EnemyWizardSpriteSheet = Content.Load<Texture2D>(@"Textures\Boss_Sprite");
            Enemy.textureWidthInterval = Enemy.EnemySpriteSheet.Width / 3;
            Enemy.textureHeightInterval = Enemy.EnemySpriteSheet.Height / 4;

            Projectile.ProjectileSpriteSheet = Content.Load<Texture2D>(@"Textures\FireBall");
            Projectile.textureWidthInterval = Projectile.ProjectileSpriteSheet.Width / 3;
            Projectile.textureHeightInterval = Projectile.ProjectileSpriteSheet.Height / 4;

            //Load first level
            LevelLoader.LoadLevel(graphics, Content, Level.Levels[0], player);
        }

        private void MainMenuExitSelected()
        {
            Exit();
        }

        private void PauseMenuResumeSelected()
        {
            currentGameState = GameState.Game;
        }

        private void PauseMenuExitSelected()
        {
            currentGameState = GameState.MainMenu;
        }
    }
}
