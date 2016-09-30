using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //private Dictionary<string, string> _oldContactVals;
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
            AddControlsToPanel(fp);

        }

        private void AddControlsToPanel(FlowLayoutPanel fpan)
        {
            var lblList = new List<Label>();
            var tbList = new List<TextBox>();
            foreach(var kvp in _oldContactDict)
            {
                var lbl = new Label();
                SetLabelProps(lbl, kvp.Key, kvp.Value);
                lblList.Add(lbl);
            }

            foreach(var kvp in _newContactDict)
            {
                var tb = new TextBox();
                SetTbProps(tb,kvp.Key, kvp.Value);
                tbList.Add(tb);
            }
        }

        private void SetTbProps(TextBox aTb,string aTbName, string aTbText)
        {
            aTb.Name = "Tb" + aTbName;
            aTb.Text = aTbText;
            aTb.Width = 180;
            aTb.Margin = new Padding(10, 5, 70, 50);
        }

        private void SetLabelProps(Label aLabel, string lblName, string dictVal)
        {
            aLabel.Name = "lbl" + lblName;
            aLabel.Text = lblName + ": " + dictVal;
            aLabel.Width = 200;
            aLabel.Margin = new Padding(10,5,50,5);
        }
    }
}
