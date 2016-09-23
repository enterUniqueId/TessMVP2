using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;

namespace TessMVP2.View
{
    public partial class FormFieldControl : Form, IMyViewFormFieldControl
    {
        private Button _btnCommit;
        private Form.ControlCollection _form2ControlCollection;
        public Form Form2 { get { return this; } }
        public Button BtnCommit { get { return this._btnCommit; } set { this._btnCommit = value; } }
        //public Form.ControlCollection Form2ControlCollection { get { return this._form2ControlCollection; } set { this._form2ControlCollection = value; } }


        public FormFieldControl()
        {
            //InitializeComponent();
        }
    }
}
