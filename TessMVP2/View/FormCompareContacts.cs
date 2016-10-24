using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using System.Drawing;
using TessMVP2.Presenter.Interfaces.View;


namespace TessMVP2.View
{

    public partial class FormCompareContacts : Form, IViewModelFormCompare
    {
        private IEnumerable<DynamicControlViewModel> _dynamicControls;
        private Control.ControlCollection _formCompareClist;
        private List<Control> _contlist;
        public Control.ControlCollection FormCompareClist { get { return this._formCompareClist; } }
        public string FormBezeichnung { get { return this.Text; } set { this.Text = value; } }

        public void SetNewLabelText(List<string> texts)
        {
            int j = 0;
            for (int i = 0; i < this._contlist.Count; i++)
            {
                var c = _contlist[i];
                if (c.GetType() == typeof(Label))
                {
                    c.Text = texts[j];
                    j++;
                }
            }
        }
        public IEnumerable<DynamicControlViewModel> DynamicControls
        {
            set
            {
                this._dynamicControls = value;
            }
        }


        private Button _update;
        private Button _createNew;
        private Button _cancel;
        private ComboBox _comboBox;

        public ComboBox ComboBox { get { return _comboBox; } set { _comboBox = value; } }
        public Button BtnUpdate { get { return this._createNew; } set { this._createNew = value; } }
        public Button BtnCreateNew { get { return this._update; } set { this._update = value; } }
        public Button BtnCancel { get { return this._cancel; } set { this._cancel = value; } }


        public FormCompareContacts(IEnumerable<DynamicControlViewModel> list)
        {
            var dfb = new DynamicFormBuilder(this, list);
            this._formCompareClist = this.Controls as ControlCollection;
            var bla = Controls;
            _update = Controls.Find("F3BtnUpdate", true)[0] as Button;
            _createNew = Controls.Find("F3BtnCreate", true)[0] as Button;
            _cancel = Controls.Find("F3BtnCancel", true)[0] as Button;
            var panel = Controls.Find("F3BtnPanel", true)[0] as Panel;
            _comboBox = Controls.Find("F3CbContacts", true)[0] as ComboBox;
            SetFormProps();

            SetButtonProps(_update, panel);
            SetButtonProps(_createNew, panel);
            SetButtonProps(_cancel, panel);
            //InitializeComponent();
        }

        private void SetFormProps()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
        }


        private void SetButtonProps(Button btn, Panel parent)
        {
            btn.AutoSize = true;
            btn.Width = _update.Width;
            btn.Dock = DockStyle.None;
            int xloc = (int)(parent.Width / 2 - btn.Width / 2);
            int yloc = (int)(parent.Height / 3 - btn.Height / parent.Controls.Count) * parent.Controls.IndexOf(btn) + 10;
            //btn.Margin = new Padding(padlr, 15, padlr, 0);
            btn.Location = new Point(xloc, yloc);
        }

        public void FormClose()
        {
            this.Close();
        }

        public void Detach(IPresenterFormCompareCallbacks callback)
        {
            _update.Click -= (sender, e) => callback.OnButtonUpdateClick();
            _createNew.Click -= (sender, e) => callback.OnButtonCreateClick();
            _cancel.Click -= (sender, e) => callback.OnButtonCancelClick();
            _comboBox.SelectedIndexChanged -= new EventHandler(callback.OnCbSelectedItemChange);
        }

        public void Attach(IPresenterFormCompareCallbacks callback)
        {
            _update.Click += (sender, e) => callback.OnButtonUpdateClick();
            _createNew.Click += (sender, e) => callback.OnButtonCreateClick();
            _cancel.Click += (sender, e) => callback.OnButtonCancelClick();
            _comboBox.SelectedIndexChanged += new EventHandler (callback.OnCbSelectedItemChange);
            WireView3ContextMenus(callback);
        }

        private void WireView3ContextMenus(IPresenterFormCompareCallbacks callback)
        {

            foreach (Control c in _contlist)
            {
                if (c.GetType() == typeof(Label))
                {
                    for (int i = 0; i < c.ContextMenu.MenuItems.Count; i++)
                    {

                        c.ContextMenu.MenuItems[i].Click += new EventHandler(callback.OnCmClick);
                    }
                }
                if (c.GetType() == typeof(TextBox))
                {
                    c.TextChanged += new EventHandler(callback.OnTbTextChanged);
                }
            }
        }

        public void FormShowDialog(List<Control> clist, IPresenterFormCompareCallbacks callback)
        {
            _contlist = clist;
            Attach(callback);
            this.ShowDialog();
        }
    }
}
