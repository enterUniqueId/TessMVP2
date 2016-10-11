using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View;
using TessMVP2.Presenter.Interfaces.View;
using System.Windows.Forms;

namespace TessMVP2.View.Interfaces
{
    public interface IViewModelFormFieldControl:IView<IPresenterFormFieldControlCallbacks>
    {
        IEnumerable<DynamicControlViewModel> DynamicControls { set; }
        Control.ControlCollection FormFieldClist { get; }
        void FormDispose();
        void FormShow();
        void FormClose();
    }
}
