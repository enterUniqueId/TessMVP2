using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using TessMVP2.Model.Interfaces;


namespace TessMVP2.Model
{
    public class EditImage:IImgFileSave
    {
        private string _newFilepath;
        public string NewFilepath { get { return this._newFilepath; } set { this._newFilepath = value; } }


        public string EncodeAndSave(string file, Bitmap bmp)
        {
            var anEncoder = System.Drawing.Imaging.Encoder.Quality;
            //Zielformat
            ImageCodecInfo aImageCodecInfo = GetEncoderInfo("image/jpeg");
            //parameter in parameters arr; parameters anlegen, dann parameter einfügen
            var encoderParams = new EncoderParameters(1);
            //eigentliche Kompressionseinstellung
            var encoderParam = new EncoderParameter(anEncoder, 75L);
            encoderParams.Param[0] = encoderParam;
            //neue Datei benennen
            string fileNameEnd = file.Substring(file.Count() - 4);
            file = file.Replace(fileNameEnd, "");
            file += "GS";
            file += fileNameEnd;
            bmp.Save(file, aImageCodecInfo, encoderParams);
            return file;
        }

        private ImageCodecInfo GetEncoderInfo(String mimeType)
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

        //http://stackoverflow.com/questions/2746103/what-would-be-a-good-true-black-and-white-colormatrix
        public void ImgBW(string file)
        {
            try
            {
                Image img = Image.FromFile(file);
                Bitmap sourceImage = new Bitmap(img);

                using (Graphics gr = Graphics.FromImage(sourceImage)) // SourceImage is a Bitmap object
                {
                    //graustufen
                    var gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            };

                    var ia = new ImageAttributes();
                    ia.SetColorMatrix(new ColorMatrix(gray_matrix));
                    float d = (float)0.8;
                    //durch Schwellenwert S/W
                    ia.SetThreshold(d);
                    var rc = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                    gr.DrawImage(sourceImage, rc, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, ia);
                    _newFilepath = EncodeAndSave(file, sourceImage);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ein Fehler im Bildformat ist aufgetreten. Scannen Sie das Bild bitte erneut ein.");
            }
        }
    }
}
