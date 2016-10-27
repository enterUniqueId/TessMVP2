using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Model;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;

namespace UnitTests.Mocks
{
    class MockModel:IMyModel,IFujiModel,IOutlookModel
    {

        //scanner
        private Dictionary<string, string> _resfields;
        private string _imgPath;
        private TessOcr _ocr;
        private OutlookWork _olwork;
        private StringProcessor _stringProcessor;
        private Dictionary<string, string> _resFields;
        private List<string> _hits;
        public bool DeviceIsSet { get; set; }
        public bool ImageIsSaved { get; set; }


        public MockModel(IMyPresenterModelCallbacks callbacks)
        {

        }

        public string OcrResult { get; private set; }

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
                return _resfields;
            }
            set
            {
                _resfields = value;
            }
        }

        public string ImgPath
        {
            get
            {
                return _imgPath;
            }

            set
            {
                _imgPath = value; 
            }
        }

        public OutlookWork OlWork
        {
            get
            {
                return _olwork;
            }

            set
            {
                _olwork = value;
            }
        }

        public List<string> Hits
        {
            get
            {
                return _hits;
            }
        }

        public event EventHandler ImgfileSavedInFolder;

        //scanner
        public void CreateScannerObject()
        {
            DeviceIsSet = true;
        }

        public void Scan()
        {
            ImageIsSaved = true;
        }

        public void Attach(IMyPresenterModelCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterModelCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void Start(IMyPresenterModelCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void Attach(IMyPresenterFujiCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterFujiCallbacks callbacks)
        {
            throw new NotImplementedException();
        }

        public void GetContacts()
        {
            throw new NotImplementedException();
        }

        public void Attach(IMyPresenterOutlookCallbacks presenter)
        {
            throw new NotImplementedException();
        }

        public void Detach(IMyPresenterOutlookCallbacks presenter)
        {
            throw new NotImplementedException();
        }
    }
}
