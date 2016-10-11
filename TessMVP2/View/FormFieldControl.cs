using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces.View;
using TessMVP2.View.Interfaces;

namespace TessMVP2.View
{
    public partial class FormFieldControl : Form, IViewModelFormFieldControl
    {
        private Button _btnCommit;
        private IEnumerable<DynamicControlViewModel> _dynamicControls;
        private Control.ControlCollection _formFieldClist;
        public Control.ControlCollection FormFieldClist { get { return this._formFieldClist; } }

        public IEnumerable<DynamicControlViewModel> DynamicControls
        {
            set
            {
                var tb = new TextBox();
                var lbl = new Label();
                var gb = new GroupBox();
                var rtb = new RichTextBox();
                _btnCommit = new Button();
                var fp = new FlowLayoutPanel();
                SetFormProps();
                foreach (DynamicControlViewModel model in value)
                {
                    // build up user controls here....
                    switch (model.ControlType)
                    {
                        case DynamicControlViewModel.ControlTypes.TextBox:
                            tb = new TextBox();
                            tb.Name = model.TextBoxName;
                            tb.Text = model.TextBoxText;
                            SetTextboxProps(tb);
                            gb.Controls.Add(lbl);

                            break;
                        case DynamicControlViewModel.ControlTypes.Button:
                            _btnCommit = new Button();
                            this.Controls.Add(_btnCommit);
                            _btnCommit.Name = model.ButtonName;
                            _btnCommit.Text = model.ButtonText;
                            SetDefaultButtonProps(_btnCommit, rtb);
                            fp.Controls.Add(_btnCommit);
                            break;
                        case DynamicControlViewModel.ControlTypes.Label:
                            lbl = new Label();

                            lbl.Name = model.LabelName;
                            lbl.Text = model.LabelText;
                            SetLabelProps(lbl);
                            break;
                        case DynamicControlViewModel.ControlTypes.GroupBox:
                            gb = new GroupBox();
                            gb.Controls.Add(tb);
                            fp.Controls.Add(gb);
                            //SetDefaultGBoxProps(gb);
                            break;
                        case DynamicControlViewModel.ControlTypes.FlowLayoutPanel:
                            fp = new FlowLayoutPanel();
                            SetDefaultPanelProps(fp);
                            break;
                        case DynamicControlViewModel.ControlTypes.RichtextBox:
                            rtb = new RichTextBox();
                            SetDefaultRTBoxProps(rtb, fp);
                            fp.Controls.Add(rtb);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                this.Controls.Add(fp);
            }

        }


        public FormFieldControl(IEnumerable<DynamicControlViewModel> list, IPresenterFormFieldControlCallbacks callback)
        {
            this.DynamicControls = list;
            this._formFieldClist = this.Controls as ControlCollection;

            Attach(callback);
            InitializeComponent();
        }

        private void SetFormProps()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Zuordnungen";

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
        }

        public void Attach(IPresenterFormFieldControlCallbacks callback)
        {
            this._btnCommit.Click += (sender, e) => callback.OnBtnCommitClick();
            this.FormClosing += (sender, e) => callback.OnForm2Closed();
        }

        public void Detach(IPresenterFormFieldControlCallbacks callback)
        {
            this._btnCommit.Click -= (sender, e) => callback.OnBtnCommitClick();
            this.FormClosing -= (sender, e) => callback.OnForm2Closed();
        }

        public void FormDispose()
        {
            this.Dispose();
        }

        public void FormShow()
        {
            this.Show();
        }

        public void FormClose()
        {
            this.Close();
        }
    }
}
