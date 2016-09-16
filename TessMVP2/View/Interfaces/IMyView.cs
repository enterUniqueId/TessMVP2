using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Controller.Interfaces;

namespace TessMVP2.View.Interfaces
{
    public interface IMyView:IView<IMyPresenterViewCallbacks>
    {
        //inhalt aller Elemente mit text-property
        string TextBoxText { get; set; }
        string RichTextBoxText { get; set; }
        
    }
}
