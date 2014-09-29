using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kinect
{
    class GameInfo
    {
        private String Path {get; set;}
        private String Title {get; set;}
        private String Description {get; set;}
        private String ImagePath { get; set; }
 
        public GameInfo(String filename)
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
