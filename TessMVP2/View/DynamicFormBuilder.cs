using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using TessMVP2.Model;

namespace TessMVP2.View
{
    class DynamicFormBuilder:IDynamicControlViewModel
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
                var cmenu = new ContextMenu();
                var cb = new ComboBox();
                foreach (DynamicControlViewModel model in value)
                {
                    switch (model.ControlType)
                    {
                        case DynamicControlViewModel.ControlTypes.TextBox:
                            tb = new TextBox();
                            tb.Name = model.TextBoxName;
                            tb.Text = model.TextBoxText;
                            SetTextboxProps(tb);
                            pan.Controls.Add(tb);
                            pan.Controls.Add(lbl);

                            break;
                        case DynamicControlViewModel.ControlTypes.Button:
                            btn = new Button();
                            _form.Controls.Add(btn);
                            btn.Name = model.ButtonName;
                            btn.Text = model.ButtonText;
                            SetDefaultButtonProps(btn, rtb);
                            pan.Controls.Add(btn);
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
                            gb.Controls.Add(lbl);
                            fp.Controls.Add(gb);
                            break;
                        case DynamicControlViewModel.ControlTypes.FlowLayoutPanel:
                            fp = new FlowLayoutPanel();
                            SetFpanProps(fp);
                            break;
                        case DynamicControlViewModel.ControlTypes.RichtextBox:
                            rtb = new RichTextBox();
                            SetDefaultRTBoxProps(rtb, pan);
                            fp.Controls.Add(rtb);
                            break;
                        case DynamicControlViewModel.ControlTypes.Panel:
                            pan = new Panel();
                            SetPanProps(pan, model.Col);
                            pan.Name = model.PanelName;
                            fp.Controls.Add(pan);
                            break;
                        case DynamicControlViewModel.ControlTypes.Pbox:
                            pbox = new PictureBox();
                            SetPboxProps(pbox, lbl, model);
                            pan.Controls.Add(pbox);
                            break;
                        case DynamicControlViewModel.ControlTypes.Cmenu:
                            cmenu = new ContextMenu();
                            cmenu.Name = model.CmenuName;
                            lbl.ContextMenu = cmenu;
                            break;
                        case DynamicControlViewModel.ControlTypes.CmenuItem:
                            cmenu.MenuItems.Add(model.CmenuItemName);
                            break;
                        case DynamicControlViewModel.ControlTypes.ComboBox:
                            cb = new ComboBox();
                            cb.Name = model.ComboBoxName;
                            var contlist = new List<object>();
                            foreach (ContactItem item in model.ComboBoxItems)
                            {
                                if (item.FullName == "dummy")
                                {
                                    item.Delete();
                                    continue;
                                }
                                contlist.Add(new { FullName = item.FullName, EntryID = item.EntryID });
                            }

                            cb.DataSource = null;
                            cb.Items.Clear();
                            cb.DataSource = new BindingSource(contlist, null);
                            cb.DisplayMember = "FullName";
                            cb.ValueMember = "EntryID";
                            if (cb.Items.Count > 0)
                                cb.SelectedItem = 0;

                            SetComboBoxProps(cb, fp);
                            fp.Controls.Add(cb);
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
            DynamicControls = list;

        }

        private void SetComboBoxProps(ComboBox box, FlowLayoutPanel fpan)
        {
            var parentPanel = fpan.Controls[fpan.Controls.Count - 1] as Panel;
            box.Margin = new Padding((int)(parentPanel.Width / 2 - box.Width / 2), 25, 0, 0);
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

        private void SetFpanProps(FlowLayoutPanel fpan)
        {
            fpan.FlowDirection = FlowDirection.TopDown;
            fpan.Margin = new Padding(10);
            fpan.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 200);
            fpan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fpan.AutoSize = true;
            fpan.Dock = DockStyle.Fill;
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

        private void SetPanProps(Panel pan, DynamicControlViewModel.Colors cols)
        {
            pan.MinimumSize = new Size(280, 120);
            //pan.AutoSize = true;
            pan.Visible = true;
            pan.BorderStyle = BorderStyle.Fixed3D;

            switch (cols)
            {
                case DynamicControlViewModel.Colors.control:
                    pan.BackColor = SystemColors.Control;
                    break;
                case DynamicControlViewModel.Colors.window:
                    pan.BackColor = SystemColors.Window;
                    break;
                case DynamicControlViewModel.Colors.AliceBlue:
                    pan.BackColor = Color.AliceBlue;
                    break;
                case DynamicControlViewModel.Colors.orange:
                    pan.BackColor = Color.Orange;
                    break;
                case DynamicControlViewModel.Colors.blue:
                    pan.BackColor = Color.Blue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetPboxProps(PictureBox pb, Label lbl, DynamicControlViewModel model)
        {
            pb.Image = _pboxImageChecked;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y - (pb.Height - lbl.Height));
            pb.Margin = new Padding(30, 0, 0, 0);
            pb.Name = model.PboxName;
            pb.Dock = DockStyle.None;
            pb.Hide();
        }


    }
}
