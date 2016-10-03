using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;


namespace TessMVP2.View
{
    public partial class FormCompareContacts : Form, IMyViewFormCompareContacts
    {
        private Button _update;
        private Button _createNew;
        private Button _cancel;
        private ContextMenu _name;
        private ContextMenu _tel;
        private ContextMenu _tel2;
        private ContextMenu _mobil;
        private ContextMenu _fax;
        private ContextMenu _strasse;
        private ContextMenu _plz;
        private ContextMenu _ort;
        private ContextMenu _postfach;
        private ContextMenu _pos;
        private ContextMenu _inet;
        private ContextMenu _firma;
        private ContextMenu _email;
        private ContextMenu _email2;
        private ContextMenu _email3;

        public Button BtnUpdate { get { return this._createNew; } set { this._createNew = value; } }
        public Button BtnCreateNew { get { return this._update; } set { this._update = value; } }
        public Button BtnCancel { get { return this._cancel; } set { this._cancel = value; } }
        public Form Form3 { get { return this; } }
        public ContextMenu F3CmName { get { return _name; } set { _name = value; } }
        public ContextMenu F3CmTelefonNummer { get { return _tel; } set { _tel = value; } }
        public ContextMenu F3CmTelefonNummer2 { get { return _tel2; } set { _tel2 = value; } }
        public ContextMenu F3CmMobilNummer { get { return _mobil; } set { _mobil = value; } }
        public ContextMenu F3CmFaxNummer { get { return _fax; } set { _fax = value; } }
        public ContextMenu F3CmStrasse { get { return _strasse; } set { _strasse = value; } }
        public ContextMenu F3CmPostleitzahl { get { return _plz; } set { _plz = value; } }
        public ContextMenu F3CmOrt { get { return _ort; } set { _ort = value; } }
        public ContextMenu F3CmPostfach { get { return _postfach; } set { _postfach = value; } }
        public ContextMenu F3CmPosition { get { return _pos; } set { _pos = value; } }
        public ContextMenu F3CmHomepage { get { return _inet; } set { _inet = value; } }
        public ContextMenu F3CmFirma { get { return _firma; } set { _firma = value; } }
        public ContextMenu F3CmEmail { get { return _email; } set { _email = value; } }
        public ContextMenu F3CmEmail2 { get { return _email2; } set { _email2 = value; } }
        public ContextMenu F3CmEmail3 { get { return _email3; } set { _email3 = value; } }


        public FormCompareContacts()
        {
            //InitializeComponent();
        }
    }
}
