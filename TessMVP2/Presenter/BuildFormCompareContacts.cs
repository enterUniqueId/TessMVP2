using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;


namespace TessMVP2.Presenter
{
    class BuildFormCompareContacts
    {
        private IMyViewFormCompareContacts _view3;
        public object View3 { get { return _view3; } }
        private TessPresenter _mainPresenter;
        private Form _formObject;
        private List<string> _oldContactVals;
        private Dictionary<string, string> _newContactVals;
        public Form FormCompareContacts { get { return this._formObject; } private set { this._formObject = value; } }
        //helperList
        private List<string> _contactParameterNames = new List<string> {
                                                                    "FullName","BusinessTelephoneNumber","Business2TelephoneNumber","MobileTelephoneNumber","BusinessFaxNumber",
                                                                    "BusinessAddressStreet","BusinessAddressPostalCode","EntryID","BusinessAddressCity","BusinessAddressPostOfficeBox","JobTitle",
                                                                    "BusinessHomePage","CompanyName","Email1Address","Email2Address","Email3Address" };
        private Dictionary<string, string> _oldContactDict;


        public BuildFormCompareContacts(List<string> valsLeftSide, Dictionary<string, string> valsRightSide, TessPresenter presenter)
        {
            this._oldContactVals = valsLeftSide;
            this._newContactVals = valsRightSide;
            this
            SetFormProps();
            AddControls();
        }

        private void AddControls()
        {
            var FlowPanelLeft = new FlowLayoutPanel();
            SetDefaultPanelProps(FlowPanelLeft);
            var FlowPanelRight = new FlowLayoutPanel();
            SetDefaultPanelProps(FlowPanelRight);
            this._formObject.Controls.Add(FlowPanelLeft);
            this._formObject.Controls.Add(FlowPanelRight);
            AddControlsToPanel(FlowPanelRight, _newContactVals);
            GenDict();
            AddControlsToPanel(FlowPanelLeft, _oldContactDict);
            _view3.BtnCommit = new Button();
            FlowPanelRight.Controls.Add(_view3.BtnCommit);
            _view3.Form3.Controls.Add(FlowPanelLeft);
            _view3.Form3.Controls.Add(FlowPanelRight);

        }

        private void AddControlsToPanel(FlowLayoutPanel parentPanel, Dictionary<string, string> aDict)
        {
            foreach (var kvp in aDict)
            {
                if (kvp.Value != null)
                {
                    var gb = new GroupBox();
                    var labelRight = new Label();
                    SetLabelProps(labelRight);
                    labelRight.Text = kvp.Key;
                    var tbRight = new TextBox();
                    tbRight.Text = kvp.Value;
                    gb.Controls.Add(labelRight);
                    gb.Controls.Add(tbRight);
                    parentPanel.Controls.Add(gb);
                }

            }

        }

        private void GenDict()
        {
            _oldContactDict = new Dictionary<string, string>();
            for (int i = 0; i < _contactParameterNames.Count; i++)
            {
                this._oldContactDict.Add(_contactParameterNames[i], _oldContactVals[i]);
            }
        }


        private void SetFormProps()
        {
            this._formObject.AutoSize = true;
            this._formObject.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._formObject.Text = "Zuordnungen";
        }

        private void SetTextboxProps(TextBox tb)
        {
            tb.Width = 300;
            //tb.Height = 70;
            tb.Margin = new Padding(5);
            tb.Dock = DockStyle.Top;
        }

        private void SetLabelProps(Label lbl)
        {
            lbl.Width = 200;
            //lbl.Height = 50;
            lbl.Margin = new Padding(5);
            lbl.Dock = DockStyle.Top;
        }
        private void SetDefaultPanelProps(FlowLayoutPanel pan)
        {
            pan.AutoSize = true;
            pan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pan.FlowDirection = FlowDirection.TopDown;
            pan.Margin = new Padding(10);
            pan.MaximumSize = new System.Drawing.Size(1333, 750);
            pan.Dock = DockStyle.Top;
        }
    }
}
