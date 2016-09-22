using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TessMVP2.View.Interfaces;

namespace TessMVP2.View
{
    public partial class FormFieldControl : Form, IMyViewFormFieldControl
    {
        public Form Form2 { get { return this; } }

        public FormFieldControl()
        {
            //InitializeComponent();
        }
    }
}
