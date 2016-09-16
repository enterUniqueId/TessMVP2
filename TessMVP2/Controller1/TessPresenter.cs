using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMvc1.Model;
using System.Windows.Forms;
using TessMvc1.Controller.Interfaces;
using TessMvc1.View.Interfaces;
using TessMvc1.View;
using TessMvc1.Model.Interfaces;
using TessMvc1.Controller.Interfaces;



namespace TessMvc1.Controller
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
