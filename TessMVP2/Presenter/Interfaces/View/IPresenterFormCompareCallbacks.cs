using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View;

namespace TessMVP2.Presenter.Interfaces.View
{
    public interface IPresenterFormCompareCallbacks:IPresenter
    {
        void OnButtonUpdateClick();
        void OnButtonCreateClick();
        void OnButtonCancelClick();
        void OnForm3Closed();
        void OnCmClick(object sender, EventArgs e);
        void OnTbTextChanged(object sender, EventArgs e);
        void OnCbSelectedItemChange(object sender, EventArgs e);

    }
}
