using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces
{
    public interface IPresenter
    {
        //
        //void Initialize();
        object View1 { get; }
        object View2 { get; }
        object View3 { get; }
        object Model { get; }
    }
}
