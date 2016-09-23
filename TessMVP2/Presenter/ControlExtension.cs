using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace TessMVP2.Presenter
{
    public static class ControlExtension
    {       
        public static IEnumerable<Control> All(this ControlCollection controls)
        {

            foreach (Control control in controls)
            {
                foreach (Control grandChild in control.Controls.All())
                    yield return grandChild;

                yield return control;
            }
        }
    }
}
