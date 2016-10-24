using System.Collections.Generic;
using System.Linq;
using TessMVP2.View;
using Microsoft.Office.Interop.Outlook;

namespace TessMVP2.Presenter
{
    class BuildFormCompare
    {
        //private List<object> _allContacts;
        private Items _allContacts;
        private Dictionary<string, string> _conformities;
        private Dictionary<string, string> _newContactDict;
        private Dictionary<string, string> _oldContactDict;
        private List<DynamicControlViewModel> _controlList;
        public List<DynamicControlViewModel> ControlList { get { return this._controlList; } }



        //public BuildFormCompare(Dictionary<string, string> newContactvals, Dictionary<string, string> oldContactVals, Dictionary<string, string> conforms,
        //                           List<object> allContacts)
        //{
        //    this._oldContactDict = oldContactVals;
        //    this._oldContactDict.Remove("EntryID");
        //    this._newContactDict = newContactvals;
        //    this._newContactDict.Remove("EntryID");
        //    this._conformities = conforms;
        //    this._allContacts = allContacts;
        //    BuildList();
        //}

        public BuildFormCompare(Dictionary<string, string> newContactvals, Dictionary<string, string> oldContactVals, Dictionary<string, string> conforms,
                         Items allContacts)
        {
            this._oldContactDict = oldContactVals;
            this._oldContactDict.Remove("EntryID");
            this._newContactDict = newContactvals;
            this._newContactDict.Remove("EntryID");
            this._conformities = conforms;
            this._allContacts = allContacts;
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
                dynControl.LabelName = "F3lbl"+kvp.Key;
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
                i++;

            }

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Panel;
            dynControl.PanelName = "F3BtnPanel";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnUpdate";
            dynControl.ButtonText = "Kontakt aktualisieren";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnCreate";
            dynControl.ButtonText = "Kontakt Anlegen";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.Button;
            dynControl.ButtonName = "F3BtnCancel";
            dynControl.ButtonText = "Abbrechen";
            _controlList.Add(dynControl);

            dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.ComboBox;
            dynControl.ComboBoxName = "F3CbContacts";
            dynControl.ComboBoxItems = _allContacts;
            _controlList.Add(dynControl);
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
