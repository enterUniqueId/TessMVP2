using System;
using Tesseract;
using System.Windows.Forms;

namespace TessMVP2.Model
{
    public class TessOcr
    {
        private string _imgPath;
        private string _ocrResult;
        public string OcrResult { get { return _ocrResult; } }



        public TessOcr(string imgPath)
        {
            _imgPath = imgPath;
        }

        public void Start()
        {
            doOcr();
        }

        private void doOcr()
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //Application.Exit();
            }
        }
    }
}

