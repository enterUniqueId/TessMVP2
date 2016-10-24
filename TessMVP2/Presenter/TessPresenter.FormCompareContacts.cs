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
        private string _lastContactIDSelected;



        public void OnTbTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Size = TbAutosize(tb);
            var pan = tb.Parent as Panel;
            var lbl = pan.Controls[2] as Label;
            lbl.Text = lbl.Text.Substring(0, lbl.Text.IndexOf(":") + 1) + tb.Text;
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

        private void SetupOwnPanel(object sender, string cmText)
        {
            var mi = sender as MenuItem;
            var mm = mi.GetContextMenu();
            var lbl = mm.SourceControl as Label;
            string oldLabelName = lbl.Name;
            var panel = lbl.Parent as Panel;
            var fpan = panel.Parent as FlowLayoutPanel;
            var tb = panel.Controls.Find("F3Tb" + oldLabelName.Substring(5), true)[0] as TextBox;
            var tb2 = fpan.Controls.Find("F3Tb" + cmText, true)[0] as TextBox;
            var lbl2 = fpan.Controls.Find("F3lbl" + cmText, true)[0] as Label;


            tb2.Name ="F3Tb"+ oldLabelName.Substring(5);
            lbl2.Text = oldLabelName.Substring(5)+":";
            lbl2.Name = oldLabelName;
            lbl.Name = "F3lbl" + cmText;
            lbl.Text = cmText + ":" + tb.Text;
            tb.Name = "F3Tb" + cmText;
            
        }

        public void BuildNewLabelText(MenuItem item, string tbText)
        {
            ContextMenu owner = item.Parent as ContextMenu;
            if (owner != null)
            {
                var lbl = owner.SourceControl as Label;
                //string sr = lbl.Text.Substring(0, lbl.Text.IndexOf(":") + 1);
                lbl.Text = item.Text + tbText;

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
                //SetupPanelItemsFromCm(sender, item.Text);
                //var tbText = GetTextBoxTextFromCmItem(sender, item.Text);
                //BuildNewLabelText(item, tbText);
                SetupOwnPanel(sender, item.Text);
            }
        }

        public void OnButtonUpdateClick()
        {
            _processUserInput.ResDict.Clear();
            _processUserInput.GetInputs(null, true, 5);
            this._inputResults = _processUserInput.ResDict;
            if (_outlook.EntryID == null)
                _outlook.EntryID = _lastContactIDSelected;
            this._inputResults.Add("EntryID", _outlook.EntryID);
            _outlook.UpdateExistingContact(_inputResults);
            _view3.FormClose();
        }

        public void OnButtonCreateClick()
        {
            this._outlook.CreateContact(_outlook.ResultDict);
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

        public void OnCbSelectedItemChange(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            _lastContactIDSelected = cb.SelectedValue.ToString();
            var contact = _outlook.GetContactItemFromID(_lastContactIDSelected);
            var labelList = _outlook.BuildLabelFromContact(contact);
            _view3.SetNewLabelText(labelList);
        }
    }
}