using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameManager.LoadConfig("GameManagerConfig.xml");

            foreach(GameInfo g in GameManager.ListGames())
            {

            }


            Kinect.App app = new Kinect.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
