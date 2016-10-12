﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.View
{
    public class DynamicControlViewModel
    {
        public enum ControlTypes
        {
            TextBox,
            GroupBox,
            Button,
            Label,
            FlowLayoutPanel,
            RichtextBox,
            Panel,
            Pbox,
            Cmenu,
            CmenuItem

        }

        public enum Colors
        {
            control,
            window,
            AliceBlue,
            blue,
            orange
        }

        public string LabelText { get; set; }
        public string LabelName { get; set; }
        public string FlowPanelName { get; set; }
        public string GroupboxName { get; set; }
        public string TextBoxName { get; set; }
        public string TextBoxText { get; set; }
        public string RtbName { get; set; }
        public string RtbText { get; set; }
        public string ButtonName { get; set; }
        public string ButtonText { get; set; }
        public string PanelName { get; set; }
        public string PboxName { get; set; }
        public string CmenuName { get; set; }
        public string CmenuItemName { get; set; }
        public ControlTypes ControlType { get; set; }
        public Colors Col { get; set; }
    }
}
