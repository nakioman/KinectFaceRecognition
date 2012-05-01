using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            using (var ms = new MemoryStream(PixelData))
            {
                var bitmap = new Bitmap(ms);
                return bitmap;
            }
            
        }
    }
}