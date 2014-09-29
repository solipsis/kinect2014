using System;
using System.IO;
using System.Collections;


namespace Kinect
{
    public class GameManager
    {
        private ArrayList gameList;
        private static GameManager _instance;

        private string configFile;
        private string gamesDir;
   
        public GameManager(string config)
        {
            this.configFile = config;   

            _instance = this;
        }
        
        public static void findGames()
        { 
        
        }

        public static void launchGame(int gameID)
        {
 
        }

        public static ArrayList listGames()
        {
            return null;
        }

    }
}