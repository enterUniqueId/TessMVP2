using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;
using System.Windows.Forms;
namespace TessMVP2.View.Interfaces
{
    public interface IMyViewFormStart
    {
        //inhalt aller Elemente mit text-property
        string TextBoxText { get; set; }
        string RichTextBoxText { get; set; }
        Button Form1Btn1 { get; }
        Form Form1 { get; }
        
    }
}
