using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;
using System.Windows.Forms;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.View.Interfaces
{
    public interface IMyViewFormStart : IView<IMyPresenterFormStartCallbacks>
    {
        ToolStripItem TsiFuji { get; }
        ToolStripItem TsiWia { get; }
        string FormStartText { set; }
        string F1Btn1Text { set; }
        string F1lbl1Text { set; }
        bool BtnStatus { get; set; }
        void FormClose();
        void FormHide();
        void FormShow();
    }
}
