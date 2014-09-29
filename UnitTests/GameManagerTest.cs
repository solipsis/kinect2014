using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Kinect;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class GameManagerTest
    {   

        [TestMethod]
        public void TestConfigLoad()
        {
            Directory.SetCurrentDirectory(@"..\..\TestFiles\GameManager");
            
            GameManager.LoadConfig(@"TestGameManagerConfig.xml");

        }
    }
}
