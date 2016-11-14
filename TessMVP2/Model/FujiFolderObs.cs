using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.ComponentModel;

[assembly: InternalsVisibleTo("UnitTests")]

namespace TessMVP2.Model
{
    
    public class FujiFolderObs:IFujiModel,IDisposable
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
            var curPath = Directory.GetCurrentDirectory();
            _fsw.Path = curPath+_tempDir;
            _fsw.Filter = _format;
            //Attach(callback);
        }

        public void Attach(IMyPresenterFujiCallbacks callback)
        {
            Initialize(callback);
            _fsw.Created += new FileSystemEventHandler(callback.OnImgFileCreated);
            _fsw.EnableRaisingEvents = true;
            
        }

        public void Detach(IMyPresenterFujiCallbacks callback)
        {
            _fsw.Created -= new FileSystemEventHandler(callback.OnImgFileCreated);
            _fsw.EnableRaisingEvents = false;
            Dispose();
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}
