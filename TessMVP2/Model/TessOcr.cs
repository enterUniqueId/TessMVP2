using System;
using Tesseract;
using System.Windows.Forms;
using TessMVP2.Model.Interfaces;

namespace TessMVP2.Model
{
    public class TessOcr:ITessOcr
    {
        private string _imgPath;
        private string _ocrResult;
        public string OcrResult { get { return _ocrResult; } }



        public TessOcr(string imgPath)
        {
            _imgPath = imgPath;
        }

        public string Start()
        {
            return doOcr();
        }

        private string doOcr()
        {

            try
            {
                using (TesseractEngine engine = new TesseractEngine(@"./tessdata", "deu", EngineMode.Default))
                {

                    using (Pix img = Pix.LoadFromFile( _imgPath ) )
                    {

                        using (Page page = engine.Process(img))
                        {
                            _ocrResult = page.GetText();
                            return _ocrResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
    }
}

