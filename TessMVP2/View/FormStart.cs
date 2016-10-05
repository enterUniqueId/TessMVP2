using System;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;
using System.IO;

namespace TessMVP2.View
{
    public partial class FormStart : Form, IMyViewFormStart
    {

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

        public FormStart()
        {
            InitializeComponent();
            //button1.Click += (sender, e) => callback.OnButtonClick();
            textBox1.Text = "cd3.jpg";
            this.Text = "V-scanner";

        }

        public void Attach()
        {
            //_textBox1.TextChanged += (sender, e) => callback.OnTextChange();
            //_button1Clicked += (sender, e) => callback.OnButtonClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FormStart_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void fujitsuScanSnapSseriesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
