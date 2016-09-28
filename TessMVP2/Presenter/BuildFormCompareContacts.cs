using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using TessMVP2.View;


namespace TessMVP2.Presenter
{
    class BuildFormCompareContacts
    {
        private IMyViewFormCompareContacts _view3;
        public object View3 { get { return _view3; } }
        private TessPresenter _mainPresenter;
        private FormCompareContacts _formObject;
        //private Dictionary<string, string> _oldContactVals;
        private Dictionary<string, string> _newContactVals;
        public FormCompareContacts FormCompareContacts { get { return this._formObject; } private set { this._formObject = value; } }
        private Dictionary<string, string> _oldContactDict;


        public BuildFormCompareContacts(Dictionary<string, string> valsLeftSide, Dictionary<string, string> valsRightSide, TessPresenter presenter)
        {
            var view = new FormCompareContacts();
            this._view3 = view;
            this._mainPresenter = presenter;
            _formObject = (FormCompareContacts)this._view3.Form3;
            this._oldContactDict = valsLeftSide;
            this._newContactVals = valsRightSide;
            SetFormProps();
            AddControls();
            _mainPresenter.ViewForm3 = this._view3;
        }

        private void AddControls()
        {
            var FlowPanelLeft = new FlowLayoutPanel();
            SetDefaultPanelProps(FlowPanelLeft);
            var newLabelLeft = new Label();
            newLabelLeft.Text = "Outlook Kontakt";
            newLabelLeft.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            //newLabelLeft.Dock = DockStyle.Top;
            _formObject.Controls.Add(newLabelLeft);
            var FlowPanelRight = new FlowLayoutPanel();
            SetDefaultPanelProps(FlowPanelRight);
            var newLabelRight = new Label();
            newLabelRight.Text = "Neuer Kontakt";
            newLabelRight.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
           // newLabelLeft.Dock = DockStyle.Top;
            _formObject.Controls.Add(newLabelRight);
            this._formObject.Controls.Add(FlowPanelLeft);
            this._formObject.Controls.Add(FlowPanelRight);

            AddControlsToPanel(FlowPanelRight, _newContactVals);
            AddControlsToPanel(FlowPanelLeft, _oldContactDict);
            var gbbtn = new GroupBox();
            FlowPanelRight.Controls.Add(gbbtn);
            _view3.BtnUpdate = new Button();
            _view3.BtnUpdate.AutoSize = true;
            _view3.BtnUpdate.Text = "Update/übernehme alten Kontakt";
            gbbtn.Controls.Add(_view3.BtnUpdate);
            //CenterControl(_view3.BtnUpdate, gbbtn);
            _view3.BtnUpdate.Dock = DockStyle.Top;

            _view3.BtnCreateNew = new Button();
            _view3.BtnCreateNew.AutoSize = true;
            //_view3.BtnUpdate.Margin = new Padding(Convert.ToInt32(FlowPanelLeft.Width / 4 - _view3.BtnUpdate.Width / 2));
            _view3.BtnCreateNew.Text = "Erstelle neuen Kontakt";
            gbbtn.Controls.Add(_view3.BtnCreateNew);
            //CenterControl(_view3.BtnCreateNew, gbbtn);
            _view3.BtnCreateNew.Dock = DockStyle.Top;

            FlowPanelRight.Location = new Point(FlowPanelLeft.Width, 0);
            CenterControl(newLabelRight, FlowPanelRight);
            CenterControl(newLabelLeft, FlowPanelLeft);
            int x = (int)(FlowPanelLeft.Width / 2 - newLabelLeft.Width / 2);
            FlowPanelLeft.Location = new Point(0,newLabelLeft.Height+10);
            FlowPanelRight.Location = new Point(FlowPanelLeft.Width+10, newLabelRight.Height + 10);
            newLabelLeft.Location = new Point(x, 0);
            newLabelRight.Location = new Point(x+FlowPanelLeft.Width, 0);
        }

        private void AddControlsToPanel(FlowLayoutPanel parentPanel, Dictionary<string, string> aDict)
        {
            foreach (var kvp in aDict)
            {
                if (kvp.Value != null)
                {
                    var gb = new GroupBox();
                    var newLabel = new Label();
                    newLabel.Text = "F3Lbl"+kvp.Key;
                    SetLabelProps(newLabel);
                    newLabel.Text = kvp.Key;
                    var newTb = new TextBox();
                    newTb.Name = "F3Tb" + kvp.Key;
                    SetTextboxProps(newTb);
                    newTb.Text = kvp.Value;
                    gb.Controls.Add(newLabel);
                    gb.Controls.Add(newTb);
                    parentPanel.Controls.Add(gb);
                }

            }

        }

       /* private void GenDict()
        {
            _oldContactDict = new Dictionary<string, string>();
            for (int i = 0; i < _contactParameterNames.Count; i++)
            {
                this._oldContactDict.Add(_contactParameterNames[i], _oldContactVals[i]);
            }
        }*/


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
            
        }

        private void CenterControl(Control cToCenter, Control parentCtr)
        {
            int padLeft = Convert.ToInt32(parentCtr.Width / 2 - cToCenter.Width / 2);
            cToCenter.Margin = new Padding(padLeft, 20, padLeft, 0);
        }
    }
}
