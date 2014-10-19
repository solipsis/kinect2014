using System;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

//using Microsoft.Speech;


using Microsoft.Kinect;
namespace KinectCounter_old
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBox.Show(KinectSensor.KinectSensors.Count.ToString() + " Kinect sensors detected" );
        }
    }
}
