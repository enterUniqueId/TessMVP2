using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Model;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;
using UnitTests.Mocks.Interfaces;

namespace UnitTests.Mocks
{
    class MockModelFormStart:IMyModel
    { 

        //scanner
        public bool DeviceIsSet { get; set; }
        public bool ImageIsSaved { get; set; }

        public MockModelFormStart(IMyPresenterModelCallbacks callbacks)
        {

        }

        public string OcrResult
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, List<string>> StringResult
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, string> ResFields
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ImgPath
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public OutlookWork OlWork
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler ImgfileSavedInFolder;

        //scanner
        public void CreateScannerObject()
        {
            DeviceIsSet = true;
        }

        public void Attach(IMockModelF1Callbacks callback)
        {
            ImgfileSavedInFolder += (sender, e) => callback.OnEventFileCreated();
        }

        public void Detach(IMockModelF1Callbacks callback)
        {
            ImgfileSavedInFolder-=(sender,e)=>callback.OnEventFileCreated();
        }

        public void Scan()
        {
            ImageIsSaved = true;
        }

        public void Attach(IMyPresenterModelCallbacks presenter)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterModelCallbacks presenter)
        {
            throw new NotImplementedException();
        }

        public void Start(IMyPresenterModelCallbacks callbacks)
        {
            throw new NotImplementedException();
        }
    }
}
