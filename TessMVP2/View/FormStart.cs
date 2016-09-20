using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.Controller.Interfaces;
using TessMVP2.View.Interfaces;

namespace TessMVP2
{
    public partial class FormStart : Form, IMyView
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

        public FormStart(IMyPresenterViewCallbacks callback)
        {
            InitializeComponent();

            //evtl Attach hierher?
            button1.Click += (sender, e) => callback.OnButtonClick();
            textBox1.Text = "cd1.jpg";
        }

        public void Attach(IMyPresenterViewCallbacks callback)
        {
            //_textBox1.TextChanged += (sender, e) => callback.OnTextChange();
            //_button1Clicked += (sender, e) => callback.OnButtonClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
