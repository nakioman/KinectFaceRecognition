using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;

namespace KinectFaceRecognition
{
    public class FaceDetectionRecognition
    {
        public HaarCascade Face = new HaarCascade("Cascades/haarcascade_frontalface_alt2.xml");//haarcascade_frontalface_alt_tree.xml");
        public HaarCascade Mouth = new HaarCascade("Cascades/haarcascade_mcs_mouth.xml");

        /// <summary>
        /// This get the face detected by the pixel data
        /// NOTE: This will only return the first face data with a visible mouth
        /// </summary>
        /// <param name="pixelData"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Image<Gray, byte> GetDetectedFace(byte[] pixelData, int height, int width)
        {
            var bitmap = BytesToBitmap(pixelData, height, width);
            var image = new Image<Bgr, byte>(bitmap);
            var grayImage = image.Convert<Gray, Byte>();

            //Face Detector
            var facesDetected = Face.Detect(grayImage, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            //Action for each element detected
            foreach (var faceFound in facesDetected)
            {
                var face = image.Copy(faceFound.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);

                face._EqualizeHist();
                IsMouthDetected(face);

                return face;
            }

            return null;
        }

        private static Bitmap BytesToBitmap(byte[] pixelData, int height, int width)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;

            Marshal.Copy(pixelData, 0, ptr, pixelData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// This method detect true if we found a mouth in the image
        /// NOTE: The idea is to transform this method in true, when the user is speaking (mouth open)
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public bool IsMouthDetected(Image<Gray, byte> face)
        {
            var detectRectangle = new Rectangle(0, face.Height * 2 / 3, face.Width, face.Height / 3);
            var whereMouthShouldBe = face.GetSubRect(detectRectangle);
            var mouths = Mouth.Detect(whereMouthShouldBe, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(5, 5));

            return mouths.Any();
        }
    }
}