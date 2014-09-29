using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kinect
{
    public class GameInfo
    {
        public String Path {get; set;}
        public String Title {get; set;}
        public String Description {get; set;}
        public String ImagePath { get; set; }


        public GameInfo() { }
        public GameInfo(String filename)
        {
            //string xml = File.ReadAllText(filename);
            parseXML(filename);
        }

        private void parseXML(string filename)
        {
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                reader.ReadToFollowing("title");
                Title = reader.ReadElementContentAsString();
                reader.ReadToFollowing("path");
                Path = reader.ReadElementContentAsString();
                reader.ReadToFollowing("description");
                Description = reader.ReadElementContentAsString();
                reader.ReadToFollowing("img_path");
                ImagePath = reader.ReadElementContentAsString();
            }
        }
    }
}
