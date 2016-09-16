using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMvc1.Controller.Interfaces
{
    public interface IMyPresenterViewCallbacks
    {
        void OnButtonClick();
        void OnTextChange();
    }
}
