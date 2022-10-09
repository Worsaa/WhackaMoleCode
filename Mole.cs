using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;
using static WhackaMole.Game1;

namespace WhackaMole
{
    public class Mole
    {
        public Texture2D moleTex;
        public Texture2D holeTex;
        public Texture2D foreTex;

        public Vector2 molePos;
        public Vector2 moleStartPos;
        public Vector2 direction;
        public Vector2 velocity;           
        public Rectangle moleHitBox;
        public float moleMaxPos;
        public bool moleHit;
           
        Random random;
        float timer;
        float RandomTimerUp;
        float RandomTimerDown;      

        public enum MoleState { moleGoingUp = 1, moleIsUp = 2, moleGoingDown = 3, moleIsDown = 4 };
        public MoleState currentMoleState = MoleState.moleIsDown;

        public Mole(Texture2D moleTex, Texture2D holeTex, Texture2D foreTex, int posX, int posY)//Rectangle HitBox)
        {
            this.moleTex = moleTex;
            this.holeTex = holeTex;
            this.foreTex = foreTex;
            this.molePos = new Vector2(posX, posY);
            this.moleHitBox = new Rectangle((int)posX, (int)posY, moleTex.Width, moleTex.Height);

            moleStartPos = molePos;
            direction = new Vector2(0, -1);
            velocity = new Vector2(0, 2);         
            moleHit = false;           
        }

        public void Update(float elapsedSeconds) 
        {           
            moleHitBox = new Rectangle((int)molePos.X, (int)molePos.Y, moleTex.Width, (int)(moleStartPos.Y - molePos.Y + 30));
            moleMaxPos = moleStartPos.Y - 180;
                  
            random = new Random();
            RandomTimerUp = random.Next(2, 5);
            RandomTimerDown = random.Next(2, 12);

            switch (currentMoleState) 
            {
                case MoleState.moleGoingUp:
              
                    molePos = molePos + direction * velocity;
                    
                    if (molePos.Y > moleStartPos.Y || molePos.Y < moleMaxPos)
                    {                        
                        direction = direction * -1;
                        currentMoleState = MoleState.moleIsUp;                       
                    }

                    break;
                
                case MoleState.moleIsUp:
                    
                    timer -= elapsedSeconds;
                    if (timer <= 0f)
                    {                      
                        timer += RandomTimerUp;                      
                        currentMoleState = MoleState.moleGoingDown;
                    }

                    break;
                
                case MoleState.moleGoingDown:

                    molePos = molePos + direction * velocity;

                    if (molePos.Y > moleStartPos.Y || molePos.Y < moleMaxPos)
                    {
                        direction = direction * -1;
                        currentMoleState = MoleState.moleIsDown;                       
                    }

                    break;
                
                case MoleState.moleIsDown:
                    
                    timer -= elapsedSeconds;
                    if (timer <= 0f)
                    {
                        timer += RandomTimerDown;                    
                        currentMoleState = MoleState.moleGoingUp;
                        moleHit = false;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {          
            spriteBatch.Draw(holeTex, moleStartPos, Color.White);         
            spriteBatch.Draw(moleTex, molePos, Color.White);
            spriteBatch.Draw(foreTex, moleStartPos, Color.White);          
        }

        public bool isHitMole(int x, int y)
        {
            moleHit = false;
            if (moleHitBox.Contains(x, y))
            {
                moleHit = true;
            }
            return moleHit;
        }
    } 
}
