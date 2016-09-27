using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View;
using TessMVP2.View;
using TessMVP2.View.Interfaces;
using System.Windows;

namespace TessMVP2.Presenter
{
    class BuildFormFieldControl
    {

        private IMyViewFormFieldControl _view2;

        public object View2 { get { return _view2; } }
        //public Button btnCommit { get; private set; }

        private Dictionary<string, List<string>> _resDict;
        private TessPresenter _mainPresenter;
        //public FormFieldControl View2 { get; private set; }

        public BuildFormFieldControl(Dictionary<string, List<string>> fieldDict, TessPresenter mainPres)
        {
            var view = new FormFieldControl();
            this._view2 = view;
            this._resDict = fieldDict;
            this._mainPresenter = mainPres;
            SetFormProps();
            AddControls();
        }

        private void AddControls()
        {
            var newFlowPanel = new FlowLayoutPanel();
            SetDefaultPanelProps(newFlowPanel);
            _view2.Form2.Controls.Add(newFlowPanel);

            foreach (KeyValuePair<string, List<string>> kvp in this._resDict)
            {
                var gb = new GroupBox();
                gb.Name = "gb" + kvp.Key;
                //SetDefaultGBoxProps(gb);
                var newLabel = new Label();
                SetLabelProps(newLabel);
                newLabel.Name = "F2lbl" + kvp.Key;
                newLabel.Text = kvp.Key;
                var newTextbox = new TextBox();
                SetTextboxProps(newTextbox);
                newTextbox.Name = "F2tb" + kvp.Key;
                if (kvp.Value.Count > 0)
                {
                    newTextbox.Text = kvp.Value[0];

                }
                newFlowPanel.Controls.Add(gb);
                gb.Controls.Add(newTextbox);
                gb.Controls.Add(newLabel);
                

            }
            var newRichTextBox = new RichTextBox();
            SetDefaultRTBoxProps(newRichTextBox, newFlowPanel);
            _view2.BtnCommit = new Button();
            SetDefaultButtonProps(_view2.BtnCommit, newRichTextBox);
            newFlowPanel.Controls.Add(newRichTextBox);
            newFlowPanel.Controls.Add(_view2.BtnCommit);
            this._view2.Form2.Show();
            _mainPresenter.ViewForm2 = this._view2;
        }

        private void SetFormProps()
        {
            _view2.Form2.AutoSize = true;
            _view2.Form2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _view2.Form2.Text = "Zuordnungen";

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

        private void SetDefaultRTBoxProps(RichTextBox rtb, FlowLayoutPanel parent)
        {
            rtb.Height = Convert.ToInt32(parent.Height * 0.5);
            rtb.Width = Convert.ToInt32(parent.Width * 0.75);
        }

        private void SetDefaultGBoxProps(GroupBox gb)
        {
            gb.AutoSize = true;
            gb.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void SetDefaultButtonProps(Button btn, RichTextBox rtb)
        {
            int padLeft = Convert.ToInt32(rtb.Width / 2 - btn.Width / 2);
            btn.Margin = new Padding(padLeft, 20, padLeft, 0);
            btn.Text = "Commit";
            btn.Name = "F2btnCommit";

        }
    }
}
