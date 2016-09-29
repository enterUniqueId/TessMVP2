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
    public partial class FormYesNoCancel : Form, IViewFormYesNoCancel
    {
        private Button _btnYes;
        private Button _btnNo;
        private Button _btnCancel;

        public Button BtnYes { get { return this._btnYes; } set { this._btnYes = value; } }
        public Button BtnNo { get { return this._btnNo; } set { this._btnNo = value; } }
        public Button BtnCancel { get { return this._btnCancel; } set { this._btnCancel = value; } }
        public Form Form4 { get { return this; } }

        public FormYesNoCancel()
        {

        }
    }
}
