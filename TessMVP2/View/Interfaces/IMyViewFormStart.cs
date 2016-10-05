using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;
using System.Windows.Forms;
namespace TessMVP2.View.Interfaces
{
    public interface IMyViewFormStart
    {
        string TextBoxText { get; set; }
        string RichTextBoxText { get; set; }
        Button Form1Btn1 { get; }
        Button Form1Btn2 { get; }
        Button Form1Btn3 { get; }
        ToolStripItem TsiFuji { get; }
        ToolStripItem TsiWia { get; }
        Form Form1 { get; }
        
        
    }
}
