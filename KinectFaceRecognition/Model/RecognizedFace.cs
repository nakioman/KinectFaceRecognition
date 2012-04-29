using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KinectFaceRecognition.Model
{
    public class RecognizedFace
    {
        public byte[] PixelData { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public Bitmap GetBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;

            Marshal.Copy(PixelData, 0, ptr, PixelData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}