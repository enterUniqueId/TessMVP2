using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.View.Interfaces;
using TessMVP2.View;
using System.Windows.Forms;
using System.Drawing;

namespace TessMVP2.Presenter
{
    class BuildFormYesNoCancel
    {
        private IViewFormYesNoCancel _view4;
        public object View4 { get { return _view4; } }

        private TessPresenter _mainPresenter;
        private FormYesNoCancel _formObject;
        private string _message;
        public FormYesNoCancel FormYesNoCancel { get { return this._formObject; } private set { this._formObject = value; } }



        public BuildFormYesNoCancel(TessPresenter presenter,string msg)
        {
            var view = new FormYesNoCancel();
            this._view4 = view;
            this._mainPresenter = presenter;
            _formObject = (FormYesNoCancel)this._view4.Form4;
            this._message = msg;
            SetFormProps();
            AddControls();
            _mainPresenter.ViewForm4 = this._view4;
        }

        private void SetFormProps()
        {
            _formObject.Width = 500;
            _formObject.Height = 200;
            _formObject.AutoSize = false;
            _formObject.FormBorderStyle = FormBorderStyle.FixedDialog;
            _formObject.Text = "Was möchten Sie tun?";
        }

        private void AddControls()
        {
            var fp = new FlowLayoutPanel();
            
            var lblMsg = new Label();
            SetLblProps(lblMsg,_formObject);
            _view4.BtnYes = new Button();
            SetBtnProps(_formObject.BtnYes,"Anzeigen",_formObject);
            _view4.BtnNo = new Button();
            SetBtnProps(_formObject.BtnNo, "Kontakt erstellen", _formObject);
            _view4.BtnCancel = new Button();
            SetBtnProps(_formObject.BtnCancel, "verwerfen", _formObject);
            _formObject.Controls.Add(lblMsg);
            fp.Controls.Add(_view4.BtnYes);
            fp.Controls.Add(_view4.BtnNo);
            fp.Controls.Add(_view4.BtnCancel);
            _formObject.Controls.Add(fp);
            SetFpProps(fp, _formObject);
        }

        private void SetFpProps(FlowLayoutPanel flp,Form parent)
        {    
            flp.FlowDirection = FlowDirection.LeftToRight;
            flp.Margin = new Padding(70, 10, 70, 10);
            flp.AutoSize = true;
            flp.Location = new Point(parent.Width / 2 -flp.Width/2, 100);

        }

        private void SetBtnProps(Button btn, string text, Form parent)
        {
            btn.Text = text;
            btn.AutoSize = true;
        }

        private void SetLblProps(Label lbl, Form parent)
        {
            lbl.AutoSize = true;
            lbl.Text = _message;
            lbl.Location=new Point(25, Convert.ToInt32(parent.Height/3-lbl.Height/2));

        }
    }
}
