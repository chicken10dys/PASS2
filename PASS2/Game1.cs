//Author: ...
//File Name: Game1.cs
//Project Name: PASS2
//Creation Date: November 24, 2022
//Modified Date: November 27, 2022
//Description: Throw balls into bucket

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace PASS2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        //Get mouse and keyboard state
        MouseState mouse;
        KeyboardState kb;
        KeyboardState prevKb;
        
        //Store window size
        int screenWidth;
        int screenHeight;

        //Store the gamestate and make the default 0
        int gamestate = 0;
        
        //Assign each gamestate a number for the gamestate int
        const int MENU = 0;
        const int SCORES = 1;
        const int GAME = 2;
        const int PAUSE = 3;
        const int END = 4;
        
        //Declare textures
        Texture2D bgImg;
        Texture2D menuBg; 
        Texture2D easyBtnImg;
        Texture2D scoresBtnImg;
        Texture2D exitBtnImg;
        Texture2D menuBtnImg;
        Texture2D resumeBtnImg;
        Texture2D blankImg;
        
        
        //Declare rectangles
        Rectangle bgRec;
        Rectangle easyBtnRec;
        Rectangle scoresBtnRec;
        Rectangle exitBtnRec;
        Rectangle resumeBtnRec;
        Rectangle menuBtnRec;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            //Make mouse visible
            IsMouseVisible = true;
            
            //Get the window size
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            
            //Load in sprites
            easyBtnImg = Content.Load<Texture2D>("Sprites/BtnEasy");
            scoresBtnImg = Content.Load<Texture2D>("Sprites/BtnHighscores");
            exitBtnImg = Content.Load<Texture2D>("Sprites/BtnExit");
            resumeBtnImg = Content.Load<Texture2D>("Sprites/BtnResume");
            menuBtnImg = Content.Load<Texture2D>("Sprites/BtnMenu");
            
            //Load in the background
            bgImg = Content.Load<Texture2D>("Backgrounds/CircusBG");
            
            //Set rectangle positions
            bgRec = new Rectangle(0, 0, screenWidth, screenHeight);
            easyBtnRec = new Rectangle(((screenWidth / 2) - (easyBtnImg.Width / 2)), 100, easyBtnImg.Width, easyBtnImg.Height);
            scoresBtnRec = new Rectangle(((screenWidth / 2) - (scoresBtnImg.Width / 2)), 175, scoresBtnImg.Width, scoresBtnImg.Height);
            exitBtnRec = new Rectangle(((screenWidth / 2) - (exitBtnImg.Width / 2)), 250, exitBtnImg.Width, exitBtnImg.Height);
            menuBtnRec =  new Rectangle(0, 0, menuBtnImg.Width, menuBtnImg.Height);
            resumeBtnRec = new Rectangle(((screenWidth / 2) - (resumeBtnImg.Width / 2)), ((screenHeight / 2) - (resumeBtnImg.Height / 2)), resumeBtnImg.Width, resumeBtnImg.Height);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            // TODO: Add your update logic here

            //Update your keyboard and mouse position
            mouse = Mouse.GetState();
            prevKb = kb;
            kb = Keyboard.GetState();
            
            switch (gamestate)
            {
                
                case MENU:
                    //Check if user is pressing the exit button and close the game
                    if(exitBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        Exit();
                    
                    //Check if user is pressing the easy button and start the game in easy mode (by switching the gamestate)
                    if(easyBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        gamestate = GAME;
                    
                    //Check if user is pressing the score button and open the high score page (by switching the gamestate)
                    if(scoresBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        gamestate = SCORES;
                    break;
                
                case GAME:
                    //Check if user is pressing the pause button
                    if(kb.IsKeyDown(Keys.Escape) && kb != prevKb)
                        gamestate = PAUSE;
                    break;
                
                case PAUSE:
                    //Check if user is pressing the pause button
                    if(kb.IsKeyDown(Keys.Escape) && kb != prevKb)
                        gamestate = GAME;
                    //Check if user is pressing the resume button
                    if(resumeBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        gamestate = GAME;
                    break;
                
                case SCORES:
                    //Check if user is pressing the menu button
                    if(menuBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        gamestate = MENU;
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //Draw required objects based on the gamestate
            spriteBatch.Begin();
            
            if (gamestate == 2)
                spriteBatch.Draw(bgImg, bgRec, Color.White);
            
            switch (gamestate)
            {
                case MENU:
                    spriteBatch.Draw(easyBtnImg, easyBtnRec, Color.White);
                    spriteBatch.Draw(scoresBtnImg, scoresBtnRec, Color.White);
                    spriteBatch.Draw(exitBtnImg, exitBtnRec, Color.White);
                    break;
                
                case GAME:
                    spriteBatch.Draw(bgImg, bgRec, Color.White);
                    break;
                
                case SCORES:
                    spriteBatch.Draw(menuBtnImg, menuBtnRec, Color.White);
                    break;
                
                case PAUSE:
                    spriteBatch.Draw(resumeBtnImg, resumeBtnRec, Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
