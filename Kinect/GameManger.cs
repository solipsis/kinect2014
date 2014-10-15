using System;
using System.IO;
using System.Collections;
using System.Data.Common;
using System.Xml;

namespace Kinect
{
    public class GameManager
    {
        //Name of Schema
        private const string configSchema = "GameManagerConfig.xsd";
        //List of all found games
        private ArrayList gameList;
        //Refrence to the singleton instance
        private static GameManager _Instance;

        //
        private string configFile;
        private DirectoryInfo gamesDir;
        private string gameInfoConfigFileName;
   
        private GameManager(string config)
        {
            this.configFile = config;
            this.gameList = new ArrayList();
            this.ReadConfig();
        }

        private void ReadConfig()
        {
            //Load the Schema
            XmlReaderSettings readSettings = new XmlReaderSettings();
            readSettings.Schemas.Add(null, configSchema);
            readSettings.ValidationType = ValidationType.Schema;

            //Load the config
            XmlDocument myDoc;
            myDoc = new XmlDocument();
            myDoc.Load(this.configFile);

            //Create a reader for the config
            XmlReader read = XmlReader.Create(new StringReader(myDoc.InnerXml), readSettings);
            string curName;

            //Read through
            while (read.Read())
            {   
                //Read each node(only elements
                switch (read.NodeType)
                {
                    case XmlNodeType.Element:
                        curName = read.Name;
                        switch (curName)
                        {
                            //Process each of the tags
                            case "GameDirectory":
                                this.gamesDir = new DirectoryInfo(read.ReadString());                                
                                break;
                            case "GameInfoConfig":
                                this.gameInfoConfigFileName = read.ReadString();
                                break;     
                        }
                        break;
                }
            }
            read.Close();
        }
        
        //Load a config file
        public static void LoadConfig(string config)
        {
            _Instance = new GameManager(config);
            findGames();
        }
        
        //Scan the Games directory for any games availble
        public static void findGames()
        { 
            GameInfo newGame;

            //List each of the sub dirs           
            foreach( DirectoryInfo gameDir in _Instance.gamesDir.EnumerateDirectories())
            {
               
                //Check each subdirectory for the config file
                foreach(FileInfo gameConfig in gameDir.GetFiles(_Instance.gameInfoConfigFileName))
                {
                    newGame = new GameInfo(gameConfig.FullName);
                    _Instance.gameList.Add(newGame);
                }
            }
            return;
        }

        public static void LaunchGame(String path)
        {
            Launcher gameLauncher = new Launcher(path);
        }

        public static ArrayList ListGames()
        {
                
            // ignore please...
            //TODO



            //Clone is shallow copy only but faster.  Change to .Copy if a deep cop is needed
            return (ArrayList)_Instance.gameList.Clone();
        }

    }
}