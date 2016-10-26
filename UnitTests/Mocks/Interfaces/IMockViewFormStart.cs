using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;

namespace UnitTests.Mocks.Interfaces
{
    interface IMockViewFormStart:IMyViewFormStart
    {
        bool FormShown { get;}
    }
}
