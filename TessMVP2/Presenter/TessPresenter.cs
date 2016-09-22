using System;
using TessMVP2.Model;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.View.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.View;



namespace TessMVP2.Presenter
{
    class TessPresenter : IMyPresenter, IMyPresenterModelCallbacks
    {
        private IMyViewFormStart _view1;
        private IMyModel _model;
        private IMyViewFormFieldControl _view2;

        public object View1 { get { return _view1;} }
        public object View2 { get { return _view2; } }
        public object Model { get { return _model; } }

        public TessPresenter()
        {
            FormStart view = new FormStart();
            this._view1 = view;
            view.Show();
            TessMainModel model = new TessMainModel(this);
            
            this._model = model;
            wireView1Events();


        }

        private void wireView1Events()
        {
            this._view1.Form1Btn1.Click += (sender, e) => OnButtonClick();
            this._view1.Form1.FormClosed += (sender, e) => OnForm1Closed();
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

        public void OnStringFinished()
        {
            //FormFieldControl view = new FormFieldControl();
            //this._view2 = view;
            var bffc = new BuildFormFieldControl(_model.StringResult);
            
            this._view1.Form1.Hide();

            this._view2.Form2.Show();
        }
    }
}
