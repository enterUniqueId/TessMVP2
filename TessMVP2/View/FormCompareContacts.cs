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
    public partial class FormCompareContacts : Form,IViewModelFormCompare
    {
        private IEnumerable<DynamicControlViewModel> _dynamicControls;
        private Control.ControlCollection _formCompareClist;
        private List<Control> _contlist;
        public Control.ControlCollection FormCompareClist { get { return this._formCompareClist; } }


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


        public Button BtnUpdate { get { return this._createNew; } set { this._createNew = value; } }
        public Button BtnCreateNew { get { return this._update; } set { this._update = value; } }
        public Button BtnCancel { get { return this._cancel; } set { this._cancel = value; } }



        public FormCompareContacts(IEnumerable<DynamicControlViewModel> list)
        {
            var dfb = new DynamicFormBuilder(this, list);
            this._formCompareClist = this.Controls as ControlCollection;
            var bla = this.Controls;
            _update = this.Controls.Find("F3BtnUpdate", true)[0] as Button;
            _createNew= this.Controls.Find("F3BtnCreate", true)[0] as Button;
            _cancel = this.Controls.Find("F3BtnCancel", true)[0] as Button;
            SetFormProps();
            //InitializeComponent();
        }

        private void SetFormProps()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.Text = "Übereinstimmung gefunden";
        }


        private void SetButtonProps(Button btn, Panel parent)
        {
            btn.AutoSize = true;
            btn.Dock = DockStyle.None;
            int xloc = (int)(parent.Width / 2 - btn.Width / 2);
            int yloc = (int)(parent.Height / 3 - btn.Height / parent.Controls.Count) * parent.Controls.Count;
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
        }

        public void Attach(IPresenterFormCompareCallbacks callback)
        {
            _update.Click += (sender, e) => callback.OnButtonUpdateClick();
            _createNew.Click += (sender, e) => callback.OnButtonCreateClick();
            _cancel.Click += (sender, e) => callback.OnButtonCancelClick();
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
