using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TessMVP2.Presenter.Interfaces
{
    public interface IMyPresenterFujiCallbacks:IMyPresenter
    {
        void OnImgFileCreated(object sender, FileSystemEventArgs e);
    }
}
