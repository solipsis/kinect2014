using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect
{
    class GameButton : System.Windows.Controls.Button
    {
        public String Title { get; set; }
        public String Description { get; set; }

        public GameButton(string title, string description) : base()
        {
            Title = title;
            Description = description;
        }
    }
}
