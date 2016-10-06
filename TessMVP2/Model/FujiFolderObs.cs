using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;

namespace TessMVP2.Model
{
    class FujiFolderObs:IFujiModel
    {
        private FileSystemWatcher _fsw;
        private string _tempDir;
        private string _format;
        public FileSystemWatcher FSW { get { return this._fsw; } }
        

        public FujiFolderObs(IMyPresenterFujiCallbacks callback,string tempDir, string format)
        {
            this._format = format;
            this._tempDir = tempDir;
            Initialize(callback);
        }

        private void Initialize(IMyPresenterFujiCallbacks callback)
        {
           _fsw = new FileSystemWatcher();
            Attach(callback);
            var curPath = Directory.GetCurrentDirectory();
            _fsw.Path = curPath+_tempDir;
            _fsw.Filter = _format;
            _fsw.EnableRaisingEvents = true;
        }

        public void Attach(IMyPresenterFujiCallbacks callback)
        {
            _fsw.Created += new FileSystemEventHandler(callback.OnImgFileCreated);
        }

        public void Detach(IMyPresenterFujiCallbacks callback)
        {
            _fsw.Created -= new FileSystemEventHandler(callback.OnImgFileCreated);
        }
    }
}
