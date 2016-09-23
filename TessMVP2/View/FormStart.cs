using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces;
using TessMVP2.View.Interfaces;

namespace TessMVP2.View
{
    public partial class FormStart : Form, IMyViewFormStart
    {

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
        public Button Form1Btn1
        {
            get { return this.button1; }
        }

        public Form Form1 { get { return this; } }
        
        public FormStart()
        {
            InitializeComponent();
            //button1.Click += (sender, e) => callback.OnButtonClick();
            textBox1.Text = "cd1.jpg";
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
    }
}
