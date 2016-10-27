using System;
using TessMVP2.Presenter.Interfaces.View;
using TessMVP2.View.Interfaces;
using System.Windows.Forms;


namespace UnitTests.Mocks
{
    class MockViewFormStart : IMyViewFormStart
    {

        private bool _btnStatus;
        private string _btnText;
        private string _lbl1Text;
        private string _formText;
        private string[] _submenus = { "tsifuji", "tsiwia" };
        public bool FormClosed;
        public bool FormShown { get; private set; }
        public bool FormHidden;
        public event EventHandler btn1Click;
        public event EventHandler tsifujiClick;
        public event EventHandler tsiwiaClick;
        public event EventHandler formshowevent;
        public event EventHandler formclosing;

        public MockViewFormStart(IMyPresenterFormStartCallbacks callbacks)
        {
            Attach(callbacks);
            BtnStatus = true;
        }


        public bool BtnStatus
        {
            get
            {
                return _btnStatus;
            }

            set
            {
                _btnStatus = value;
            }
        }

        public string F1Btn1Text
        {
            set
            {

            }
        }

        public string F1lbl1Text
        {
            set
            {

            }
        }

        public string FormStartText
        {
            set
            {

            }
        }

        public string TsiFujiMock
        {
            get
            {
                return _submenus[0];
            }
        }

        public string TsiWiaMock
        {
            get
            {
                return _submenus[1];
            }
        }

        public void Attach(IMyPresenterFormStartCallbacks callback)
        {
            btn1Click += (sender, e) => callback.OnButtonClick();
            tsifujiClick += (sender, e) => callback.OnFujitsuClick();
            tsiwiaClick += (sender, e) => callback.OnWiaClick();
            formshowevent += (sender, e) => callback.OnForm1Shown();
            formclosing += (sender, e) => callback.OnForm1Closing();
        }

        public void Detach(IMyPresenterFormStartCallbacks callback)
        {
            btn1Click -= (sender, e) => callback.OnButtonClick();
            tsifujiClick -= (sender, e) => callback.OnFujitsuClick();
            tsiwiaClick -= (sender, e) => callback.OnWiaClick();
            formshowevent -= (sender, e) => callback.OnForm1Shown();
            formclosing -= (sender, e) => callback.OnForm1Closing();
        }

        public void FormClosing()
        {
            if (formclosing != null)
                formclosing(this, EventArgs.Empty);
        }

        public void FormClose()
        {
            FormClosed = true;
        }

        public void FormHide()
        {
            FormHidden = true;
        }

        public void FormShow()
        {
            FormShown = true;
            if (formshowevent != null)
                formshowevent(this, EventArgs.Empty);
        }



        public ToolStripItem TsiWia
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ToolStripItem TsiFuji
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
