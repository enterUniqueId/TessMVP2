using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces
{
    public interface IMyPresenterModelCallbacks
    {
        void OnOcrResultChanged();
        void OnStringFinished();
    }
}
