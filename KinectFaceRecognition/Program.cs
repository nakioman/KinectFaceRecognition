using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Kinect;

namespace KinectFaceRecognition
{
    class Program
    {
        private static KinectSensor _sensor;

        static void Main(string[] args)
        {            
            
            
            _sensor =  KinectSensor.KinectSensors.FirstOrDefault();
            _sensor.ColorStream.Enable();
            _sensor.SkeletonStream.Disable();
            _sensor.DepthStream.Disable();
            _sensor.Start();

            _sensor.ColorFrameReady += SensorColorFrameReady;

            while (true)
            {
                
            }
        }

        static void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
             var colorImageFrame = e.OpenColorImageFrame();
             if (colorImageFrame != null)
             {
                 var faceDetection = new FaceDetectionRecognition();
                 var pixelData = new byte[colorImageFrame.PixelDataLength];
                 colorImageFrame.CopyPixelDataTo(pixelData);

                 var detectedFace = faceDetection.GetDetectedFace(pixelData, colorImageFrame.Height, colorImageFrame.Width);

                 if(detectedFace != null)
                 {
                     var user = faceDetection.RecognizeFace(detectedFace);
                     if (user != null)
                     {
                         Console.WriteLine("I recognize you with a mouth!, your are: {0}", user.NickName);    
                     }
                     else
                     {
                         Console.Clear();
                         Console.WriteLine("You are a new user, would you like to be added to the database? Y/N");
                         var key = Console.ReadKey();

                         if (key.KeyChar == 'Y' || key.KeyChar == 'y')
                         {
                             Console.WriteLine("Please provide a nick name for you: ");
                             var name = Console.ReadLine();

                             faceDetection.SaveNewDetectedFace(name, detectedFace);
                         }
                     }
                     
                 }
             }
        }
    }
}
