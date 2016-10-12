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
    public interface IViewModelFormCompare:IView<IPresenterFormCompareCallbacks>
    {
        IEnumerable<DynamicControlViewModel> DynamicControls { set; }
        Control.ControlCollection FormCompareClist { get; }
        void FormClose();
        void FormShowDialog(List<Control> clist, IPresenterFormCompareCallbacks callback);

        Button BtnUpdate { get; set; }
        Button BtnCreateNew { get; set; }
        Button BtnCancel { get; set; }


    }
}
