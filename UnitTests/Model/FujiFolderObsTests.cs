using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Presenter.Interfaces;
using System.IO;
using System.Threading;
using System;

namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class FujiFolderObsTests:IMyPresenterFujiCallbacks
    {
        private bool _eventFired;


        [TestMethod()]
        public void FujiFolderObsTest()
        {
            var tempDir = @"\temp";
            var fujiFolderObsObject = new FujiFolderObs(this, tempDir, "*jpg");

            Assert.IsNotNull(fujiFolderObsObject.FSW);
            Assert.AreEqual(Directory.GetCurrentDirectory()+@"\temp", fujiFolderObsObject.FSW.Path);
        }

        public void OnImgFileCreated(object sender, FileSystemEventArgs e)
        {
            _eventFired = true;
        }
    }
}