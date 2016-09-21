using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Model;
using TessMVP2.View;

namespace TessMVP2.Presenter
{
    class BuildFormFieldControl
    {
        private Dictionary<string, List<string>> _resDict;
        private FormFieldControl _form;
        public FormFieldControl Form { get; private set; }

        public BuildFormFieldControl(Dictionary<string, List<string>> fieldDict,Form form)
        {
            this.Form = (FormFieldControl)form;
            this._resDict = fieldDict;            
        }
    }
}
