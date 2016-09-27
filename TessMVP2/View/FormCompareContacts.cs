using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;


namespace TessMVP2.View
{
    class FormCompareContacts : Form, IMyViewFormCompareContacts
    {
        private Button _btnCommit;
        public Button BtnCommit { get { return this._btnCommit; } set { this._btnCommit = value; } }
        public Form Form3 { get { return this; } }


        public FormCompareContacts()
        {

        }
    }
}
