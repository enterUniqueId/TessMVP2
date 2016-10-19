using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.View.Interfaces;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.Presenter
{
    partial class TessPresenter : IPresenterFormCompareCallbacks
    {
        private IViewModelFormCompare _view3;
        public object View3 { get { return _view3; } }
        private List<Control> _clist;



        public void OnTbTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Size = TbAutosize(tb);
        }

        private Size TbAutosize(TextBox tb)
        {
            Size size = TextRenderer.MeasureText(tb.Text, tb.Font);
            return size;
        }

        public void OnCmClick(object sender, EventArgs e)
        {
            //contextmenu
            SetupContextEvent(sender,e);
        }

        public void SetupContextEvent(object sender, EventArgs e)
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

        public void OnButtonUpdateClick()
        {
            _processUserInput.Clist = this._clist;
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs(true, true, 5);
            this._inputResults = _processUserInput.ResDict;
            this._inputResults.Add("EntryID", _outlook.EntryID);
            _outlook.UpdateExistingContact(_inputResults);
            _view3.FormClose();
            _view2.FormClose();
        }

        public void OnButtonCreateClick()
        {
            this._outlook.CreateContact();
            _view3.FormClose();
            _view2.FormClose();
        }

        public void OnButtonCancelClick()
        {
            _view3.FormClose();
        }

        public void OnForm3Closed()
        {
            //todo
        }
    }
}
