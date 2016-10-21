using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.Model;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.Presenter
{
    partial class TessPresenter:IPresenterFormFieldControlCallbacks
    {
        private IViewModelFormFieldControl _view2;
        public object View2 { get { return _view2; } }
        


        public void Initialize()
        {

        }


        public void OnForm2Closed()
        {
            _view1.FormShow();
            _view2.FormDispose();
            _view2 = null;
        }

        public void OnBtnCommitClick()
        {
            var processInput = new ProcessUserResults();
            //this._processUserInput = processInput;
            //processInput.GetInputs();
            //this._inputResults = new Dictionary<string, string>();
            //this._inputResults = processInput.ResDict;
            this._outlook = new OutlookWork(this._inputResults, this);
            this._model.OlWork = this._outlook;
            _outlook.GetContacts();
            _view2.FormClose();
            
        }


    }
}
