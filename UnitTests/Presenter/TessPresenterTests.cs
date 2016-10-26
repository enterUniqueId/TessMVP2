using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TessMVP2.View.Interfaces;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;
using UnitTests.Mocks.Interfaces;
using UnitTests.Mocks;
using TessMVP2.Presenter.Interfaces.View;

namespace TessMVP2.Presenter.Tests
{
    [TestClass()]
    public class TessPresenterTests : IMyPresenterModelCallbacks, IMyPresenterFormStartCallbacks
    {

        private MockViewFormStart _view1;
        private MockModelFormStart _model1;
        TessPresenter presenter;
        //zum übergeben an den presenter
        IMyPresenterModelCallbacks model;
        IMyViewFormStart view;

        public object View1
        {
            get
            {
                return view;
            }
        }

        public object View2
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object View3
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object Model
        {
            get
            {
                return model;
            }
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {

            _model1 = new MockModelFormStart(this);
            _view1 = new MockViewFormStart(this);
            presenter = new TessPresenter(_view1, _model1);

        }

        [TestMethod()]
        public void TessPresenterTest()
        {
            Assert.IsTrue(_view1.BtnStatus.Equals(false));
        }

        [TestMethod()]
        public void OnButtonClickTest()
        {

            presenter.OnButtonClick();
            Assert.IsTrue(_view1.BtnStatus.Equals(true));

        }

        [TestMethod()]
        public void OnFujitsuClickTest()
        {
            presenter.OnFujitsuClick();
            Assert.IsTrue(_view1.BtnStatus.Equals(false));
        }

        [TestMethod()]
        public void OnWiaClickTest()
        {
            presenter.OnWiaClick();
            Assert.IsTrue(_view1.BtnStatus.Equals(true));
        }

        [TestMethod()]
        public void OnForm1ClosingTest()
        {
            Assert.Fail();
        }


        [TestMethod()]
        public void OnOcrResultChangedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnStringFinishedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnImgFileCreatedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnNoDuplicatesFoundTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnEventFileCreated()
        {
            throw new NotImplementedException();
        }

        public void OnButtonClick()
        {
            throw new NotImplementedException();
        }

        public void OnFujitsuClick()
        {
            throw new NotImplementedException();
        }

        public void OnWiaClick()
        {
            throw new NotImplementedException();
        }

        public void OnForm1Shown()
        {
            Assert.IsTrue(_view1.FormShown);
        }

        public void OnForm1Closing()
        {
            throw new NotImplementedException();
        }

        public void OnOcrResultChanged()
        {
            throw new NotImplementedException();
        }

        public void OnStringFinished()
        {
            throw new NotImplementedException();
        }
    }
}