using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Model.Interfaces
{
    interface IModel<TCallbacks>
    {
        void Attach(TCallbacks presenter);
    }
}
