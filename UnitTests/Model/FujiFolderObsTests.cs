using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Presenter.Interfaces;
using System.IO;
using System.Threading;

namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class FujiFolderObsTests : IMyPresenterFujiCallbacks
    {
        private bool _eventFired;
        private string _tempDir;
        private FujiFolderObs _fujiFolderObsObject;
        private ManualResetEventSlim _me;
        public FujiFolderObsTests()
        {
            _tempDir = @"\temp";
            _fujiFolderObsObject = new FujiFolderObs(this, _tempDir, "*jpg");
            _me = new ManualResetEventSlim(false);
        }


        

        [TestMethod()]
        public void FujiFolderObsTest()
        {
            Assert.IsNotNull(_fujiFolderObsObject.FSW);
        }

        [TestMethod()]
        public void AttachTest()
        {
            _eventFired = false;
            _fujiFolderObsObject.Attach(this);
            OnImgFileCreatedTest(_fujiFolderObsObject, true);
            Assert.IsTrue(_eventFired);
        }

        [TestMethod()]
        public void DetachTest()
        {
            _eventFired = false;
            //im constructor attached
            _fujiFolderObsObject.Detach(this);
            OnImgFileCreatedTest(_fujiFolderObsObject, true);
            Assert.IsFalse(_eventFired);
            _me.Dispose();
        }

        [TestMethod()]
        private void OnImgFileCreatedTest(FujiFolderObs fujiFolderObsObject = null, bool isFunc = false)
        {
            string tempDir = @"\temp";
            var testFile = Directory.GetCurrentDirectory() + tempDir + @"\imgFileCreatedByEvent.jpg";
            if (File.Exists(testFile))
                File.Delete(testFile);
            if (fujiFolderObsObject == null)
            {
                fujiFolderObsObject = new FujiFolderObs(this, tempDir, "*jpg");
                _eventFired = false;
                fujiFolderObsObject.Attach(this);
            }
            
            File.Create(testFile);
            _me.Wait(500);
            if (!isFunc)
                Assert.IsTrue(_eventFired);
            
        }

        public void OnImgFileCreated(object sender, FileSystemEventArgs e)
        {
            _eventFired = true;
            _me.Set();
            _me.Dispose();
        }
    }
}