using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces.View
{
    public interface IMyPresenterFormStartCallbacks
    {
        void OnButtonClick();
        void OnForm1Closed();
        void OnButton2Click();
        void OnButton3Click();
        void OnFujitsuClick();
        void OnWiaClick();
        void OnForm1Closing();
        void OnForm1Shown();
    }
}
