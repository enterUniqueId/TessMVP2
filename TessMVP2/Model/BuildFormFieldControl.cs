using System;
using System.Collections.Generic;
using TessMVP2.View;
using TessMVP2.View.Interfaces;

namespace TessMVP2.Model
{
    class BuildFormFieldControl
    {
        private Dictionary<string, List<string>> _resDict;
        private List<DynamicControlViewModel> _controlList;
        public List<DynamicControlViewModel> ControlList { get { return this._controlList; } }

        public BuildFormFieldControl(Dictionary<string, List<string>> fieldDict)
        {
            this._resDict = fieldDict;
            BuildList();
        }

        private void BuildList()
        {
            _controlList = new List<DynamicControlViewModel>();
            var dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.FlowLayoutPanel;
            _controlList.Add(dynControl);

            foreach (KeyValuePair<string, List<string>> kvp in this._resDict)
            {
                if (kvp.Key != "EntryID")
                {
                    dynControl = new DynamicControlViewModel();
                    dynControl.ControlType = DynamicControlViewModel.ControlTypes.Panel;
                    dynControl.GroupboxName = "pan" + kvp.Key;
                    _controlList.Add(dynControl);
                    dynControl = new DynamicControlViewModel();
                    dynControl.ControlType = DynamicControlViewModel.ControlTypes.Label;
                    dynControl.LabelName = "F2lbl" + kvp.Key;
                    dynControl.LabelText = kvp.Key;
                    _controlList.Add(dynControl);
                    dynControl = new DynamicControlViewModel();
                    dynControl.ControlType = DynamicControlViewModel.ControlTypes.TextBox; ;
                    dynControl.TextBoxName = "F2tb" + kvp.Key;
                    if (kvp.Value.Count > 0)
                    {
                        dynControl.TextBoxText = kvp.Value[0];
                    }
                    _controlList.Add(dynControl);
                }
            }
            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.RichtextBox;
            dynControl.RtbText = "";
            dynControl.RtbName = "F2rtb1";
            _controlList.Add(dynControl);
            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonText = "Commit";
            dynControl.ButtonName = "F2btnCommit";
            _controlList.Add(dynControl);

        }
    }
}
