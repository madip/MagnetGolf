using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class testGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerSprite;
        Vector2 playerPos;
        Vector2 playerVel;
        SpriteFont asd;
        MouseState mousestate = Mouse.GetState();
        Vector2 mouseLocation;
        float kiirendus = 0.99999999f;
        Texture2D tex;
        Vector2 joonalgus;
        Vector2 joonlopp;
        Vector2 eelminealgus;
        Vector2 eelminelopp;
        Texture2D blackHoleSprite;
        Vector2 blackHolePos;
        Vector2 blackHolePushPos;
        Texture2D cursor;
        Color varv;
        int strike = 0;
        public testGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //IsMouseVisible = true;
        }

        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            playerPos = new Vector2(250, 250);
            playerVel = new Vector2(0, 0);
            blackHolePos = new Vector2( 500, 280);
            mouseLocation = new Vector2(mousestate.X, mousestate.Y);
            blackHolePushPos = new Vector2(100, 100);

            // TODO: Add your initialization logic here

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
            blackHoleSprite = Content.Load<Texture2D>("bHole");
            playerSprite = Content.Load<Texture2D>("ball");
            cursor = Content.Load<Texture2D>("cursor");
            
           tex = Content.Load<Texture2D>("line");
            
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (mousestate.RightButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                playerPos = new Vector2(250, 250);
                playerVel = new Vector2(0, 0);
                strike = 0;
            }

            Window.Title = "Vector: X: " + (playerPos.X - mousestate.X) + " Y: " + (playerPos.Y - mousestate.Y) + " Velocity: X: " + playerVel.X + " Y: " + playerVel.Y + " Distance: " + Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y));
            if (mousestate.LeftButton == ButtonState.Pressed)
            {

                if (strike == 0)
                {

                    joonalgus = playerPos;
                    joonlopp = new Vector2(mousestate.X, mousestate.Y);
                    colorChange();

                    if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {

                        eelminealgus = joonalgus;
                        eelminelopp = joonlopp;
                        joonalgus = Vector2.Zero;
                        joonlopp = Vector2.Zero;
                        kiirendus = 0.99f;
                        float a = 0;



                        //Et kiirus ei ületaks teatud piiri(160px / 5)
                        if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) > 160)
                        {
                            a = (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) - 160) * 100 / Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y));
                            playerVel = new Vector2(playerPos.X - mousestate.X, playerPos.Y - mousestate.Y) * ((100 - a) / 100) / 5 * -1;

                        }
                        else
                        {
                            playerVel = new Vector2(((playerPos.X - mousestate.X)), ((playerPos.Y - mousestate.Y))) * -1 / 5;

                        }
                        strike = 1;
                    }
                }
            }
                    
            if (playerVel.X < -0.01 || playerVel.X > 0.01 || playerVel.Y < -0.01 || playerVel.Y > 0.01)
            {

                kiirendus = kiirendus - 0.00002f;
                playerVel.X = playerVel.X * kiirendus;
                playerVel.Y = playerVel.Y * kiirendus;
            }
            else 
            {
                playerVel.X = 0;
                playerVel.Y = 0;
            
            }
                    mousestate = Mouse.GetState();
                    playerPos += playerVel;
                    blackHoleSink();
                    blackHolePush();
                    CheckBounds();

                    // TODO: Add your update logic here

                    base.Update(gameTime);
        }
        
        private void DRAWLINE(Vector2 p1, Vector2 p2, Color color) 
        {
            float angle = (float)Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            float dist = Vector2.Distance(p1, p2);

            spriteBatch.Begin();
            spriteBatch.Draw(tex, new Rectangle((int)p2.X, (int) p2.Y, (int)dist, 3), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();

        }

        private void blackHoleSink() 
        {
            if(Vector2.Distance(blackHolePos, playerPos) < 120)
            {
                playerVel.X = playerVel.X + (playerPos.X - blackHolePos.X) * -0.005f;
                playerVel.Y = playerVel.Y + (playerPos.Y - blackHolePos.Y) * -0.005f;
            }
        }

        private void blackHolePush()
        {
            if (Vector2.Distance(blackHolePushPos, playerPos) < 85)
            {
                playerVel.X = playerVel.X + (playerPos.X - blackHolePushPos.X) * 0.005f;
                playerVel.Y = playerVel.Y + (playerPos.Y - blackHolePushPos.Y) * 0.005f;
            }
        }

        private void colorChange()
        {
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 0)
            {
                varv = Color.LimeGreen;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 15)
            {
                varv = Color.Lime;
            }
            if(Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 30)
            {
                varv = Color.GreenYellow;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 60)
            {
                varv = Color.Yellow;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 80)
            {
                
                varv = Color.Orange;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 100)
            {
                
                varv = Color.DarkOrange;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 120)
            {
                varv = Color.Tomato;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 140)
            {
                varv = Color.OrangeRed;
            }
            if (Vector2.Distance(playerPos, new Vector2(mousestate.X, mousestate.Y)) >= 160)
            {
                varv = Color.Red;
            }
        }

        private void CheckBounds()
        {
            if (playerPos.X >= GraphicsDevice.Viewport.Width - playerSprite.Width/2)
            {
                if (playerVel.X > 0)
                {
                    playerVel.X *= -1;
                }
            }
            else if (playerPos.X - playerSprite.Height / 2 <= 0)
            {
                playerVel.X *= -1;
            }
            if (playerPos.Y >= GraphicsDevice.Viewport.Height - playerSprite.Height/2)
            {
                if (playerVel.Y > 0)
                {
                    playerVel.Y *= -1;
                }
                
            }
            else if (playerPos.Y - playerSprite.Height / 2 <= 0)
            {
                playerVel.Y *= -1;
            }
            
            }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(playerSprite, playerPos, null, Color.White, 0.0f, new Vector2(playerSprite.Width / 2, playerSprite.Height / 2), 0.8f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(cursor, new Vector2(mousestate.X, mousestate.Y), null, Color.White, 0.5f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(blackHoleSprite, blackHolePos, null, Color.Black, 0.0f, new Vector2(blackHoleSprite.Width / 2, blackHoleSprite.Height / 2), 0.8f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(blackHoleSprite, blackHolePushPos, null, Color.Red, 0.0f, new Vector2(blackHoleSprite.Width / 2, blackHoleSprite.Height / 2), 0.6f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
            DRAWLINE(eelminealgus, eelminelopp, Color.LightGray);
            DRAWLINE(joonalgus, joonlopp, varv);
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
