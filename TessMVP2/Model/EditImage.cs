using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace TessMVP2.Model
{
    class EditImage
    {

        /*https://dotnet-snippets.de/snippet/bitmap-in-graustufen-wandeln/70*/

        public static void BildSW(string file)
        {
            Image img = Image.FromFile(file);
            Bitmap bmp = new Bitmap(img);
            PixelFormat pxf = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            int tmpSW;

            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {
                tmpSW = rgbValues[counter];
                tmpSW += rgbValues[counter + 1];
                tmpSW += rgbValues[counter + 2];

                tmpSW /= 3;

                rgbValues[counter] = rgbValues[counter + 1] = rgbValues[counter + 2] = Convert.ToByte(tmpSW);
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(bmpData);

            var anEncoder = System.Drawing.Imaging.Encoder.Quality;
            ImageCodecInfo aImageCodecInfo = GetEncoderInfo("image/jpeg");
            var encoderParams = new EncoderParameters(1);
            var encoderParam = new EncoderParameter(anEncoder, 75L);
            encoderParams.Param[0] = encoderParam;
            img = null;
            File.Delete(file);
            file.Remove(file.Length-4)
            bmp.Save(file,aImageCodecInfo,encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            //https://msdn.microsoft.com/de-de/library/ytz20d80(v=vs.110).aspx

            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
