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

namespace TessMVP2.Presenter
{
    partial class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks, IMyPresenterOutlookCallbacks, IMyPresenterFujiCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private IMyViewFormFieldControl _view2;
        private IViewFormYesNoCancel _view4;

        public object View1 { get { return _view1; } }
        public object View2 { get { return _view2; } }
        
        public object View4 { get { return _view4; } }
        public object Model { get { return _model; } }

        private Dictionary<string, string> _inputResults;
        private OutlookWork _outlook;
        private ProcessUserResults _processUserInput;
        public IMyViewFormFieldControl ViewForm2 { get { return _view2; } set { _view2 = value; } }
        public IViewFormYesNoCancel ViewForm4 { get { return _view4; } set { _view4 = value; } }

        public TessPresenter()
        {
            FormStart view = new FormStart();
            this._view1 = view;
            view.Show();
            TessMainModel model = new TessMainModel();
            this._model = model;
            AttachView1Events();
        }

        private void AttachView1Events()
        {
            this._view1.Form1Btn1.Click += (sender, e) => OnButtonClick();
            this._view1.Form1.FormClosed += (sender, e) => OnForm1Closed();
            this._view1.Form1Btn2.Click += (sender, e) => OnButton2Click();
            this._view1.Form1Btn3.Click += (sender, e) => OnButton3Click();
            this._view1.TsiFuji.Click += (sender, e) => OnFujitsuClick();
            this._view1.TsiWia.Click += (sender, e) => OnWiaClick();
        }

        private void AttachView2Events()
        {
            this._view2.Form2.FormClosing += (sender, e) => OnForm2Closed();
            this._view2.BtnCommit.Click += (sender, e) => OnButtonCommitClick();
        }

        private void AttachView4Events()
        {
            //this._view4.Form4.FormClosing += (sender, e) => OnButtonCancelClick();
            this._view4.BtnYes.Click += (sender, e) => OnButtonYesClick();
            this._view4.BtnNo.Click += (sender, e) => OnButtonNoClick();
            this._view4.BtnCancel.Click += (sender, e) => OnButtonCancelClick();
            //this._view4.Form4.Disposed += (sender, e) => OnButtonCancelClick();
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
            //

        }

        private void OnFujitsuClick()
        {
            var parent = _view1.TsiFuji.OwnerItem as ToolStripMenuItem;
            ((ToolStripDropDownMenu)parent.DropDown).ShowCheckMargin = true;
            ((ToolStripDropDownMenu)parent.DropDown).ShowImageMargin = true;
            FujiFolderObs fuji = new FujiFolderObs(this);
            fuji.FSW.SynchronizingObject = _view1.Form1;

        }

        private void OnWiaClick()
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
        private void OnForm1Closed()
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void OnForm2Closed()
        {
            _view1.Form1.Show();
            _view2.Form2.Dispose();
            _view2 = null;
        }

        private void OnButtonCommitClick()
        {
            var processInput = new ProcessUserResults(_view2.Form2.Controls[0]);
            this._processUserInput = processInput;
            processInput.GetInputs();
            this._inputResults = new Dictionary<string, string>();
            this._inputResults = processInput.ResDict;
            this._outlook = new OutlookWork(this._inputResults, this);
            this._model.OlWork = this._outlook;
            _outlook.GetContacts();
        }

        public void OnStringFinished()
        {
            var bffc = new BuildFormFieldControl(_model.StringResult, this);

            //this._view1.Form1.Hide();
            this._view2.Form2.Show();
            AttachView2Events();
        }

        void IMyPresenterOutlookCallbacks.OnRedundantEntryFound()
        {
            var bfc = new BuildFormCompare(_outlook.ResultDict, _outlook.OutlookContacts[_outlook.CurrentContact], this);
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

        private void OnButtonYesClick()
        {
            //man könnte auch NUR den oldcontact übergeben

        }

        private void OnButtonNoClick()
        {
            this._outlook.CreateContact();
            _view4.Form4.Close();
        }

        private void OnButtonCancelClick()
        {
            _view4.Form4.Close();
        }

        private void OnButtonCancelCompareClick()
        {
            _view3.Form3.Close();
        }

        public void OnImgFileCreated(object sender, FileSystemEventArgs e)
        {
            _model.ImgPath = e.FullPath;
            _model.Start(this);
            EditImage.BildSW(e.FullPath);
            //MessageBox.Show(e.Name+" "+e.FullPath);
        }
    }
}
