using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.View.Interfaces
{
    public interface IView<TCallbacks>
    {
        void Attach(TCallbacks presenter);
    }
}
