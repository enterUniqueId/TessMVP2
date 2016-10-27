using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Tesseract;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;
using System.Windows.Forms;
using System.IO;
using WIA;
using TessMVP2.View;

namespace TessMVP2.Model
{
    public class TessMainModel : IMyModel
    {
        private string _imgPath;
        private string _fujiFolder = @"\temp";
        private string _fujiFormat = "*jpg";
        private TessOcr _ocr;
        private StringProcessor _stringProcessor;
        private Scanner _scanner;
        private ProcessUserResults _processUserInput;
        private Dictionary<string, string> _resFields;
        private OutlookWork _outlook;
        public delegate void OcrChangedHandler(object sender, EventArgs e);
        public event OcrChangedHandler OcrResultChanged;
        public delegate void FinishedStringChangedHandler(object sender, EventArgs e);
        public event FinishedStringChangedHandler FinishedStringChanged;
        public OutlookWork OlWork { get { return _outlook; } set { _outlook = value; } }
        public string OcrResult { get; private set; }
        public Dictionary<string, List<string>> StringResult { get; private set; }
        public Dictionary<string, string> ResFields { get { return _resFields; } set { _resFields = value; } }
        public ProcessUserResults ProcessUserInput { get { return _processUserInput; } set { _processUserInput = value; } }

        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }

        }

        public void Attach(IMyPresenterModelCallbacks callback)
        {
            //this.RedundandEntryFound += (sender, e) => callback.OnRedundandEntryFound();
            FinishedStringChanged += (sender, e) => callback.OnStringFinished();

        }

        public void Detach(IMyPresenterModelCallbacks callback)
        {
            FinishedStringChanged -= (sender, e) => callback.OnStringFinished();
        }

        public void Start(IMyPresenterModelCallbacks callback)
        {
            Attach(callback);
            _ocr = new TessOcr(this);
            OcrResultChanged += (sender, e) => callback.OnOcrResultChanged();
            _ocr.Start();
            OcrResult= _ocr.OcrResult;
            if (OcrResultChanged != null)
                OcrResultChanged(this, EventArgs.Empty);

            _stringProcessor = new StringProcessor(this);
            _stringProcessor.Start();
            _resFields = _stringProcessor.TransformResDict(_stringProcessor.ResDict);
            if (FinishedStringChanged != null)
                FinishedStringChanged(this, EventArgs.Empty);
            FinishedStringChanged = null;
        }

        public bool CreateScanner()
        {
            _scanner = new Scanner();
            try
            {
                _scanner.selectDevice();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WiaScan()
        {
            try
            {
                _scanner.Scan();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CleanupTempfolder()
        {
            try
            {
                CleanUpTempfolder.Cleanup();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateOutlook(IMyPresenterOutlookCallbacks callbacks)
        {
            try
            {
                _outlook = new OutlookWork(ResFields, callbacks);
                _outlook.GetContacts();
                return true;
            }
            catch
            {

                return false;
            }

        }

        public List<DynamicControlViewModel> BuildCompareForm()
        {
            var allContacts = _outlook.GetAllContacts();
            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookCurrentContact, _outlook.Hits, allContacts);
            return bfc.BuildList();
        }

        public List<Control> GetControlInput(Control cont)
        {
            _processUserInput = new ProcessUserResults();
            return _processUserInput.getControls(cont);
        }

        public string EditImg(string pathToImg)
        {
            var imgEdit = new EditImage();
            try
            {
                imgEdit.ImgBW(pathToImg);
                _imgPath = imgEdit.NewFilepath;
                return imgEdit.NewFilepath;
            }
            catch
            {
                return null;
            }
        }

        public FujiFolderObs CreateFSW(IMyPresenterFujiCallbacks callbacks)
        {
            return new FujiFolderObs(callbacks, _fujiFolder, _fujiFormat);

        }
    }


}
