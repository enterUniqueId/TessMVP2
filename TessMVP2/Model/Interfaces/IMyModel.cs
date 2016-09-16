using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Controller.Interfaces;

namespace TessMVP2.Model.Interfaces
{
    interface IMyModel:IModel<IMyPresenterModelCallbacks>
    {
        string OcrResult { get;}
        string ImgPath { get; set; }
        void Start(IMyPresenterModelCallbacks callbacks);
    }
}
