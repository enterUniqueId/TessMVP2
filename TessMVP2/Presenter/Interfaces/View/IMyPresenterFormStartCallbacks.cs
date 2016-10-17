using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces.View
{
    public interface IMyPresenterFormStartCallbacks:IPresenter
    {
        void OnButtonClick();
        void OnFujitsuClick();
        void OnWiaClick();
        void OnForm1Shown();
        void OnForm1Closing();

    }
}
