﻿using System;
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
            _controlList = new List<DynamicControlViewModel>();
            var dynControl = new DynamicControlViewModel();
            dynControl.ControlType = DynamicControlViewModel.ControlTypes.FlowLayoutPanel;
            _controlList.Add(dynControl);

            var lblList = new List<DynamicControlViewModel>();
            var tbList = new List<DynamicControlViewModel>();
            foreach (var kvp in _oldContactDict)
            {
                var lbl = new Label();
                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.Label;
                dynControl.LabelName = kvp.Key;
                dynControl.LabelText = kvp.Value;

                var cm = new ContextMenu();
                dynControl = new DynamicControlViewModel();
                dynControl.ControlType = DynamicControlViewModel.ControlTypes.Cmenu;
                dynControl.LabelName = kvp.Key;
                dynControl.LabelText = kvp.Value;

                SetLabelProps(lbl, kvp.Key, kvp.Value);
                SetContextmenuItems(cm, kvp.Key);
                lbl.ContextMenu = cm;
                lblList.Add(lbl);
            }

            foreach (var kvp in _newContactDict)
            {
                var tb = new TextBox();
                string sr = kvp.Value;
                if (kvp.Value == null)
                    sr = "";
                SetTbProps(tb, kvp.Key, sr);
                tbList.Add(tb);
            }

            for (int i = 0; i < lblList.Count; i++)     //#############eigentliches adden
            {
                var pan = new Panel();
                SetPanProps(pan);
                pan.Controls.Add(lblList[i]);

                var pbox = new PictureBox();
                SetPboxProps(pbox, lblList[i]);
                pan.Controls.Add(pbox);
                lblList[i].MaximumSize = pan.Size - pbox.Size;
                if (i < tbList.Count)
                {
                    tbList[i].MaximumSize = new Size(pan.Width - 15, pan.Height);
                    pan.Controls.Add(tbList[i]);
                    PaintPanel(tbList[i]);
                }

                fpan.Controls.Add(pan);
            }


            var btnPan = new Panel();
            SetPanProps(btnPan);
            //btnPan.Margin = new Padding(0, 0, 0, 30);
            _view3.BtnUpdate = new Button();

            //fpan.Controls.Add(_view3.BtnUpdate);
            _view3.BtnCreateNew = new Button();

            //fpan.Controls.Add(_view3.BtnCreateNew);
            _view3.BtnCancel = new Button();

            //fpan.Controls.Add(_view3.BtnCancel);

            btnPan.Controls.Add(_view3.BtnUpdate);
            SetButtonProps(_view3.BtnUpdate, "Update", btnPan);
            btnPan.Controls.Add(_view3.BtnCreateNew);
            SetButtonProps(_view3.BtnCreateNew, "Anlegen", btnPan);
            btnPan.Controls.Add(_view3.BtnCancel);
            SetButtonProps(_view3.BtnCancel, "Cancel", btnPan);

            fpan.Controls.Add(btnPan);
        }

        private void PaintPanel(TextBox tb)
        {
            Panel pan = tb.Parent as Panel;
            foreach (var kvp in _conformities)
            {
                if (kvp.Value == tb.Text)
                    pan.BackColor = Color.AliceBlue;
            }
        }

        private void SetTbProps(TextBox aTb, string aTbName, string aTbText)
        {
            aTb.Name = "Tb" + aTbName;
            aTb.Text = aTbText;
            aTb.Margin = new Padding(10);
            aTb.Dock = DockStyle.None;
            aTb.Location = new Point(9, 55);
            aTb.Size = TbAutosize(aTb);
            aTb.MinimumSize = new Size(80, 20);
        }

        private Size TbAutosize(TextBox tb)
        {
            Size size = TextRenderer.MeasureText(tb.Text, tb.Font);
            return size;
        }

        private void SetLabelProps(Label aLabel, string lblName, string dictVal)
        {
            aLabel.Name = "lbl" + lblName;
            aLabel.Text = lblName + ": " + dictVal;
            aLabel.AutoSize = true;
            aLabel.MinimumSize = new Size(120, 20);
            aLabel.Margin = new Padding(10);
            aLabel.Dock = DockStyle.None;
            aLabel.Location = new Point(9, 35);
        }

        private void SetButtonProps(Button btn, string text, Panel parent)
        {
            btn.Text = text;
            btn.AutoSize = true;
            btn.Dock = DockStyle.None;
            int xloc = (int)(parent.Width / 2 - btn.Width / 2);
            int yloc = (int)(parent.Height / 3 - btn.Height / parent.Controls.Count) * parent.Controls.Count;
            //btn.Margin = new Padding(padlr, 15, padlr, 0);
            btn.Location = new Point(xloc, yloc);
        }

        private void SetContextmenuItems(ContextMenu cm, string currentItem)
        {
            foreach (var kvp in _oldContactDict)
            {
                //string k = kvp.Key.Replace("Business", ""); //evtl. die contactListenNamen noch ändern

                if (currentItem.ToLower().Contains("nummer"))
                {
                    if (kvp.Key.ToLower().Contains("nummer"))
                    {
                        cm.MenuItems.Add(kvp.Key);
                    }
                }
                else
                {
                    if (!kvp.Key.ToLower().Contains("nummer"))
                        cm.MenuItems.Add(kvp.Key);
                }
            }
            string sr = currentItem.Replace("-", "");
            cm.Name = "F3Cm" + sr;
        }

        private void SetPboxProps(PictureBox pb, Label lbl)
        {
            pb.Image = _imgChecked;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y - (pb.Height - lbl.Height));
            pb.Margin = new Padding(30, 0, 0, 0);
            pb.Name = lbl.Name.Substring(3);
            pb.Dock = DockStyle.None;
            pb.Hide();
        }

        private void SetPanProps(Panel pan)
        {
            pan.MinimumSize = new Size(280, 120);
            //pan.AutoSize = true;
            pan.Visible = true;
            pan.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetFpanProps(FlowLayoutPanel fpan)
        {
            fpan.FlowDirection = FlowDirection.TopDown;
            fpan.Margin = new Padding(10);
            fpan.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 200);
            fpan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fpan.AutoSize = true;
            fpan.Dock = DockStyle.Fill;
            //fpan.AutoSize = true;
            //fpan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }
}
