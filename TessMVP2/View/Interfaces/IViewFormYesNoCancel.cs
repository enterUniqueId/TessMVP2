using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TessMVP2.View.Interfaces
{
    interface IViewFormYesNoCancel
    {
        Button BtnYes { get; set; }
        Button BtnNo { get; set; }
        Button BtnCancel { get; set; }
        Form Form4 { get; }
    }
}
