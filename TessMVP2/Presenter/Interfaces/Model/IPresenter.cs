using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces
{
    public interface IPresenter
    {
        object View1 { get; }
        object View3 { get; }
        object Model { get; }
    }
}
