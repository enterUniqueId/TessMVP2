using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;
using TessMVP2.View;
using System.Windows.Forms;

namespace TessMVP2.Presenter
{
    class BuildFormCompare
    {
        private Dictionary<string, string> _conformities;
        private Dictionary<string, string> _newContactDict;
        private Dictionary<string, string> _oldContactDict;
        private List<DynamicControlViewModel> _controlList;
        public List<DynamicControlViewModel> ControlList { get { return this._controlList; } }


        public BuildFormCompare(Dictionary<string, string> newContactvals, Dictionary<string, string> oldContactVals, Dictionary<string, string> conforms)
        {
            this._oldContactDict = oldContactVals;
            this._oldContactDict.Remove("EntryID");
            this._newContactDict = newContactvals;
            this._newContactDict.Remove("EntryID");
            this._conformities = conforms;
            BuildList();
        }

        private void BuildList()
        {
            int i = 0;
            _controlList = new List<DynamicControlViewModel>();
            var dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.FlowLayoutPanel;
            dynControl.FlowPanelName = "F3FlowLayoutPanel1";
            _controlList.Add(dynControl);

            var lblList = new List<DynamicControlViewModel>();
            var tbList = new List<DynamicControlViewModel>();
            foreach (var kvp in _oldContactDict)
            {
                var dynPanControl = new DynamicControlViewModel();
                dynPanControl.ControlType = DynamicControlViewModel.ControlTypes.Panel;
                dynPanControl.PanelName = "F3Pan" + kvp.Key;
                _controlList.Add(dynPanControl);

                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.Label;
                dynControl.LabelName = kvp.Key;
                dynControl.LabelText = kvp.Key + ":" + kvp.Value;
                _controlList.Add(dynControl);

                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.Cmenu;
                string sr = kvp.Key.Replace("-", "");
                dynControl.CmenuName = "F3Cm" + sr;
                _controlList.Add(dynControl);
                SetContextmenuItems(kvp.Key);

                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.Pbox;
                dynControl.PboxName = "F3Pb" + kvp.Key;
                _controlList.Add(dynControl);

                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.TextBox;
                dynControl.TextBoxName = "F3Tb" + kvp.Key;
                if (_newContactDict.ElementAt(i).Value == null)
                    dynControl.TextBoxText = "";
                else
                    dynControl.TextBoxText = _newContactDict.ElementAt(i).Value;
                _controlList.Add(dynControl);
                PaintPanel(dynControl, dynPanControl);
                i++;

            }

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Panel;
            dynControl.PanelName = "F3BtnPanel";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnUpdate";
            dynControl.ButtonText = "Update";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnCreate";
            dynControl.ButtonText = "Anlegen";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnCancel";
            dynControl.ButtonText = "Cancel";
            _controlList.Add(dynControl);

        }

        private void PaintPanel(DynamicControlViewModel textbox, DynamicControlViewModel panel)
        {
            foreach (var kvp in _conformities)
            {
                if (kvp.Value == textbox.TextBoxText)
                    panel.Col = DynamicControlViewModel.Colors.AliceBlue;
                else
                    panel.Col = DynamicControlViewModel.Colors.control;
            }
        }



        private void SetContextmenuItems(string currentItem)
        {
            foreach (var kvp in _oldContactDict)
            {
                if (currentItem.ToLower().Contains("nummer"))
                {
                    if (kvp.Key.ToLower().Contains("nummer"))
                    {
                        var dynControl = new DynamicControlViewModel();
                        dynControl.ControlType = DynamicControlViewModel.ControlTypes.CmenuItem;
                        dynControl.CmenuItemName = kvp.Key;
                        _controlList.Add(dynControl);

                    }
                }
                else
                {
                    if (!kvp.Key.ToLower().Contains("nummer"))
                    {
                        var dynControl = new DynamicControlViewModel();
                        dynControl.ControlType = DynamicControlViewModel.ControlTypes.CmenuItem;
                        dynControl.CmenuItemName = kvp.Key;
                        _controlList.Add(dynControl);
                    }
                }
            }
        }
    }
}
