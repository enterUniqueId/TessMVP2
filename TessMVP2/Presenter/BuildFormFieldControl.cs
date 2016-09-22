using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Model;
using TessMVP2.View;
using TessMVP2.View.Interfaces;

namespace TessMVP2.Presenter
{
    class BuildFormFieldControl
    {

        private IMyViewFormFieldControl _view2;

        public object View2 { get { return this._view2; } }
        private Dictionary<string, List<string>> _resDict;


        public BuildFormFieldControl(Dictionary<string, List<string>> fieldDict)
        {
            var view = new FormFieldControl();
            this._view2 = view;
            this._resDict = fieldDict;            
        }

        private void AddControls()
        {
            foreach(KeyValuePair<string, List<string>> kvp in this._resDict)
            {
                var newTextbox = new TextBox();
                newTextbox.Name = "F2tb"+kvp.Key;
                newTextbox.Text = kvp.Value[0];
                var newLabel = new Label();
                newLabel.Name = "F2lbl" + kvp.Key;
                newLabel.Text = kvp.Key;
                this._view2.Form2.Controls.Add(newTextbox);
                this._view2.Form2.Controls.Add(newLabel);
            }
        }
    }
}
