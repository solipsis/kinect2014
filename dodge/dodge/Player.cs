using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dodge
{
    class Player
    {
        Vector2 position;
        Texture2D texture;
        Rectangle screenBounds;
       

         public Player(Texture2D texture, Rectangle screenBounds)
        {
            this.texture = texture;
            this.screenBounds = screenBounds;
          
            setInStartPosition();
        }

        public void setPosition(float x, float y)
        {
            // kinect positions range from -1 to 1 and this maps it to pixels within screenbounds
            float scaled_x = ((1.5f * x) + 0.5f) * screenBounds.Width ;
            float scaled_y = ((3.9f * y) + 0.5f) * screenBounds.Height * -1 ;

            System.Console.WriteLine("xPos: " + position.X);

            position = new Vector2(scaled_x, scaled_y + 600);


            // make sure the reticule stays in bounds
            
            if (position.X + (texture.Width / 2) < screenBounds.Left)
            {
                position.X = screenBounds.Left - (texture.Width / 2);
            }
            if (position.X + (texture.Width / 2) > screenBounds.Right)
            {
                position.X = screenBounds.Right - (texture.Width / 2);
            }

            if (position.Y + (texture.Height / 2) < screenBounds.Top)
            {
                position.Y = screenBounds.Top - (texture.Height / 2);
            }
            if (position.Y + (texture.Height / 2) > screenBounds.Bottom)
            {
                position.Y = screenBounds.Bottom - (texture.Height / 2);
            }    
        }

        public Rectangle getHitbox()
        {
            return new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)(texture.Width - 10), (int)(texture.Height - 10));
        }

        public void setInStartPosition()
        {
            position.X = screenBounds.Width / 2;
            position.Y = screenBounds.Height / 2;
        }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            spriteBatch.Draw(texture, position, tint);
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
