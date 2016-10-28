using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TessMVP2.Model.Interfaces
{
    public interface IImgFileSave
    {
        string EncodeAndSave(string path, Bitmap bmp);
    }
}
