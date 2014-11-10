using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dodge
{
    class Obstacle
    {
        Vector2 position;
        Texture2D texture;
        Rectangle screenBounds;
        int angle;
        int x_speed;
        

        public Obstacle( Rectangle screenBounds, int angle, Vector2 position)
        {          
            this.screenBounds = screenBounds;
            this.position = position;
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            x_speed = random.Next(-1, 2);
        }

        public void move() {
            position.Y++;
            position.X += x_speed;
        }

       

        public Vector2 getPosition()
        {
            return position;
        }

        public void update()
        {
            move();
        }

        public Rectangle getHitbox(Texture2D texture)
        {
            return new Rectangle((int)position.X + 20, (int)position.Y + 20, (int)(texture.Width - 20), (int)(texture.Height - 20));
        }
    }
}
