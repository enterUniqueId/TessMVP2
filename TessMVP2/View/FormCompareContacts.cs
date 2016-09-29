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
        private Button _update;
        private Button _createNew;
        private Button _cancel;
        public Button BtnUpdate { get { return this._createNew; } set { this._createNew = value; } }
        public Button BtnCreateNew { get { return this._update; } set { this._update = value; } }
        public Button BtnCancel { get { return this._cancel; } set { this._cancel = value; } }
        public Form Form3 { get { return this; } }


        public FormCompareContacts()
        {

        }

        private void InitializeComponent()
        {
           

        }
    }
}
