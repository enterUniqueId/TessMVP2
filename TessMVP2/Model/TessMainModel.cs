﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Tesseract;
using TessMVP2.Controller.Interfaces;
using TessMVP2.Model.Interfaces;

namespace TessMVP2.Model
{
    public class TessMainModel:IMyModel
    {

        public delegate void OcrChangedHandler(object sender, EventArgs e);
        public event OcrChangedHandler OcrResultChanged;
        public delegate void FinishedStringChangedHandler(object sender, EventArgs e);
        public event FinishedStringChangedHandler FinishedStringChanged;

        private string _imgPath;
        private TessOcr _ocr;
        private StringProcessor _stringProcessor;
        public string OcrResult { get; set; }
        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }

        }


        public TessMainModel(IMyPresenterModelCallbacks callback)
        {
            
        }


        private void constructStringProcessor(IMyPresenterModelCallbacks callback)
        {
            this._stringProcessor = new StringProcessor(this);
            this.FinishedStringChanged += (sender, e) => callback.OnStringFinished();
        }

        public void Attach(IMyPresenterModelCallbacks callback)
        {
            //dummy, da nur ein interface, ich aber die registrierungen getrennt brauche
            //interface IModel sparen?
        }

        public void Start(IMyPresenterModelCallbacks callback)
        {
            this._ocr = new TessOcr(this);
            this.OcrResultChanged+= (sender, e)=> callback.OnOcrResultChanged();
            this._ocr.Start();
            this. OcrResult = this._ocr.OcrResult;
            if (this.OcrResultChanged != null)
                this.OcrResultChanged(this, EventArgs.Empty);
            this._stringProcessor = new StringProcessor(this);
            this.FinishedStringChanged += (sender, e) => callback.OnStringFinished();
            this._stringProcessor.Start();

            if (this.FinishedStringChanged != null)
                this.FinishedStringChanged(this, EventArgs.Empty);

        }   


    }


}
