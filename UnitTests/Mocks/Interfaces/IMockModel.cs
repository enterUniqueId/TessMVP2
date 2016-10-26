using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    interface IMockModel<TCallbacks>
    {
        void Attach(TCallbacks callback);
        void Detach(TCallbacks callback);
    }
}
