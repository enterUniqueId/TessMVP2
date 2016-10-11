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

namespace TessMVP2.Presenter
{
    partial class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks, IMyPresenterOutlookCallbacks, IMyPresenterFujiCallbacks, IMyPresenterFormStartCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        // private IMyViewFormFieldControl _view2;


        public object View1 { get { return _view1; } }
        // public object View2 { get { return _view2; } }
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

        [Obsolete]
        private void AttachView1Events()
        {
            this._view1.Form1Btn1.Click += (sender, e) => OnButtonClick();
            this._view1.Form1.FormClosed += (sender, e) => OnForm1Closed();
            this._view1.Form1Btn2.Click += (sender, e) => OnButton2Click();
            this._view1.Form1Btn3.Click += (sender, e) => OnButton3Click();
            this._view1.TsiFuji.Click += (sender, e) => OnFujitsuClick();
            this._view1.TsiWia.Click += (sender, e) => OnWiaClick();
            this._view1.Form1.FormClosing += (sender, e) => OnForm1Closing();
        }


        public void OnButtonClick()
        {
            //_model.ImgPath = _view1.TextBoxText;
            //_model.Start(this);
        }

        public void OnButton2Click()
        {
            var scanner = new Scanner();
            scanner.selectDevice();

        }

        public void OnButton3Click()
        {


        }

        public void OnFujitsuClick()
        {
            var parent = _view1.TsiFuji.OwnerItem as ToolStripMenuItem;
            ((ToolStripDropDownMenu)parent.DropDown).ShowCheckMargin = true;
            ((ToolStripDropDownMenu)parent.DropDown).ShowImageMargin = true;
            this._fuji = new FujiFolderObs(this, _fujiFolder, _fujiFormat);
            _fuji.FSW.SynchronizingObject = _view1.Form1;

        }

        public void OnWiaClick()
        {

        }

        public void OnForm1Closing()
        {
            CleanUpTempfolder.Cleanup();
        }

        public void OnForm1Shown()
        {

        }

        public void OnForm1Closed()
        {
            Application.Exit();
            //Environment.Exit(0);
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
            var form2 = new FormFieldControl(bffc.ControlList, this);
            _view2 = form2;
            _view2.DynamicControls = bffc.ControlList;
            this._view1.Form1.Hide();
            this._view2.FormShow();
        }
        void IMyPresenterOutlookCallbacks.OnRedundantEntryFound()
        {
            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookContacts[_outlook.CurrentContact], this, _outlook.Hits);
            this._clist = _processUserInput.getControls(_view3.Form3.Controls[0]);
            AttachView3Events();
            _view3.Form3.ShowDialog();

            /*  if (this._FormcompareContactsList == null)
                  this._FormcompareContactsList = new List<FormCompareContacts>();
              string sr = "Der neue Kontakt stimmte zu _____ % mit Kontakt-Nr. ______ (OL-ID: _____  überein.\nDatensatz anzeigen?";
              var msgbox = new BuildFormYesNoCancel(this, sr);
              AttachView4Events();
              this._view4.Form4.ShowDialog();*/
        }


        private void OnButtonCancelCompareClick()
        {
            _view3.Form3.Close();
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
