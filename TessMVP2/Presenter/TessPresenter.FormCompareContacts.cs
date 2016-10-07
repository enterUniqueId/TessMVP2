using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
                    for (int i = 0; i < c.ContextMenu.MenuItems.Count; i++)
                    {
                        c.ContextMenu.MenuItems[i].Click += new EventHandler(OnCmClick);
                    }
                }
                if (c.GetType() == typeof(TextBox))
                {
                    c.TextChanged += new EventHandler(OnTbTextChanged);
                }
            }
        }

        private void OnTbTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Size = TbAutosize(tb);
        }

        private Size TbAutosize(TextBox tb)
        {
            Size size = TextRenderer.MeasureText(tb.Text, tb.Font);
            return size;
        }

        private void OnCmClick(object sender, EventArgs e)
        {
            //contextmenu
            SetupContextEvent(sender);
        }

        private void SetupContextEvent(object sender)
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

            Panel pan = lbl.Parent as Panel;
            pan.BackColor = SystemColors.Control;
            PictureBox pb = new PictureBox();
            foreach (Control c in pan.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    string sr = lbl.Text.Substring(0, lbl.Text.IndexOf(":") + 1);
                    lbl.Text = sr + c.Text;
                }

                if (c.GetType() == typeof(PictureBox))
                {
                    pb = c as PictureBox;
                    c.Show();
                }
            }
            pb.Location = new Point(lbl.Width + 25, lbl.Location.Y - (pb.Height - lbl.Height));
        }

        private void OnButtonUpdateForm3Click()
        {
            _processUserInput.Clist = this._clist;
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs(true, true, 3);
            this._inputResults = _processUserInput.ResDict;
            this._inputResults.Add("EntryID", _outlook.EntryID);
            _outlook.UpdateExistingContact(_inputResults);
            _view3.Form3.Close();
            _view2.Form2.Close();
        }

        private void OnButtonCreateNewContactClick()
        {
            this._outlook.CreateContact();
            _view3.Form3.Close();
            _view2.Form2.Close();
        }
    }
}
