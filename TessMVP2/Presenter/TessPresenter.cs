using System;
using TessMVP2.Model;
using System.Windows.Forms;
using TessMVP2.Controller.Interfaces;
using TessMVP2.View.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;



namespace TessMVP2.Presenter
{
    class TessPresenter:IMyPresenter,IMyPresenterViewCallbacks,IMyPresenterModelCallbacks
    {
        private IMyView _view;
        private IMyModel _model;

        public TessPresenter()
        {
            FormStart view = new FormStart(this);
            view.Show();
            TessMainModel model = new TessMainModel(this);
            this._view = view;
            this._model = model;

        }

        public object Model { get { return _model; } }

        public object UI
        {
            get { return _view; }
        }

        public void Initialize()
        {
            _view.Attach(this);
            _model.Attach(this);
        }

        public void OnButtonClick()
        {
            _model.ImgPath = _view.TextBoxText;
            _model.Start(this);
        }

        public void OnTextChange()
        {
            //
        }

        public void OnOcrResultChanged()
        {
            try
            {
                _view.RichTextBoxText = _model.OcrResult;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Fehler",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void OnStringFinished()
        {
            //todo
        }
    }
}
