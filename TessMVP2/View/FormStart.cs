using System;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using System.IO;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.View
{
    public partial class FormStart : Form, IMyViewFormStart
    {
        public string FormStartText { get { return this.Text; } set { this.Text = value; } }
        public string RichTextBoxText
        {
            get { return richTextBox1.Text; }
            set
            {
                richTextBox1.Clear();
                richTextBox1.AppendText(value);
            }
        }
        public bool BtnStatus { get { return this.button1.Enabled; } set { this.button1.Enabled = value; } }
        public ToolStripItem TsiFuji
        {
            get
            {
                var submenu = (ToolStripMenuItem)menuStrip1.Items[0];
                return submenu.DropDown.Items[0];
            }
        }
        public ToolStripItem TsiWia
        {
            get
            {
                var submenu = (ToolStripMenuItem)menuStrip1.Items[0];

                return submenu.DropDown.Items[1];
            }
        }

        public string F1Btn1Text { get { return this.button1.Text; }set { this.button1.Text = value; } }

        public FormStart(IMyPresenterFormStartCallbacks callback)
        {
            InitializeComponent();
            Attach(callback);
            var tsddm = this.scannerToolStripMenuItem.DropDown as ToolStripDropDownMenu;
            tsddm.ShowCheckMargin = true;
            tsddm.ShowImageMargin = true;

        }

        public void Attach(IMyPresenterFormStartCallbacks callback)
        {
            this.button1.Click += (sender, e) => callback.OnButtonClick();
            this.TsiFuji.Click += (sender, e) => callback.OnFujitsuClick();
            this.TsiWia.Click += (sender, e) => callback.OnWiaClick();
            this.Shown += (sender, e) => callback.OnForm1Shown();
            this.Closing += (sender, e) => callback.OnForm1Closing();
        }

        public void Detach(IMyPresenterFormStartCallbacks callback)
        {
            this.button1.Click -= (sender, e) => callback.OnButtonClick();
            this.TsiFuji.Click -= (sender, e) => callback.OnFujitsuClick();
            this.TsiWia.Click -= (sender, e) => callback.OnWiaClick();
            this.Closing -= (sender, e) => callback.OnForm1Closing();
            this.Shown -= (sender, e) => callback.OnForm1Shown();
        }

        public void FormClose()
        {
            this.Close();
            Application.Exit();
        }

        public void FormHide()
        {
            this.Hide();
        }

        public void FormShow()
        {
            this.Show();
        }


    }
}
