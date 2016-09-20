using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Tesseract;
using System.Windows.Forms;
using TessMVP2.Model;
using TessMVP2.Model.Interfaces;
using TessMVP2.Controller.Interfaces;

namespace TessMVP2.Model
{
    public class TessOcr
    {

        private TessMainModel _mainModel;
        private string _ocrResult;
        public string OcrResult { get { return _ocrResult; } }



        public TessOcr(TessMainModel parentModel)
        {
            this._mainModel = parentModel;
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

                    using (Pix img = Pix.LoadFromFile( _mainModel.ImgPath ) )
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
                Application.Exit();
                Environment.Exit(0);
            }
        }
    }
}

