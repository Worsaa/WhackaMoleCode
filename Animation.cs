using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;
using static WhackaMole.Game1;

namespace WhackaMole
{
    public class Animation
    {
        public Texture2D animationTex;
        public Vector2 animationPos;
        public Vector2 animationVelocity;      
        public Vector2 direction;
     
        double timeSinceLastFrame;
        double timeBetweenFrames;
        Point currentFrame;
        Point numberOfFrames;

        public Animation(Texture2D animationTex, Vector2 animationPos, Vector2 animationVelocity)
        {
            this.animationTex = animationTex;
            this.animationPos = animationPos;
            this.animationVelocity = animationVelocity;
            direction = new Vector2(0, -1);

            timeSinceLastFrame = 0;
            timeBetweenFrames = 0.1;
            currentFrame = new Point(0, 2);
            numberOfFrames = new Point(4, 4);          
        }

        public void Update(GameTime gameTime)
        {
            animationPos = animationPos + direction * animationVelocity;
            
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= timeBetweenFrames)
            {
                timeSinceLastFrame -= timeBetweenFrames;
                currentFrame.X++;

                if (currentFrame.X >= numberOfFrames.X)
                {

                    currentFrame.X = 0; 
                    currentFrame.Y++;

                    if (currentFrame.Y >= numberOfFrames.Y) 
                    { 
                        currentFrame.Y = 0; 
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(currentFrame.X * animationTex.Width / numberOfFrames.X - 3, currentFrame.Y * animationTex.Height /
            numberOfFrames.Y, animationTex.Width / numberOfFrames.X, animationTex.Height / numberOfFrames.Y);

            spriteBatch.Draw(animationTex, animationPos, source, Color.White);            
        }
    }
}
