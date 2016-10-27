using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces.View
{
    public interface IMyPresenterFormStartCallbacks:IMyPresenter
    {
        void OnButtonClick();
        void OnFujitsuClick();
        void OnWiaClick();
        void OnForm1Shown();
        void OnForm1Closing();

    }
}
