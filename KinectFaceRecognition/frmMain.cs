using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace KinectFaceRecognition
{
    public partial class FrmMain : Form
    {
        private KinectSensor _sensor;
        private string _tempName;
        private int _remainingShots;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            var colorImageFrame = e.OpenColorImageFrame();
            if (colorImageFrame != null)
            {


                var faceDetection = new FaceDetectionRecognition();
                var pixelData = new byte[colorImageFrame.PixelDataLength];
                colorImageFrame.CopyPixelDataTo(pixelData);

                pictureBox3.Image = FaceDetectionRecognition.BytesToBitmap(pixelData, colorImageFrame.Height, colorImageFrame.Width);
                pictureBox3.Refresh();
                var detectedFace = faceDetection.GetDetectedFace(pixelData, colorImageFrame.Height, colorImageFrame.Width);



                if (detectedFace != null)
                {
                    pictureBox4.Image = detectedFace.Bitmap;
                    pictureBox4.Refresh();

                    if (_remainingShots > 0)
                    {
                        faceDetection.SaveNewDetectedFace(_tempName, detectedFace);
                        _remainingShots--;
                    }
                    else
                    {
                        var user = faceDetection.RecognizeFace(detectedFace);


                        if (user != null)
                        {
                            Console.WriteLine("I recognize you with a mouth!, your are: {0}", user.NickName);
                            pictureBox2.Image = user.Face.GetBitmap();
                            pictureBox2.Refresh();
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
                                _tempName = name;
                                _remainingShots = 39;
                            }
                        }
                    }
                }
            }
        }

        private void FrmMain_Load_1(object sender, EventArgs e)
        {
            _sensor = KinectSensor.KinectSensors.FirstOrDefault();
            _sensor.ColorStream.Enable();
            _sensor.SkeletonStream.Disable();
            _sensor.DepthStream.Disable();
            _sensor.Start();

            _sensor.ColorFrameReady += SensorColorFrameReady;
        }
    }
}
