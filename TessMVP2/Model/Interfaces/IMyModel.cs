using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;
using System.Windows.Forms;

namespace TessMVP2.Model.Interfaces
{
    interface IMyModel : IModel<IMyPresenterModelCallbacks>
    {
        string OcrResult { get; }
        Dictionary<string, List<string>> StringResult { get; }
        Dictionary<string, string> ResFields { get;}
        string ImgPath { get; set; }
        void Start(IMyPresenterModelCallbacks callbacks);
        OutlookWork OlWork { get; set; }
        bool CreateScanner();
        bool CleanupTempfolder();
        bool CreateOutlook(IMyPresenterOutlookCallbacks callbacks);
        bool WiaScan();
        List<DynamicControlViewModel> BuildCompareForm();
        List<Control> GetControlInput(Control cont);
        string EditImg(string pathToImg);
        FujiFolderObs CreateFSW(IMyPresenterFujiCallbacks callbacks);
    }
}
