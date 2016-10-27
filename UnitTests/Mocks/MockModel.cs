using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Model;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;
using Moq;

namespace UnitTests.Mocks
{
    class MockModel:IMyModel,IFujiModel,IOutlookModel
    {
       
        private Dictionary<string, string> _resfields;
        private string _imgPath;
        private TessOcr _ocr;
        private OutlookWork _olwork;
        private StringProcessor _stringProcessor;
        private Dictionary<string, string> _resFields;
        private List<string> _hits;
        private string _fujiFolder = @"\temp";
        private string _fujiFormat = "*jpg";
        private Scanner _scanner;
        private ProcessUserResults _processUserInput;
        private OutlookWork _outlook;

        //events
        public delegate void OcrChangedHandler(object sender, EventArgs e);
        public event OcrChangedHandler OcrResultChanged;
        public delegate void FinishedStringChangedHandler(object sender, EventArgs e);
        public event FinishedStringChangedHandler FinishedStringChanged;

        public bool DeviceIsSet { get; set; }
        public bool ImageIsSaved { get; set; }
        public OutlookWork OlWork { get { return _outlook; } set { _outlook = value; } }
        public string OcrResult { get; private set; }
        public Dictionary<string, List<string>> StringResult { get; private set; }
        public Dictionary<string, string> ResFields { get { return _resFields; } set { _resFields = value; } }
        public ProcessUserResults ProcessUserInput { get { return _processUserInput; } set { _processUserInput = value; } }


        public MockModel(IMyPresenterModelCallbacks callback)
        {

        }


        public string ImgPath
        {
            get
            {
                return _imgPath;
            }

            set
            {
                _imgPath = value; 
            }
        }

        public List<string> Hits
        {
            get
            {
                return _hits;
            }
        }

        public void CreateScannerObject()
        {
            DeviceIsSet = true;
        }

        public void Scan()
        {
            ImageIsSaved = true;
            //fsw-event
        }

        public void Attach(IMyPresenterModelCallbacks callback)
        {
            FinishedStringChanged += (sender, e) => callback.OnStringFinished();
        }
        public void Detach(IMyPresenterModelCallbacks callback)
        {
            OcrResultChanged -= (sender, e) => callback.OnOcrResultChanged();
            FinishedStringChanged -= (sender, e) => callback.OnStringFinished();
        }

        public void Start(IMyPresenterModelCallbacks callback)
        {
            Attach(callback);
            _ocr = new Mock(new TessOcr(this));
                TessOcr(this);

            OcrResultChanged += (sender, e) => callback.OnOcrResultChanged();
            _ocr.Start();
            OcrResult = _ocr.OcrResult;
            if (OcrResultChanged != null)
                OcrResultChanged(this, EventArgs.Empty);

            _stringProcessor = new StringProcessor(this);
            _stringProcessor.Start();
            _resFields = _stringProcessor.TransformResDict(_stringProcessor.ResDict);
            if (FinishedStringChanged != null)
                FinishedStringChanged(this, EventArgs.Empty);
            FinishedStringChanged = null;
        }

        public void Attach(IMyPresenterFujiCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterFujiCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void GetContacts()
        {
            throw new NotImplementedException();
        }

        public void Attach(IMyPresenterOutlookCallbacks callback)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterOutlookCallbacks presenter)
        {
            throw new NotImplementedException();
        }

        public bool CreateScanner()
        {
            throw new NotImplementedException();
        }

        public bool CleanupTempfolder()
        {
            throw new NotImplementedException();
        }

        public bool CreateOutlook(IMyPresenterOutlookCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public bool WiaScan()
        {
            throw new NotImplementedException();
        }

        public List<DynamicControlViewModel> BuildCompareForm()
        {
            throw new NotImplementedException();
        }

        public List<Control> GetControlInput(Control cont)
        {
            throw new NotImplementedException();
        }

        public string EditImg(string pathToImg)
        {
            throw new NotImplementedException();
        }

        public FujiFolderObs CreateFSW(IMyPresenterFujiCallbacks callbacks)
        {
            throw new NotImplementedException();
        }
    }
}
