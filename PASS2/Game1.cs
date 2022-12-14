//Author: ...
//File Name: Game1.cs
//Project Name: PASS2
//Creation Date: November 24, 2022
//Modified Date: December 4, 2022
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
        MouseState prevMouse;
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
     
        //Gravity const
        const double GRAVITY = 9.81/60;
        
        //Store angle
        int angle = 10;
        
        //Store the speed
        float speed = 5f;
        
        //Store how filled the meters should be
        int angleMeterFill = 0;
        int speedMeterFill = 0;

        //Declare textures
        //Ui
        
        //Store backgrounds (numbers tied to gamestates)
        Texture2D [] bgImg = new Texture2D [5];
        
        Texture2D easyBtnImg;
        Texture2D scoresBtnImg;
        Texture2D exitBtnImg;
        Texture2D menuBtnImg;
        Texture2D resumeBtnImg;
        Texture2D blankImg;
        Texture2D meterImg;
        Texture2D powerArrowImg;
        
        //Game assets
        Texture2D ballImg;
        
        Texture2D [] bucketImg = new Texture2D [3];
        
        Texture2D splashImg;
        
        Animation[] splashAnim = new Animation [3];

            //Declare rectangles
        //Ui
        Rectangle bgRec;
        Rectangle easyBtnRec;
        Rectangle scoresBtnRec;
        Rectangle exitBtnRec;
        Rectangle resumeBtnRec;
        
        //Store menu button location bassed on gamestate
        Rectangle [] menuBtnRec = new Rectangle [5];
        
        Rectangle angleMeterRec;
        Rectangle angleMeterFillRec;
        Rectangle speedMeterRec;
        Rectangle speedMeterFillRec;
        Rectangle speedArrowRec;
        Rectangle HUDRec;

        //Game assets
        Rectangle ballRec;
        
        //Splash
        Vector2[] splashPos = new Vector2 [3];
        
        //Rectangles related to buckets blue = 0, green = 1, red = 2
        Rectangle [] bucketRec = new Rectangle [3];
        Rectangle [] bucketInRec = new Rectangle [3];

        //Declare Text variables
        SpriteFont font;
        SpriteFont fontXL;
        
        //0 = angle, 1 = speed, 2 = points, 3 = balls, 4 = streak, 5 = High score
        Vector2 [] statsHUDLoc = new Vector2 [6];

        //Set the ball position for movement
        Vector2 ballPos;
        
        //Store text positions for points display in the high scores and end page. 0 = score, 1 = high score
        Vector2 [] scoresLoc = new Vector2 [2];

        int dirX = 0;       //Stores the x direction, which will be one of 1(right), -1(left) or 0(stopped)
        int dirY = 0;      //Stores the y direction, which will be one of 1(down), -1(up) or 0(stopped)
        
        //Store x and y speeds
        double xSpeed;
        double ySpeed;
        
        //Store balls
        int balls = 5;
        
        //Store if ball is launched
        bool ballMoving = false;
        
        //Store points and previous points
        int points;
        int prevPoints;
        
        //Store high scores
        int highScore;
        
        //Store streak
        int streak;
        
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
            
            TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 1000 / 60);

            base.Initialize();
            //Make mouse visible
            IsMouseVisible = true;
            
            //Get the window size
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            
            //Load in sprites and fonts
            //Ui
            easyBtnImg = Content.Load<Texture2D>("Sprites/BtnEasy");
            scoresBtnImg = Content.Load<Texture2D>("Sprites/BtnHighscores");
            exitBtnImg = Content.Load<Texture2D>("Sprites/BtnExit");
            resumeBtnImg = Content.Load<Texture2D>("Sprites/BtnResume");
            menuBtnImg = Content.Load<Texture2D>("Sprites/BtnMenu");
            blankImg = Content.Load<Texture2D>("Sprites/Blank");
            meterImg = Content.Load<Texture2D>("Sprites/PowerMeter");
            powerArrowImg = Content.Load<Texture2D>("Sprites/PowerArrow");
            
            font = Content.Load<SpriteFont>("Fonts/Font");
            fontXL = Content.Load<SpriteFont>("Fonts/FontXL");
            
            //Game assets
            ballImg = Content.Load<Texture2D>("Sprites/ball");
            bucketImg[0] = Content.Load<Texture2D>("Sprites/BlueBucket");
            bucketImg[1] = Content.Load<Texture2D>("Sprites/GreenBucket");
            bucketImg[2] = Content.Load<Texture2D>("Sprites/RedBucket");
            splashImg = Content.Load<Texture2D>("Sprites/Splash");
            
            //Load in the background
            bgImg[GAME] = Content.Load<Texture2D>("Backgrounds/CircusBG");
            bgImg[MENU] = Content.Load<Texture2D>("Backgrounds/MenuBG");
            bgImg[SCORES] = Content.Load<Texture2D>("Backgrounds/HighScoreBG");
            bgImg[END] = Content.Load<Texture2D>("Backgrounds/EndGameBG");
            
            //Set positions
            //Ui
            bgRec = new Rectangle(0, 0, screenWidth, screenHeight);
            easyBtnRec = new Rectangle(((screenWidth / 2) - (easyBtnImg.Width / 2)), 225, easyBtnImg.Width, easyBtnImg.Height);
            scoresBtnRec = new Rectangle(((screenWidth / 2) - (scoresBtnImg.Width / 2)), 300, scoresBtnImg.Width, scoresBtnImg.Height);
            exitBtnRec = new Rectangle(((screenWidth / 2) - (exitBtnImg.Width / 2)), 375, exitBtnImg.Width, exitBtnImg.Height);
            menuBtnRec[SCORES] =  new Rectangle(0, 0, menuBtnImg.Width, menuBtnImg.Height);
            menuBtnRec[END] =  new Rectangle((screenWidth /2 - menuBtnImg.Width /2), (screenHeight /2 - menuBtnImg.Height /2), menuBtnImg.Width, menuBtnImg.Height);
            resumeBtnRec = new Rectangle(((screenWidth / 2) - (resumeBtnImg.Width / 2)), ((screenHeight / 2) - (resumeBtnImg.Height / 2)), resumeBtnImg.Width, resumeBtnImg.Height);
            HUDRec = new Rectangle(0, 0, screenWidth, 32);
            angleMeterRec = new Rectangle(0, HUDRec.Height, meterImg.Width, meterImg.Height);
            speedMeterRec = new Rectangle(meterImg.Width, HUDRec.Height, meterImg.Width, meterImg.Height);
            
            //0 = angle, 1 = speed, 2 = points, 3 = balls, 4 = streak, 5 = High score
            statsHUDLoc[0] = new Vector2(0, 0);
            statsHUDLoc[1] = new Vector2(150, 0);
            statsHUDLoc[2] = new Vector2(330, 0);
            statsHUDLoc[3] = new Vector2(520, 0);
            statsHUDLoc[4] = new Vector2(640, HUDRec.Height + 5);
            statsHUDLoc[5] = new Vector2(640, 0);

            //Game assets
            ballPos = new Vector2(0, (screenHeight - 16));
            ballRec = new Rectangle(0, (screenHeight - 16), 16, 16);

            //Set bucket rectangles. blue = 0, green = 1, red = 2
            bucketRec[0] = new Rectangle((screenWidth - 100), (screenHeight - bucketImg[0].Height), bucketImg[0].Width, bucketImg[0].Height);
            bucketRec[1] = new Rectangle((screenWidth - 250), (screenHeight - bucketImg[1].Height), bucketImg[1].Width, bucketImg[1].Height);
            bucketRec[2] = new Rectangle((screenWidth - 450), (screenHeight - bucketImg[2].Height), bucketImg[2].Width, bucketImg[2].Height);
            
            bucketInRec[0] = new Rectangle((bucketRec[0].X), (bucketRec[0].Y + 3), bucketImg[0].Width, (bucketImg[0].Height / 2));
            bucketInRec[1] = new Rectangle((bucketRec[1].X), (bucketRec[1].Y + 5), bucketImg[1].Width, (bucketImg[1].Height / 2));
            bucketInRec[2] = new Rectangle((bucketRec[2].X), (bucketRec[2].Y + 10), bucketImg[2].Width, (bucketImg[2].Height / 2));
            
            //Splash location
            splashPos[0] = new Vector2(bucketRec[0].X , (bucketRec[0].Y - bucketImg[0].Height /2));
            splashPos[1] = new Vector2(bucketRec[1].X , (bucketRec[1].Y - bucketImg[1].Height /2));
            splashPos[2] = new Vector2(bucketRec[2].X , (bucketRec[2].Y - bucketImg[2].Height /2));
            //Load splash animations
            splashAnim[0] = new Animation(splashImg, 9, 5, 45, 45, 45, 2, 1, splashPos[0], 0.24f, false);
            splashAnim[1] = new Animation(splashImg, 9, 5, 45, 45, 45, 2, 1, splashPos[1], 0.48f, false);
            splashAnim[2] = new Animation(splashImg, 9, 5, 45, 45, 45, 2, 1, splashPos[2], 0.64f, false);
             
            Console.WriteLine("loaded :)");
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed /*|| Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                //gamestate = GAME;
                //Exit();

            // TODO: Add your update logic here

            //Update your keyboard and mouse position
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKb = kb;
            kb = Keyboard.GetState();
            
            //Update animations
            splashAnim[0].Update(gameTime);
            splashAnim[1].Update(gameTime);
            splashAnim[2].Update(gameTime);
            switch (gamestate)
            {
                
                case MENU:
                    //Check if user is pressing the exit button and close the game
                    if(exitBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        Exit();
                    
                    //Check if user is pressing the easy button and start the game in easy mode (by switching the gamestate)
                    if(easyBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        gamestate = GAME;
                    
                    //Check if user is pressing the score button and open the high score page (by switching the gamestate)
                    if(scoresBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        gamestate = SCORES;
                    break;
                
                case GAME:
                    //Check if user is pressing the pause button
                    if(kb.IsKeyDown(Keys.Escape) && kb != prevKb)
                        gamestate = PAUSE;
                    
                    //Add logic to change angle
                    if (kb.IsKeyDown(Keys.Up) && ballMoving == false)
                        angle += 2;
                    if (kb.IsKeyDown(Keys.Down) && ballMoving == false)
                        angle -= 2;
                    
                    //Add logic to change speed
                    if (kb.IsKeyDown(Keys.Right) && ballMoving == false)
                        speed += 0.3f;
                    if (kb.IsKeyDown(Keys.Left) && ballMoving == false)
                        speed -= 0.3f;
                    
                    //Make sure angle is always between 10 and 90
                    if (angle < 10)
                        angle = 10;
                    if (angle > 90)
                        angle = 90;
                    
                    //Make sure speed is always between 5 and 20 (unless you find the special key)
                    if (!kb.IsKeyDown(Keys.S))
                    {
                        if (speed < 5)
                            speed = 5;
                        if (speed > 20)
                            speed = 20;
                    }

                    //Calculate angle meter fill and fill the meter
                    angleMeterFill = Convert.ToInt32((angle - 10) * 2.6);
                    angleMeterFillRec = new Rectangle(meterImg.Width, (meterImg.Height + HUDRec.Height), (meterImg.Width * -1), (angleMeterFill * -1));
                    
                    //Calculate speed meter fill and fill the meter
                    speedMeterFill = Convert.ToInt32((speed - 5) * (meterImg.Height / 15));
                    speedMeterFillRec = new Rectangle((meterImg.Width * 2), (meterImg.Height + HUDRec.Height), (meterImg.Width * -1), (speedMeterFill * -1));
                    speedArrowRec = new Rectangle((speedMeterRec.X + meterImg.Width + 5), ((speedMeterFill * -1) - (powerArrowImg.Height / 2) + meterImg.Height + HUDRec.Height), powerArrowImg.Width, powerArrowImg.Height);
                    
                    
                    //Launch ball
                    if (kb.IsKeyDown(Keys.Space) && kb != prevKb && ballMoving == false)
                    {
                        ballMoving = true;
                        balls -= 1;
                        /*dirX = (1);
                        dirY = (angle / 10) * -1;*/
                        
                        xSpeed = speed * Math.Cos(angle * (Math.PI/180.0));
                        ySpeed = - speed * Math.Sin(angle * (Math.PI/180.0));
                        
                    }
                    
                    
                    //Calculate ball movement
                    if (ballMoving == true)
                    {
                        //Calculate gravity and move the ball
                        /*ballPos.X = ballPos.X + (dirX * speed);
                        ballPos.Y = ballPos.Y + (dirY * speed);*/
                        
                        ySpeed = ySpeed + GRAVITY;
                        ballPos.X = (float)(ballPos.X + xSpeed);
                        ballPos.Y = (float)(ballPos.Y + ySpeed);
                        ballRec.X = (int)ballPos.X;
                        ballRec.Y = (int)ballPos.Y;
                        
                        //Bounce ball
                        if (ballRec.Right > bgRec.Right || ballRec.Left < bgRec.Left || ballRec.Intersects(bucketRec[0]) || ballRec.Intersects(bucketRec[1]) || ballRec.Intersects(bucketRec[2]))
                            xSpeed = xSpeed * -1;
                    
                        if (ballRec.Top < (bgRec.Top + HUDRec.Height) /*|| ballRec.Bottom > bgRec.Bottom*/)
                            ySpeed = ySpeed * -1;
                        if (ballRec.Bottom > bgRec.Bottom && kb.IsKeyDown(Keys.B))
                            ySpeed = ySpeed * -1;
                        
                        //Check if ball hit the floor
                        if (!kb.IsKeyDown(Keys.B))
                        {
                            if (ballRec.Bottom > bgRec.Bottom)
                            {
                                ballMoving = false;
                                ballPos.X = 0;
                                ballPos.Y = (screenHeight - 16);
                                ballRec.X = (int)ballPos.X;
                                ballRec.Y = (int)ballPos.Y;
                                angle = 10;
                                speed = 5;
                                streak = 0;
                            }

                        }
                        
                        //Check if ball landed and add points
                        if (bucketInRec[0].Contains(ballRec) /*|| bucketInRec[0].Intersects(ballRec)*/)
                        {
                            ballMoving = false;
                            ballPos.X = 0;
                            ballPos.Y = (screenHeight - 16);
                            ballRec.X = (int)ballPos.X;
                            ballRec.Y = (int)ballPos.Y;
                            points += 400;
                            angle = 10;
                            speed = 5;
                            streak += 1;
                            splashAnim[0].isAnimating = true;
                        }
                        else if (bucketInRec[1].Contains(ballRec) /*|| bucketInRec[1].Intersects(ballRec)*/)
                        {
                            ballMoving = false;
                            ballPos.X = 0;
                            ballPos.Y = (screenHeight - 16);
                            ballRec.X = (int)ballPos.X;
                            ballRec.Y = (int)ballPos.Y;
                            points += 200;
                            angle = 10;
                            speed = 5;
                            streak += 1;
                            splashAnim[1].isAnimating = true;
                        }
                        else if (bucketInRec[2].Contains(ballRec) /*|| bucketInRec[1].Intersects(ballRec)*/)
                        {
                            ballMoving = false;
                            ballPos.X = 0;
                            ballPos.Y = (screenHeight - 16);
                            ballRec.X = (int)ballPos.X;
                            ballRec.Y = (int)ballPos.Y;
                            points += 100;
                            angle = 10;
                            speed = 5;
                            streak += 1;
                            splashAnim[2].isAnimating = true;
                        }
                        
                        //Check if the player is out of balls and end the game
                        if (balls <= 0 && ballMoving == false && splashAnim[0].isAnimating == false && splashAnim[1].isAnimating == false && splashAnim[2].isAnimating == false) 
                        {
                            gamestate = END;
                            balls = 5;
                            if (points > highScore)
                                highScore = points;
                            prevPoints = points;
                            points = 0;
                            streak = 0;
                        }
                    }
                    //Set ball position
                    
                    break;
                
                case PAUSE:
                    //Check if user is pressing the pause button
                    if(kb.IsKeyDown(Keys.Escape) && kb != prevKb)
                        gamestate = GAME;
                    //Check if user is pressing the resume button
                    if(resumeBtnRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        gamestate = GAME;
                    break;
                
                case SCORES:
                    //Check if user is pressing the menu button
                    if(menuBtnRec[SCORES].Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        gamestate = MENU;
                    
                    //Store text positions for points display in the high scores and end page. 0 = score, 1 = high score
                    scoresLoc[0] = new Vector2((screenWidth / 2) - (fontXL.MeasureString("Previous score: " + prevPoints) / 2).X, 25);
                    scoresLoc[1] = new Vector2((screenWidth / 2) - (fontXL.MeasureString("High Score: " + highScore) / 2).X, 125);
                    break;
                case END:
                    //Check if user is pressing the menu button
                    if(menuBtnRec[END].Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        gamestate = MENU;
                    
                    //Store text positions for points display in the high scores and end page. 0 = score, 1 = high score
                    scoresLoc[0] = new Vector2((screenWidth / 2) - (fontXL.MeasureString("Score: " + prevPoints) / 2).X, 25);
                    scoresLoc[1] = new Vector2((screenWidth / 2) - (fontXL.MeasureString("High Score: " + highScore) / 2).X, 125);
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
            
            //Draw the background for all gamestates except pause
            if (gamestate != PAUSE)
                spriteBatch.Draw(bgImg[gamestate], bgRec, Color.White);
                
            switch (gamestate)
            {
                case MENU:
                    spriteBatch.Draw(easyBtnImg, easyBtnRec, Color.White);
                    spriteBatch.Draw(scoresBtnImg, scoresBtnRec, Color.White);
                    spriteBatch.Draw(exitBtnImg, exitBtnRec, Color.White);
                    break;
                
                case GAME:
                    //Console.WriteLine(ballRec.X + ", " + ballRec.Y + "angle: " + angle + " " + ballMoving + " Points: " + points);
                    Console.WriteLine(Convert.ToString(splashAnim[0].isAnimating) + Convert.ToString(splashAnim[1].isAnimating) + Convert.ToString(splashAnim[2].isAnimating));
                    spriteBatch.Draw(ballImg, ballRec, Color.White);
                    spriteBatch.Draw(blankImg, angleMeterFillRec, Color.Red);
                    spriteBatch.Draw(meterImg, angleMeterRec, Color.White);
                    spriteBatch.Draw(blankImg, speedMeterFillRec, Color.Lime);
                    spriteBatch.Draw(meterImg, speedMeterRec, Color.White);
                    spriteBatch.Draw(powerArrowImg, speedArrowRec, Color.Lime);
                    spriteBatch.Draw(blankImg, HUDRec, Color.Black * 0.7f);
                    
                    //Draw buckets
                    spriteBatch.Draw(bucketImg[0], bucketRec[0], Color.White);
                    spriteBatch.Draw(bucketImg[1], bucketRec[1], Color.White);
                    spriteBatch.Draw(bucketImg[2], bucketRec[2], Color.White);
                    
                    spriteBatch.DrawString(font, "Angle: " + angle, statsHUDLoc[0], Color.Red);
                    spriteBatch.DrawString(font, "Speed: " + Math.Round(speed, 2), statsHUDLoc[1], Color.Lime);
                    spriteBatch.DrawString(font, "Points: " + points, statsHUDLoc[2], Color.Pink);
                    spriteBatch.DrawString(font, "Balls: " + balls, statsHUDLoc[3], Color.Orange);
                    spriteBatch.DrawString(font, "Streak: " + streak, statsHUDLoc[4], Color.Cyan);
                    spriteBatch.DrawString(font, "High: " + highScore, statsHUDLoc[5],  Color.Purple);
                    
                    //Draw splash
                    splashAnim[0].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                    splashAnim[1].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                    splashAnim[2].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);

                    if (false)
                    {
                        spriteBatch.Draw(blankImg, bucketInRec[2], Color.Magenta);
                        spriteBatch.Draw(blankImg, bucketInRec[1], Color.Magenta);
                        spriteBatch.Draw(blankImg, bucketInRec[0], Color.Magenta);
                    }
                    
                    break;
                
                case SCORES:
                    spriteBatch.Draw(menuBtnImg, menuBtnRec[gamestate], Color.White);
                    spriteBatch.DrawString(fontXL, "Previous score: " + prevPoints, scoresLoc[0], Color.Black);
                    spriteBatch.DrawString(fontXL, "High score: " + highScore, scoresLoc[1], Color.Black);
                    break;
                
                case PAUSE:
                    spriteBatch.Draw(resumeBtnImg, resumeBtnRec, Color.White);
                    break;
                
                case END:
                    spriteBatch.Draw(menuBtnImg, menuBtnRec[gamestate], Color.White);
                    spriteBatch.DrawString(fontXL, "Score: " + prevPoints, scoresLoc[0], Color.Black);
                    spriteBatch.DrawString(fontXL, "High score: " + highScore, scoresLoc[1], Color.Black);
                    
                    break;
            }
            
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}