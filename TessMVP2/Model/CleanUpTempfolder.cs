using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace TessMVP2.Model
{
    class CleanUpTempfolder
    {
        public static void Cleanup()
        {
            var dir = Directory.GetCurrentDirectory() + "\\temp";


            var fileListByTime = Directory.GetFiles(dir).Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
            string sr = "";
            var filelist = fileListByTime.ToList();
            int i = 1;
            while (filelist.Count-i >= 10)
            {
                var filename = filelist[i];
                sr += filename.ToString() + "\n";
                File.Delete(filename.ToString());
                i++;
            }
        }
    }
}
