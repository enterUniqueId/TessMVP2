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

        public object View1 { get { return _view1; } }
        public object View2 { get { return _view2; } }
        public object View3 { get { return _view3; } }
        public object Model { get { return _model; } }

        private Dictionary<string, string> _inputResults;
        private OutlookWork _outlook;
        public IMyViewFormFieldControl ViewForm2 { get { return _view2; } set { _view2 = value; } }
        public IMyViewFormCompareContacts ViewForm3 { get { return _view3; } set { _view3 = value; } }

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
            this._view3.Form3.FormClosed += (sender, e) => OnForm1Closed();
            this._view3.BtnUpdate.Click += (sender, e) => OnButtonUpdateForm3Click();
            this._view3.BtnCreateNew.Click += (sender, e) => OnButtonCreateNewForm3Click();
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

            this._outlook = new OutlookWork(this._inputResults,this);
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
            processInput.GetInputs();
            this._inputResults = new Dictionary<string, string>();
            this._inputResults = processInput.ResDict;
            this._outlook = new OutlookWork(this._inputResults,this);
            this._model.OlWork = this._outlook;
            _outlook.GetContacts();
            //outlook.CreateContactExample();
        }

        public void OnStringFinished()
        {
            var bffc = new BuildFormFieldControl(_model.StringResult, this);

            this._view1.Form1.Hide();
            WireView2Events();
        }

        void IMyPresenterOutlookCallbacks.OnRedundandEntryFound()
        {

            var bfcc = new BuildFormCompareContacts(_outlook.OutlookContacts[_outlook.CurrentContact], _outlook.ResultDict,this);
            bfcc.FormCompareContacts.Show();
            WireView3Events();
        }

        private void OnButtonUpdateForm3Click()
        {
            MessageBox.Show("dsfsdf");
        }

        private void OnButtonCreateNewForm3Click()
        {
            MessageBox.Show("dsfsdf");
        }
    }
}
