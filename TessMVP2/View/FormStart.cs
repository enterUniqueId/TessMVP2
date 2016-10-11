using System;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using System.IO;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.View
{
    public partial class FormStart : Form, IMyViewFormStart
    {
        private string _formStartText;
        public string FormStartText { get { return this._formStartText; } set { this._formStartText = value; } }
        public Form Form1 { get { return this; } }

        public string TextBoxText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string RichTextBoxText
        {
            get { return richTextBox1.Text; }
            set
            {
                richTextBox1.Clear();
                richTextBox1.AppendText(value);
            }
        }
        public Button Form1Btn1 { get { return this.button1; } }
        public Button Form1Btn2 { get { return this.button2; } }
        public Button Form1Btn3 { get { return this.button3; } }
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

        public FormStart(IMyPresenterFormStartCallbacks callback)
        {
            InitializeComponent();
            Attach(callback);
            textBox1.Text = "cd3.jpg";
            this.Text = "V-scanner";

        }

        public void Attach(IMyPresenterFormStartCallbacks callback)
        {
            this.button1.Click += (sender, e) => callback.OnButtonClick();
            this.FormClosed += (sender, e) => callback.OnForm1Closed();
            this.button2.Click += (sender, e) => callback.OnButton2Click();
            this.button3.Click += (sender, e) => callback.OnButton3Click();
            this.TsiFuji.Click += (sender, e) => callback.OnFujitsuClick();
            this.TsiWia.Click += (sender, e) => callback.OnWiaClick();
            this.FormClosing += (sender, e) => callback.OnForm1Closing();
            this.Shown += (sender, e) => callback.OnForm1Shown();


        }

        public void Detach(IMyPresenterFormStartCallbacks callback)
        {
            this.button1.Click -= (sender, e) => callback.OnButtonClick();
            this.FormClosed -= (sender, e) => callback.OnForm1Closed();
            this.button2.Click -= (sender, e) => callback.OnButton2Click();
            this.button3.Click -= (sender, e) => callback.OnButton3Click();
            this.TsiFuji.Click -= (sender, e) => callback.OnFujitsuClick();
            this.TsiWia.Click -= (sender, e) => callback.OnWiaClick();
            this.FormClosing -= (sender, e) => callback.OnForm1Closing();
        }


    }
}
