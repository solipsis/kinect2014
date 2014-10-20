using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace KinectCounter_new
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectSensor ks = KinectSensor.GetDefault();

            Console.WriteLine(ks.ToString());

            Console.WriteLine("Kinect Default ID: " + ks.UniqueKinectId);


        }
    }
}
