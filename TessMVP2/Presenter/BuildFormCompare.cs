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
        private IMyViewFormCompareContacts _view3;
        public object View3 { get { return _view3; } }
        private TessPresenter _mainPresenter;
        private FormCompareContacts _formObject;
        private Image _imgChecked;
        private Dictionary<string, string> _newContactDict;
        public FormCompareContacts FormCompareContacts { get { return this._formObject; } private set { this._formObject = value; } }
        private Dictionary<string, string> _oldContactDict;


        public BuildFormCompare(Dictionary<string, string> newContactvals, Dictionary<string, string> oldContactVals, TessPresenter presenter)
        {
            var view = new FormCompareContacts();
            this._view3 = view;
            this._mainPresenter = presenter;
            this._formObject = (FormCompareContacts)this._view3.Form3;
            this._oldContactDict = oldContactVals;
            this._oldContactDict.Remove("EntryID");
            this._newContactDict = newContactvals;
            this._newContactDict.Remove("EntryID");
            string imgFile = Environment.CurrentDirectory + "\\img\\chk2.png";
            this._imgChecked = Image.FromFile(imgFile);
            
            AddControls();
            _mainPresenter.ViewForm3 = this._view3;
        }

        private void SetFormProps(FlowLayoutPanel fp)
        {
            this._formObject.AutoSize = true;
            this._formObject.AutoSizeMode = AutoSizeMode.GrowOnly;
            this._formObject.Text = "Redundanter Eintrag";  //besseren Text finden
            //this._formObject.Width = fp.Width;
            //this._formObject.Height = fp.Height;
        }

        private void AddControls()
        {
            var fp = new FlowLayoutPanel();
           
            _formObject.Controls.Add(fp);
            AddControlsToPanel(fp);
            SetFpanProps(fp);
            SetFormProps(fp);
        }

        private void visibility(FlowLayoutPanel fp)
        {
            foreach(Control pan in fp.Controls)
            {
                pan.BringToFront();
                foreach (Control c in pan.Controls)
                    c.BringToFront();
            }
        }

        private void AddControlsToPanel(FlowLayoutPanel fpan)
        {
            
            var lblList = new List<Label>();
            var tbList = new List<TextBox>();
            foreach (var kvp in _oldContactDict)
            {
                var lbl = new Label();
                var cm = new ContextMenu();
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
                    sr= "";
                SetTbProps(tb, kvp.Key, sr);
                tbList.Add(tb);
            }

            for (int i = 0; i < lblList.Count; i++)     //#############eigentliches adden
            {
                var pan = new Panel();
                pan.Controls.Add(lblList[i]);
                
                var pbox = new PictureBox();
                SetPboxProps(pbox, lblList[i]);
                pan.Controls.Add(pbox);
                if (i < tbList.Count)
                    pan.Controls.Add(tbList[i]);
                
                SetPanProps(pan);
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
            aLabel.MinimumSize = new Size(120,20);
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
            int yloc = (int)(parent.Height / 3 - btn.Height/ parent.Controls.Count) * parent.Controls.Count;
            //btn.Margin = new Padding(padlr, 15, padlr, 0);
            btn.Location = new Point(xloc,yloc);
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
            cm.Name = "F3Cm"+sr;
        }

        private void SetPboxProps(PictureBox pb, Label lbl)
        {
            pb.Image = _imgChecked;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y - (pb.Height - lbl.Height));
            pb.Margin = new Padding(30, 0, 0, 0);
            pb.Dock = DockStyle.None;
        }

        private void SetPanProps(Panel pan)
        {
            pan.MinimumSize = new Size(280, 120);
            pan.AutoSize = true;
            pan.Visible = true;
            pan.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetFpanProps(FlowLayoutPanel fpan)
        {
            fpan.FlowDirection = FlowDirection.TopDown;
            fpan.Margin = new Padding(10);
            fpan.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width-100, Screen.PrimaryScreen.Bounds.Height-200);
            fpan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fpan.AutoSize = true;
            fpan.Dock = DockStyle.Fill;
            //fpan.AutoSize = true;
            //fpan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }
}
