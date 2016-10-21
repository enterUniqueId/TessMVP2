using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Tesseract;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;

namespace TessMVP2.Model
{
    public class TessMainModel : IMyModel
    {

        public delegate void OcrChangedHandler(object sender, EventArgs e);
        public event OcrChangedHandler OcrResultChanged;
        public delegate void FinishedStringChangedHandler(object sender, EventArgs e);
        public event FinishedStringChangedHandler FinishedStringChanged;
        private Dictionary<string, string> _resFields;

        public OutlookWork OlWork { get; set; }
        private string _imgPath;
        private TessOcr _ocr;
        private StringProcessor _stringProcessor;
        public string OcrResult { get; private set; }
        public Dictionary<string, List<string>> StringResult { get; private set; }
        public Dictionary<string, string> ResFields { get { return _resFields; } set { _resFields = value; } }

        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }

        }

        public void Attach(IMyPresenterModelCallbacks callback)
        {
            //this.RedundandEntryFound += (sender, e) => callback.OnRedundandEntryFound();
            this.FinishedStringChanged += (sender, e) => callback.OnStringFinished();

        }

        public void Detach(IMyPresenterModelCallbacks callback)
        {
            this.FinishedStringChanged -= (sender, e) => callback.OnStringFinished();
        }


        public void Start(IMyPresenterModelCallbacks callback)
        {
            Attach(callback);
            this._ocr = new TessOcr(this);
            this.OcrResultChanged += (sender, e) => callback.OnOcrResultChanged();
            this._ocr.Start();
            this.OcrResult = this._ocr.OcrResult;
            if (this.OcrResultChanged != null)
                this.OcrResultChanged(this, EventArgs.Empty);

            _stringProcessor = new StringProcessor(this);
            _stringProcessor.Start();
            _resFields = _stringProcessor.TransformResDict(_stringProcessor.ResDict);
            //this.StringResult = _stringProcessor.ResDict;
            if (this.FinishedStringChanged != null)
                this.FinishedStringChanged(this, EventArgs.Empty);
            //Detach(callback);
            FinishedStringChanged = null;


        }


    }


}
