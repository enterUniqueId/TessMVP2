using System.Collections.Generic;
using TessMVP2.Presenter.Interfaces.View;
using System.Windows.Forms;
using TessMVP2.Model;

namespace TessMVP2.View.Interfaces
{
    public interface IViewModelFormCompare:IView<IPresenterFormCompareCallbacks>
    {
        IEnumerable<DynamicControlViewModel> DynamicControls { set; }
        Control.ControlCollection FormCompareClist { get; }
        string FormBezeichnung { get; set; }
        void SetNewLabelText(List<string> texts);
        void FormClose();
        void FormShowDialog(List<Control> clist, IPresenterFormCompareCallbacks callback);
    }
}
