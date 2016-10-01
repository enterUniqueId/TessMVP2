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
            this._newContactDict = newContactvals;
            SetFormProps();
            AddControls();
            _mainPresenter.ViewForm3 = this._view3;
            this._imgChecked = Image.FromFile(@"/img/chk2.png");
        }

        private void SetFormProps()
        {
            this._formObject.AutoSize = true;
            this._formObject.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._formObject.Text = "Redundanter Eintrag";  //besseren Text finden
        }

        private void AddControls()
        {
            var fp = new FlowLayoutPanel();
            SetFpanProps(FlowLayoutPanel fp);
            AddControlsToPanel(fp);

        }

        private void AddControlsToPanel(FlowLayoutPanel fpan)
        {
            
            var lblList = new List<Label>();
            var tbList = new List<TextBox>();
            foreach (var kvp in _oldContactDict)
            {
                var lbl = new Label();
                var cmenu = new ContextMenu();
                SetLabelProps(lbl, kvp.Key, kvp.Value);
                var cm = new ContextMenu();
                //SetCmProps(cm,getCmItems(kvp.Key),);
                lbl.ContextMenu = cm;
                lblList.Add(lbl);
            }

            foreach (var kvp in _newContactDict)
            {
                var tb = new TextBox();
                SetTbProps(tb, kvp.Key, kvp.Value);
                tbList.Add(tb);
            }

            for (int i = 0; i < lblList.Count; i++)
            {
                var pan = new Panel();
                pan.Controls.Add(lblList[i]);
                var pbox = new PictureBox();
                //SetPboxProps();
                if (i < tbList.Count)
                    pan.Controls.Add(tbList[i]);
                SetPanProps(pan);
                fpan.Controls.Add(pan);
            }

            _view3.BtnUpdate = new Button();
            fpan.Controls.Add(_view3.BtnUpdate);
           

            _view3.BtnCreateNew = new Button();
            fpan.Controls.Add(_view3.BtnCreateNew);

            _view3.BtnCancel = new Button();
            fpan.Controls.Add(_view3.BtnCancel);

            var btnPan = new Panel();
            SetPanProps(btnPan);
            btnPan.Controls.Add(_view3.BtnUpdate);
            btnPan.Controls.Add(_view3.BtnCreateNew);
            btnPan.Controls.Add(_view3.BtnCancel);
            fpan.Controls.Add(btnPan);

        }

        private void SetTbProps(TextBox aTb, string aTbName, string aTbText)
        {
            aTb.Name = "Tb" + aTbName;
            aTb.Text = aTbText;
            aTb.Width = 180;
            aTb.Margin = new Padding(10);
        }

        private void SetLabelProps(Label aLabel, string lblName, string dictVal)
        {
            aLabel.Name = "lbl" + lblName;
            aLabel.Text = lblName + ": " + dictVal;
            aLabel.Width = 200;
            aLabel.Margin = new Padding(10);
        }

        private void SetButtonProps(Button btn, string text, Panel parent)
        {
            btn.Text = text;
            btn.AutoSize = true;
            int padlr = (int)(parent.Width / 2 - btn.Width / 2);
            btn.Margin = new Padding(padlr, 15, padlr, 0);
        }

        private void SetContextmenuItems()
        {
            foreach (var kvp in _oldContactDict)
            {
                //string k = kvp.Key.Replace("Business", ""); //evtl. die contactListenNamen noch ändern
                
                if (kvp.Key.ToLower().Contains("number"))
                {
                    string[] itemStrings = { "Telefon", "Fax", "Mobil", "Plz" }; 
                }
                else
                {

                }
            }
        }

        private void SetPboxProps(PictureBox pb, Label lbl)
        {
            pb.Image = _imgChecked;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y - (pb.Height - lbl.Height));
            pb.Margin = new Padding(30, 0, 0, 0);
        }

        private void SetPanProps(Panel pan)
        {
            pan.Size = new Size(260, 100);
            pan.Dock = DockStyle.Fill;
            pan.Margin= new thick
        }

        private void SetFpanProps(FlowLayoutPanel fpan)
        {
            fpan.Dock = DockStyle.Fill;
            fpan.FlowDirection = FlowDirection.TopDown;
        }
    }
}
