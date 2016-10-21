using System;
using System.Collections.Generic;
using System.Drawing;
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
            SetupContextEvent(sender, e);
        }

        public string GetTextBoxTextFromCmItem(object sender, string cmText)
        {

            var mi = sender as MenuItem;
            var mm = mi.GetContextMenu();
            var lbl = mm.SourceControl;
            if (lbl is Control)
            {
                var parent = lbl as Control;
                while ((parent = parent.Parent) != null)
                {
                    if (parent.GetType() == typeof(FlowLayoutPanel))
                    {

                        return (((FlowLayoutPanel)parent).Controls.Find("F3Tb" + cmText, true)[0] as TextBox).Text;
                    }
                }
            }
            return null;
        }

        public void BuildNewLabelText(MenuItem item, string tbText)
        {
            ContextMenu owner = item.Parent as ContextMenu;
            if (owner != null)
            {
                var lbl = owner.SourceControl as Label;
                string sr = lbl.Text.Substring(0, lbl.Text.IndexOf(":") + 1);
                lbl.Text = sr + tbText;

                var pan = lbl.Parent as Panel;
                //pan.BackColor = SystemColors.Control;
                foreach (Control c in pan.Controls)
                {
                    if (c.GetType() == typeof(PictureBox))
                    {
                        var pb = c as PictureBox;
                        pb.Location = new Point(lbl.Width + 25, lbl.Location.Y - (pb.Height - lbl.Height));
                        pb.Show();
                    }
                }
            }
        }

        public void SetupContextEvent(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                var tbText = GetTextBoxTextFromCmItem(sender, item.Text);
                BuildNewLabelText(item, tbText);
            }
        }

        public void OnButtonUpdateClick()
        {
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs(null, true, 5);
            this._inputResults = _processUserInput.ResDict;
            this._inputResults.Add("EntryID", _outlook.EntryID);
            _outlook.UpdateExistingContact(_inputResults);
            _view3.FormClose();
            try
            {
                _view2.FormClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
