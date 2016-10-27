using System;
using TessMVP2.View;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.Model;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TessMVP2.Presenter.Interfaces.View;
using WIA;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace TessMVP2.Presenter
{

    partial class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks, IMyPresenterOutlookCallbacks, IMyPresenterFujiCallbacks, IMyPresenterFormStartCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private Scanner _scanner;
        private Device _device;
        private Dictionary<string, string> _inputResults;
        private OutlookWork _outlook;
        private ProcessUserResults _processUserInput;
        private FujiFolderObs _fuji;
        private string _fujiFolder = @"\temp";
        private string _fujiFormat = "*jpg";
        private EditImage _imgEdit;

        public object View1 { get { return _view1; } }
        public object Model { get { return _model; } }
        public bool ScannerIsSet { get; private set; }
        public bool FolderCleaned { get; private set; }
        public bool OutlookSet { get; private set; }
        public bool ScanSucceeded { get; private set; }

        public TessPresenter()
        {
            _view1 = new FormStart(this);
            _view1.FormShow();
            _model = new TessMainModel();
        }

        public TessPresenter(IMyViewFormStart view1, IMyModel model)
        {
            _view1 = view1;
            _model = model;
            ScannerIsSet = false;
            _view1.BtnStatus = false;
        }

        public void OnButtonClick()
        {
            if (ScannerIsSet)
            {
                OnFujitsuClick();
                _view1.BtnStatus = true;
                if (ScannerIsSet)
                    ScanSucceeded = _model.WiaScan() ? true : false;
            }
            else
            {
                OnWiaClick();
            }
        }


        public void OnFujitsuClick()
        {
            _view1.BtnStatus = false;
            this._fuji = new FujiFolderObs(this, _fujiFolder, _fujiFormat);
            _fuji.FSW.SynchronizingObject = _view1 as Form;
            _view1.F1lbl1Text = "Bitte scannen Sie das Objekt";

        }

        public void OnWiaClick()
        {
            _view1.BtnStatus = true;
            ScannerIsSet = _model.CreateScanner() ? true : false;
        }

        public void OnForm1Closing()
        {
            FolderCleaned = _model.CleanupTempfolder() ? true : false;
            Application.Exit();
        }

        public void OnForm1Shown()
        {
            _view1.FormStartText = "Tess.Net-VScanner";
            _view1.F1Btn1Text = "Scan";
        }

        public void OnOcrResultChanged()
        {
            //ersetzt durch fsw
        }

        public void OnStringFinished()
        {
            OutlookSet = _model.CreateOutlook(this) ? true : false;
            _outlook = _model.OlWork;  //
        }

        void IMyPresenterOutlookCallbacks.OnRedundantEntryFound()
        {
            // var allContacts = _outlook.GetAllContacts();
            var allContacts = _outlook.GetAllContacts();
            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookCurrentContact, _outlook.Hits, allContacts);
            _view3 = new FormCompareContacts(bfc.ControlList);
            _processUserInput = new ProcessUserResults();
            _clist = _processUserInput.getControls(_view3.FormCompareClist[0]);
            _view3.FormBezeichnung = "Übereinstimmung gefunden(bestehender Kontakt/neuer Kontakt)";
            _view3.FormShowDialog(_clist, this);
        }

        private void OnButtonCancelCompareClick()
        {
            _view3.FormClose();
        }

        public void OnImgFileCreated(object sender, FileSystemEventArgs e)
        {
            _fuji.Detach(this);
            this._imgEdit = new EditImage();
            _imgEdit.ImgBW(e.FullPath);
            _model.ImgPath = _imgEdit.NewFilepath;
            _model.Start(this);
            _fuji.Attach(this);
        }

        public void OnNoDuplicatesFound()
        {
            //var allContacts = _outlook.GetAllContacts();
            var allContacts = _outlook.GetAllContacts();

            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookCurrentContact, _outlook.Hits, allContacts);
            _view3 = new FormCompareContacts(bfc.ControlList);
            _processUserInput = new ProcessUserResults();
            _clist = _processUserInput.getControls(_view3.FormCompareClist[0]);
            _view3.FormBezeichnung = "Scandaten bearbeiten";
            _view3.FormShowDialog(_clist, this);
        }
    }
}
