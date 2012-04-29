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

            Console.ReadKey();

        }

        static void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
             var colorImageFrame = e.OpenColorImageFrame();
             if (colorImageFrame != null)
             {
                 var face = new FaceDetectionRecognition();
                 var pixelData = new byte[colorImageFrame.PixelDataLength];
                 colorImageFrame.CopyPixelDataTo(pixelData);

                 var detectedFace = face.GetDetectedFace(pixelData, colorImageFrame.Height, colorImageFrame.Width);

                 if(detectedFace != null)
                 {
                     Console.WriteLine("I recognize you with a mouth!");
                 }
             }
        }
    }
}
