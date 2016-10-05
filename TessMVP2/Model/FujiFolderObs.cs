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
        private string _folder;
        private FileSystemWatcher _fsw;
        public FileSystemWatcher FSW { get { return this._fsw; } }
        public string Folder { get { return this._folder; } }
        

        public FujiFolderObs(IMyPresenterFujiCallbacks callback)
        {
            Initialize(callback);
        }

        private void Initialize(IMyPresenterFujiCallbacks callback)
        {
           _fsw = new FileSystemWatcher();

            var curPath = Directory.GetCurrentDirectory();
            var tempDir = @"\temp";
            _fsw.Path = curPath+tempDir;
            _fsw.Filter = "*.jpg";
            _fsw.Created += new FileSystemEventHandler(callback.OnImgFileCreated);
            _fsw.EnableRaisingEvents = true;
        }

        public void Attach(IMyPresenterFujiCallbacks callback)
        {
            
        }

        public void Detach(IMyPresenterFujiCallbacks presenter)
        {
            
        }
    }
}
