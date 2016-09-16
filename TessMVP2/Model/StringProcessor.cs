using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Controller.Interfaces;
using TessMVP2.Model.Interfaces;

namespace TessMVP2.Model
{
    class StringProcessor
    {

        private List<string> _specificStrings;
        private TessMainModel _mainModel;
        private string _ocrResult { get; set; }


        public StringProcessor(TessMainModel parentModel)
        {
            this._mainModel = parentModel;
            this._ocrResult = _mainModel.OcrResult;
        }

        public void Start()
        {
            splitString();
        }

        private void splitString()
        {
            _specificStrings = this._ocrResult.Split('\n').ToList();

            while (_specificStrings.Contains("") || _specificStrings.Contains(" "))
            {
                _specificStrings.Remove("");
                _specificStrings.Remove(" ");
            }

            
                
            
        }   
    }
}
