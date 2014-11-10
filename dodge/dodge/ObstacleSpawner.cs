using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace dodge
{
    class ObstacleSpawner
    {
        
        int angle;

        int delay;
        int last_spawn;
        Texture2D obstacleTexture;
        Rectangle screenBounds;
        ArrayList obstacles;
        Random random;

        public ObstacleSpawner(int angle, Rectangle screenBounds, ArrayList obstacles)
        {
            this.angle = angle;          
            this.screenBounds = screenBounds;
            this.obstacles = obstacles;
            last_spawn = 0;
            delay = 20;
            random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
           
        }

        public void spawnObstacle() 
        {
            
            Console.WriteLine(screenBounds.Width);
            int x = random.Next(0, screenBounds.Width);
            //Vector2 position = new Vector2(random.Next(0, screenBounds.Width), -10);
            Vector2 position = new Vector2(x, -100);
           // if (angle == 90) {
           //     position = 
          //  }
            obstacles.Add(new Obstacle(screenBounds, angle, position));
        }

        public void update()
        {
            last_spawn++;
            if (last_spawn == delay)
            {
                spawnObstacle();
                last_spawn = 0;
            }
        }
    }
}
