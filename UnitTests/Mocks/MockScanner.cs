using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Model;
using TessMVP2.Model.Interfaces;
using WIA;

namespace UnitTests.Mocks
{
    class MockScanner:IScanner
    {
        public bool SelectDevice()
        {

            return true;
        }

        public string Scan()
        {
            return "A valid Filepath";
        }
    }
}
