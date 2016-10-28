using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace TessMVP2.Model.Interfaces
{
    interface IScanner
    {
        bool SelectDevice();
        string Scan();
    }
}
