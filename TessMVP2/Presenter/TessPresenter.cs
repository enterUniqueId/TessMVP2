using System;
using TessMVP2.Model;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.View.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.View;
using System.Linq;
using System.Collections.Generic;

namespace TessMVP2.Presenter
{
    class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private IMyViewFormFieldControl _view2;

        public object View1 { get { return _view1; } }
        public object View2 { get { return _view2; } }
        public object Model { get { return _model; } }

        public IMyViewFormFieldControl ViewForm2 { get { return _view2; } set { _view2 = value; } }

        public TessPresenter()
        {
            FormStart view = new FormStart();
            this._view1 = view;
            view.Show();
            TessMainModel model = new TessMainModel(this);

            this._model = model;
            WireView1Events();


        }

        private void WireView1Events()
        {
            this._view1.Form1Btn1.Click += (sender, e) => OnButtonClick();
            this._view1.Form1.FormClosed += (sender, e) => OnForm1Closed();
        }

        private void WireView2Events()
        {
            this._view2.Form2.FormClosed += (sender, e) => OnForm1Closed();
            this._view2.BtnCommit.Click += (sender, e) => OnButtonCommitClick();
        }

        public void OnButtonClick()
        {
            _model.ImgPath = _view1.TextBoxText;
            _model.Start(this);
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

        }

        public void OnStringFinished()
        {
            var bffc = new BuildFormFieldControl(_model.StringResult, this);

            this._view1.Form1.Hide();
            WireView2Events();
        }
    }
}
