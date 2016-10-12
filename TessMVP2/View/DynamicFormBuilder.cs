using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;

namespace TessMVP2.View
{
    class DynamicFormBuilder
    {
        private Form _form;
        private Image _pboxImageChecked;
        public Form DynForm { get { return _form; } }

        public IEnumerable<DynamicControlViewModel> DynamicControls
        {
            get { return DynamicControls; }
            set
            {
                var tb = new TextBox();
                var lbl = new Label();
                var gb = new GroupBox();
                var pan = new Panel();
                var rtb = new RichTextBox();
                var btn = new Button();
                var fp = new FlowLayoutPanel();
                var pbox = new PictureBox();
                foreach (DynamicControlViewModel model in value)
                {
                    // build up user controls here....
                    switch (model.ControlType)
                    {
                        case DynamicControlViewModel.ControlTypes.TextBox:
                            //if (!model.TextBoxName.ToLower().Contains("name"))
                            tb = new TextBox();
                            tb.Name = model.TextBoxName;
                            tb.Text = model.TextBoxText;
                            SetTextboxProps(tb);

                            break;
                        case DynamicControlViewModel.ControlTypes.Button:
                            btn = new Button();
                            _form.Controls.Add(btn);
                            btn.Name = model.ButtonName;
                            btn.Text = model.ButtonText;
                            SetDefaultButtonProps(btn, rtb);
                            fp.Controls.Add(btn);
                            break;
                        case DynamicControlViewModel.ControlTypes.Label:
                            //if (!model.LabelName.ToLower().Contains("name"))
                            lbl = new Label();
                            lbl.Name = model.LabelName;
                            lbl.Text = model.LabelText;
                            SetLabelProps(lbl);
                            break;
                        case DynamicControlViewModel.ControlTypes.GroupBox:
                            gb = new GroupBox();
                            gb.Controls.Add(tb);
                            gb.Controls.Add(lbl);
                            fp.Controls.Add(gb);
                            //SetDefaultGBoxProps(gb);
                            break;
                        case DynamicControlViewModel.ControlTypes.FlowLayoutPanel:
                            fp = new FlowLayoutPanel();
                            SetDefaultPanelProps(fp);
                            break;
                        case DynamicControlViewModel.ControlTypes.RichtextBox:
                            rtb = new RichTextBox();
                            SetDefaultRTBoxProps(rtb, pan);
                            fp.Controls.Add(rtb);
                            break;
                        case DynamicControlViewModel.ControlTypes.Panel:
                            pan = new Panel();
                            SetPanProps(pan);
                            pan.Controls.Add(tb);
                            pan.Controls.Add(lbl);
                            fp.Controls.Add(pan);
                            break;
                        case DynamicControlViewModel.ControlTypes.Pbox:
                            pbox = new PictureBox();
                            SetPboxProps(pbox, lbl);
                            pan.Controls.Add(pbox);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                _form.Controls.Add(fp);
            }

        }

        public DynamicFormBuilder(Form form, IEnumerable<DynamicControlViewModel> list)
        {
            this._form = form;
            string imgFile = Environment.CurrentDirectory + "\\img\\chk2.png";
            this._pboxImageChecked = Image.FromFile(imgFile);

        }

        private void SetTextboxProps(TextBox tb)
        {
            tb.Margin = new Padding(10);
            tb.Dock = DockStyle.None;
            tb.Location = new Point(9, 55);
            tb.Size = TbAutosize(tb);
            tb.MinimumSize = new Size(80, 20);
        }

        private Size TbAutosize(TextBox tb)
        {
            Size size = TextRenderer.MeasureText(tb.Text, tb.Font);
            return size;
        }

        private void SetLabelProps(Label lbl)
        {
            lbl.AutoSize = true;
            lbl.MinimumSize = new Size(120, 20);
            lbl.Margin = new Padding(10);
            lbl.Dock = DockStyle.None;
            lbl.Location = new Point(9, 35);
        }

        private void SetDefaultPanelProps(FlowLayoutPanel pan)
        {
            pan.AutoSize = true;
            pan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pan.FlowDirection = FlowDirection.TopDown;
            pan.Margin = new Padding(10);
            pan.MaximumSize = new System.Drawing.Size(1333, 750);
        }

        private void SetDefaultRTBoxProps(RichTextBox rtb, Panel parent)
        {
            rtb.Height = Convert.ToInt32(parent.Height * 2);
            rtb.Width = Convert.ToInt32(parent.Width * 1.5);
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

        private void SetPanProps(Panel pan)
        {
            pan.MinimumSize = new Size(280, 120);
            //pan.AutoSize = true;
            pan.Visible = true;
            pan.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetPboxProps(PictureBox pb, Label lbl)
        {
            pb.Image = _pboxImageChecked;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y - (pb.Height - lbl.Height));
            pb.Margin = new Padding(30, 0, 0, 0);
            pb.Name = lbl.Name.Substring(3);
            pb.Dock = DockStyle.None;
            pb.Hide();
        }


    }
}
