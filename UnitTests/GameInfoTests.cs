using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kinect;

namespace UnitTests
{
    [TestClass]
    public class GameInfoTests
    {
        [TestMethod]
        public void TestCreation()
        {
            GameInfo info = new GameInfo("GameInfoTest.xml");
            Assert.AreEqual("cool game", info.Title);
            Assert.AreEqual("../pony", info.Path);
            Assert.AreEqual("a very cool game", info.Description);
            Assert.AreEqual("images/dinosaur.png", info.ImagePath);
        }

//TODO add test for malformed xml
    }
}
