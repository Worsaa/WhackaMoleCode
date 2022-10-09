using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Security.Principal;
using WhackaMole;
using static System.Formats.Asn1.AsnWriter;
using static WhackaMole.Mole;


namespace WhackaMole
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public enum GameState { GameMenu = 1, GamePlay = 2, GamePostLobby = 3 }
        public GameState currentGameState = GameState.GameMenu;
  
        public SpriteFont textFont;
        public Texture2D backGroundTree;
        public Vector2 backGroundPos;

        public Texture2D holeTex;
        public Texture2D foreTex;
        public Texture2D moleTex;

        Mole[,] moles2DArray;
        bool isHitMole;
        float timer = 20;
        public int intScore;
        MouseState mouseState, oldMouseState;   

        //Animation
        public Texture2D animationStone;
        public Vector2 animationStonePos;
        public Vector2 animationStoneVelocity;   
        public Vector2 animationHolePos;
        public Vector2 animationForePos;
        Animation animation;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 650;
            graphics.PreferredBackBufferHeight = 650;
        }

        protected override void Initialize()
        {            
            intScore = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            animationStone = Content.Load<Texture2D>("spritesheet_stone");
            textFont = Content.Load<SpriteFont>("font");
            holeTex = Content.Load<Texture2D>("hole");
            foreTex = Content.Load<Texture2D>("hole_foreground");                              
            backGroundTree = Content.Load<Texture2D>("backgroundTop");
            backGroundPos = new Vector2(0, 0);
            mouseState = Mouse.GetState();

            animationStonePos = new Vector2(75, 20);
            animationStoneVelocity = new Vector2(0, -2);
            animationHolePos = new Vector2(20, 500);
            animationForePos = new Vector2(20, 500);
            animation = new Animation(animationStone, animationStonePos, animationStoneVelocity);
      
            moleTex = Content.Load<Texture2D>("mole");
            moles2DArray = new Mole[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = j * 200 + 35; 
                    int y = i * 150 + 200;
                                           
                    moles2DArray[i, j] = new Mole(moleTex, holeTex, foreTex, x, y);               
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
                       
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            switch (currentGameState)
            {
                case GameState.GameMenu:

                    animation.Update(gameTime);

                    break;
                case GameState.GamePlay:

                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            moles2DArray[i, j].Update((float)gameTime.ElapsedGameTime.TotalSeconds);                         
                        }
                    }

                    foreach (Mole mole in moles2DArray)
                    {                                           
                        if (mole.moleHitBox.Contains(mouseState.X, mouseState.Y) && oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            isHitMole = mole.isHitMole(mouseState.X, mouseState.Y);
                            mole.currentMoleState = Mole.MoleState.moleGoingDown;

                            if (isHitMole) 
                            {                              
                                intScore += 1;
                            }
                        }
                    }
                    if (timer <= 0)
                    {
                        currentGameState = GameState.GamePostLobby; 
                    }

                    break;

                case GameState.GamePostLobby:
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                
                case GameState.GameMenu: 
                    GraphicsDevice.Clear(new Color(111, 209, 72, 255));
                    spriteBatch.Begin();

                    // Press space för att gå till nästa gameState                 
                    spriteBatch.Draw(foreTex, animationForePos, Color.White);
                    spriteBatch.DrawString(textFont, "Press Spacebar to start", new Vector2(175, 300), Color.AntiqueWhite);
                    spriteBatch.Draw(holeTex, animationHolePos, Color.White);
                    animation.Draw(spriteBatch);
                    
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                        currentGameState = GameState.GamePlay;
                    
                    spriteBatch.End();
                    break;
               
                case GameState.GamePlay:
                    GraphicsDevice.Clear(new Color(111, 209, 72, 255));                 
                    spriteBatch.Begin();

                    spriteBatch.Draw(backGroundTree, backGroundPos, Color.White);
                    spriteBatch.DrawString(textFont, "Timer:" + timer.ToString("0"), new Vector2(10, 40), Color.Red);
                    spriteBatch.DrawString(textFont, "Score:" + intScore, new Vector2(10, 10), Color.White);
                   
                    for (int i = 0; i < moles2DArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < moles2DArray.GetLength(1); j++)
                        {
                            moles2DArray[i, j].Draw(spriteBatch);                          
                        }
                    }

                    spriteBatch.End();
                    break;
                 
                case GameState.GamePostLobby:
                    GraphicsDevice.Clear(new Color(111, 209, 72, 255));
                    spriteBatch.Begin();

                    spriteBatch.DrawString(textFont, "Game over", new Vector2(255, 300), Color.White);
                    spriteBatch.DrawString(textFont, "Your score:" + intScore, new Vector2(245, 335), Color.White);

                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
