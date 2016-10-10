using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;

namespace TessMVP2.View.Interfaces
{
    public interface IView<TCallbacks>
    {
        void Attach(TCallbacks presenter);
        void Detach(TCallbacks presenter);
    }
}
