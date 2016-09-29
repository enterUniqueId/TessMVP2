using System;
using TessMVP2.View;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.Model;
using System.Linq;
using System.Collections.Generic;

namespace TessMVP2.Presenter
{
    class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks, IMyPresenterOutlookCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private IMyViewFormFieldControl _view2;
        private IMyViewFormCompareContacts _view3;
        private IViewFormYesNoCancel _view4;

        public object View1 { get { return _view1; } }
        public object View2 { get { return _view2; } }
        public object View3 { get { return _view3; } }
        public object View4 { get { return _view4; } }
        public object Model { get { return _model; } }

        private Dictionary<string, string> _inputResults;
        private OutlookWork _outlook;
        private List<FormCompareContacts> _FormcompareContactsList;
        private ProcessUserResults _processUserInput;
        public IMyViewFormFieldControl ViewForm2 { get { return _view2; } set { _view2 = value; } }
        public IMyViewFormCompareContacts ViewForm3 { get { return _view3; } set { _view3 = value; } }
        public IViewFormYesNoCancel ViewForm4 { get { return _view4; } set { _view4 = value; } }

        public TessPresenter()
        {
            FormStart view = new FormStart();
            this._view1 = view;
            view.Show();
            TessMainModel model = new TessMainModel();

            this._model = model;
            WireView1Events();


        }

        private void WireView1Events()
        {
            this._view1.Form1Btn1.Click += (sender, e) => OnButtonClick();
            this._view1.Form1.FormClosed += (sender, e) => OnForm1Closed();
            this._view1.Form1Btn2.Click += (sender, e) => OnButton2Click();
            this._view1.Form1Btn3.Click += (sender, e) => OnButton3Click();
        }

        private void WireView2Events()
        {
            this._view2.Form2.FormClosed += (sender, e) => OnForm1Closed();

            this._view2.BtnCommit.Click += (sender, e) => OnButtonCommitClick();
        }

        private void WireView3Events()
        {
            //this._view3.Form3.FormClosed += (sender, e) => OnForm1Closed();
            this._view3.BtnUpdate.Click += (sender, e) => OnButtonUpdateForm3Click();
            this._view3.BtnCreateNew.Click += (sender, e) => OnButtonCreateNewContactClick();
            this._view3.BtnCancel.Click += (sender, e) => OnButtonCancelCompareClick();
        }

        private void WireView4Events()
        {
            //this._view4.Form4.FormClosing += (sender, e) => OnButtonCancelClick();
            this._view4.BtnYes.Click += (sender, e) => OnButtonYesClick();
            this._view4.BtnNo.Click += (sender, e) => OnButtonNoClick();
            this._view4.BtnCancel.Click += (sender, e) => OnButtonCancelClick();
            this._view4.Form4.Disposed += (sender, e) => OnButtonCancelClick();
        }

        public void OnButtonClick()
        {
            _model.ImgPath = _view1.TextBoxText;
            _model.Start(this);
        }

        public void OnButton2Click()
        {
            var scanner = new Scanner();
            scanner.selectDevice();

        }

        public void OnButton3Click()
        {

            this._outlook = new OutlookWork(this._inputResults, this);
            _outlook.GetContacts();
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

            this._view1.Form1.Hide();
            WireView2Events();
        }

        void IMyPresenterOutlookCallbacks.OnRedundantEntryFound()
        {
            if (this._FormcompareContactsList == null)
                this._FormcompareContactsList = new List<FormCompareContacts>();
            string sr = "Der neue Kontakt stimmte zu _____ % mit Kontakt-Nr. ______ (OL-ID: _____  überein.\nDatensatz anzeigen?";
            var msgbox = new BuildFormYesNoCancel(this, sr);
            this._view4.Form4.Show();
            WireView4Events();
        }

        private void OnButtonUpdateForm3Click()
        {
            _processUserInput.Clist = _view3.Form3.Controls[2];
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs();
            this._inputResults = _processUserInput.ResDict;
            _outlook.UpdateExistingContact(_inputResults);
            //todo
        }

        private void OnButtonCreateNewContactClick()
        {
            //var processInput = new ProcessUserResults(_view3.Form3.Controls[0]);
            //processInput.GetInputs();
            //this._inputResults = new Dictionary<string, string>();
            //this._inputResults = processInput.ResDict;
            //this._outlook = new OutlookWork(this._inputResults, this);   // leer?
            //this._model.OlWork = this._outlook;
            this._outlook.CreateContact();
            _view3.Form3.Close();
            //_FormcompareContactsList[_FormcompareContactsList.Count - 1].Hide();
            //_FormcompareContactsList.RemoveAt(_FormcompareContactsList.Count-1);
        }

        private void OnButtonYesClick()
        {
            var bfcc = new BuildFormCompareContacts(_outlook.OutlookContacts[_outlook.CurrentContact], _outlook.ResultDict, this);
            bfcc.FormCompareContacts.Show();
            WireView3Events();
            _view4.Form4.Close();
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


    }
}
