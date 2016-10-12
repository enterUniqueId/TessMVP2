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
using System.Windows;

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
                this._dynamicControls = value;
            }
        }

        public FormFieldControl(IEnumerable<DynamicControlViewModel> list, IPresenterFormFieldControlCallbacks callback)
        {
            var dfb = new DynamicFormBuilder(this, list);
            SetFormProps();
            this._formFieldClist = this.Controls as ControlCollection;
            _btnCommit = this.Controls.Find("F2btnCommit", true)[0] as Button;
            Attach(callback);
            //InitializeComponent();
        }

        private void SetFormProps()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Zuordnungen";

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
