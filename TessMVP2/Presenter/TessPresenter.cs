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

namespace TessMVP2.Presenter
{
    partial class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks, IMyPresenterOutlookCallbacks, IMyPresenterFujiCallbacks, IMyPresenterFormStartCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private Scanner _scanner;
        private Device _device;


        public object View1 { get { return _view1; } }
        public object Model { get { return _model; } }

        private Dictionary<string, string> _inputResults;
        private OutlookWork _outlook;
        private ProcessUserResults _processUserInput;
        private FujiFolderObs _fuji;
        private string _fujiFolder;
        private string _fujiFormat;
        private EditImage _imgEdit;


        public TessPresenter()
        {
            FormStart view = new FormStart(this);
            this._view1 = view;
            view.Show();
            TessMainModel model = new TessMainModel();
            this._model = model;
            this._fujiFolder = @"/temp";
            this._fujiFormat = "*jpg";
        }


        public void OnButtonClick()
        {
            if (_device != null)
            {
                OnFujitsuClick();
                _view1.BtnStatus = true;
                _scanner.Scan();
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

        }

        public void OnWiaClick()
        {
            _view1.BtnStatus = true;
            _scanner = new Scanner();
            _scanner.selectDevice();
            this._device = _scanner.Device;
        }

        public void OnForm1Closing()
        {
            CleanUpTempfolder.Cleanup();
            Application.Exit();
        }

        public void OnForm1Shown()
        {

        }


        public void OnOcrResultChanged()
        {
            try
            {
                _view1.RichTextBoxText = _model.OcrResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnStringFinished()
        {
            var bffc = new BuildFormFieldControl(_model.StringResult);
            _view2 = new FormFieldControl(bffc.ControlList, this);
            _view2.DynamicControls = bffc.ControlList;
            this._view1.FormHide();
            this._view2.FormShow();
        }

        void IMyPresenterOutlookCallbacks.OnRedundantEntryFound()
        {
            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookContacts[_outlook.CurrentContact], _outlook.Hits);
            _view3 = new FormCompareContacts(bfc.ControlList);
            this._clist = _processUserInput.getControls(_view3.FormCompareClist[0]);
            _view3.FormShowDialog(_clist,this);
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
    }
}
