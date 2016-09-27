using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;

namespace TessMVP2.Model.Interfaces
{
    interface IMyModel:IModel<IMyPresenterModelCallbacks>
    {
        string OcrResult { get;}
        Dictionary<string, List<string>> StringResult { get; }
        string ImgPath { get; set; }
        void Start(IMyPresenterModelCallbacks callbacks);
        OutlookWork OlWork { get; set; }
    }
}
