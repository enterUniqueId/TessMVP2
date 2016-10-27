using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Model;

namespace TessMVP2.View.Interfaces
{
    public interface IDynamicControlViewModel
    {
        IEnumerable<DynamicControlViewModel>DynamicControls { get; set; }
    }
}
