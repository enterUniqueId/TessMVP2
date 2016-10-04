using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.View.Interfaces;
using System.Windows.Forms;

namespace TessMVP2.Presenter
{
    partial class TessPresenter : IMyPresenter
    {
        private IMyViewFormCompareContacts _view3;
        public IMyViewFormCompareContacts ViewForm3 { get { return _view3; } set { _view3 = value; } }
        public object View3 { get { return _view3; } }
        private List<Control> _clist;



        private void AttachView3Events()
        {
            //this._view3.Form3.FormClosed += (sender, e) => OnForm1Closed();
            this._view3.BtnUpdate.Click += (sender, e) => OnButtonUpdateForm3Click();
            this._view3.BtnCreateNew.Click += (sender, e) => OnButtonCreateNewContactClick();
            this._view3.BtnCancel.Click += (sender, e) => OnButtonCancelCompareClick();
            WireView3ContextMenus();
        }

        private void WireView3ContextMenus()
        {
            
            foreach (Control c in _clist)
            {
                if (c.GetType() == typeof(Label))
                {
                    if (c.ContextMenu.MenuItems.Count < 11)
                    {
                        c.ContextMenu.MenuItems[0].Click += new EventHandler (OnCmTelClick);
                        c.ContextMenu.MenuItems[1].Click += new EventHandler(OnCmTel2Click);
                        c.ContextMenu.MenuItems[2].Click += new EventHandler(OnCmMobilClick);
                        c.ContextMenu.MenuItems[3].Click += new EventHandler(OnCmFaxClick);
                    }
                    else
                    {
                        c.ContextMenu.MenuItems[0].Click += new EventHandler(OnCmNameClick);
                        c.ContextMenu.MenuItems[1].Click += new EventHandler(OnCmStrasseClick);
                        c.ContextMenu.MenuItems[2].Click += new EventHandler(OnCmPlzClick);
                        c.ContextMenu.MenuItems[3].Click += new EventHandler(OnCmOrtClick);
                        c.ContextMenu.MenuItems[4].Click += new EventHandler(OnCmPostfachClick);
                        c.ContextMenu.MenuItems[5].Click += new EventHandler(OnCmPosClick);
                        c.ContextMenu.MenuItems[6].Click += new EventHandler(OnCmInetClick);
                        c.ContextMenu.MenuItems[7].Click += new EventHandler(OnCmFirmaClick);
                        c.ContextMenu.MenuItems[8].Click += new EventHandler(OnCmEmailClick);
                        c.ContextMenu.MenuItems[9].Click += new EventHandler(OnCmEmail2Click);
                        c.ContextMenu.MenuItems[10].Click += new EventHandler(OnCmEmail3Click);
                    }
                }
            }
        }

        private void SetupContextEvent(object sender,string tbName)
        {
            var lbl = new Label();
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                ContextMenu owner = item.Parent as ContextMenu;
                if (owner != null)
                {
                    lbl = owner.SourceControl as Label;

                }
            }
            foreach (Control c in _clist)
            {
                if (c.Name == tbName && c is TextBox)
                {
                    lbl.Text = c.Text;
                    break;
                }
            }
        }

        private void OnCmTelClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbTelefon-Nummer");
        }

        private void OnCmTel2Click(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbTelefon-Nummer2");
        }

        private void OnCmMobilClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbMobil-Nummer");
        }

        private void OnCmFaxClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbFax-Nummer");
        }

        private void OnCmNameClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbName");
        }

        private void OnCmStrasseClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbStrasse");
        }

        private void OnCmOrtClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbOrt");
        }

        private void OnCmPlzClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbPostleitzahl");
        }

        private void OnCmPostfachClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbPostfach");
        }

        private void OnCmPosClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbPosition");
        }

        private void OnCmInetClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbInet");
        }

        private void OnCmFirmaClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbFirma");
        }

        private void OnCmEmailClick(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbEmail");
        }

        private void OnCmEmail2Click(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbEmail2");
        }

        private void OnCmEmail3Click(object sender, EventArgs e)
        {
            SetupContextEvent(sender, "TbEmail3");
        }

        private void OnButtonUpdateForm3Click()
        {
            _processUserInput.Clist = this._clist;
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs(true,true,3);
            this._inputResults = _processUserInput.ResDict;
            this._inputResults.Add("EntryID", _outlook.EntryID);
            _outlook.UpdateExistingContact(_inputResults);
            _view3.Form3.Close();
        }

        private void OnButtonCreateNewContactClick()
        {
            this._outlook.CreateContact();
            _view3.Form3.Close();
        }
    }
}
