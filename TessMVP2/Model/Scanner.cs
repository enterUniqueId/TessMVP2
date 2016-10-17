using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIA;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TessMVP2.Model
{
    class Scanner
    {
        // Scanner only device properties (DPS)
        const string WIA_SCAN_COLOR_DEPTH = "4104";
        const string WIA_SCAN_COLOR_MODE = "6146";
        const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
        const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
        const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
        const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
        const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
        const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
        const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
        const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
        const int widthA4at300dpi = 2480;
        const int heightA4at300dpi = 3508;


        public Device Device { get; private set; }
        private WIA.CommonDialog dialog { get; set; }

        public void selectDevice()
        {
            this.dialog = new WIA.CommonDialog();
            //kann nicht eingebettet werden, deswegen WIA interop einbetten auf false
            DeviceManager deviceManager = new DeviceManagerClass();
            this.Device = null;
            try
            {
                this.Device = dialog.ShowSelectDevice(
                WiaDeviceType.ScannerDeviceType, true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Konnte keine Verbindung zu Scanner herstellen. "+ ex.Message);
            }
        }


        private static void SetA4(WIA.IProperties properties, int dpi)
        {
            int width = (int)((widthA4at300dpi / 300.0) * dpi);
            int height = (int)((heightA4at300dpi / 300.0) * dpi - 10);

            SetWIAProperty(properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, dpi);
            SetWIAProperty(properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, dpi);
            SetWIAProperty(properties, WIA_HORIZONTAL_SCAN_START_PIXEL, 0);
            SetWIAProperty(properties, WIA_VERTICAL_SCAN_START_PIXEL, 0);
            SetWIAProperty(properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, width);
            SetWIAProperty(properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, height);
            SetWIAProperty(properties, WIA_SCAN_COLOR_MODE, 1);
            SetWIAProperty(properties, WIA_SCAN_BRIGHTNESS_PERCENTS, 50);
            SetWIAProperty(properties, WIA_SCAN_CONTRAST_PERCENTS, 30);
            SetWIAProperty(properties, WIA_SCAN_COLOR_DEPTH, 24);
        }

        private static void SetWIAProperty(WIA.IProperties properties,
               object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }

        public void Scan()
        {
            ImageFile scannedImage = null;
            Item item = this.Device.Items[1];
            SetA4(item.Properties, 300);
            scannedImage = this.dialog.ShowTransfer(item, FormatID.wiaFormatJPEG, false) as ImageFile;
            string path = Environment.CurrentDirectory + "\\temp\\";
            string format = "jpg";
            if (File.Exists(path + "scan_img0." + format))
            {
                List<int> nums = new List<int>();
                var files = Directory.GetFiles(path);
                string pattern = @"(\d+\.)";
                Regex reg = new Regex(pattern);

                foreach (string file in files)
                {
                    Match m = reg.Match(file);
                    string x = m.Value;
                    x = x.Replace(".", "");
                    try
                    {
                        nums.Add(Convert.ToInt32(x));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                string counterNum = Convert.ToString(nums.Max() + 1);
                string k = path + "scan_img" + counterNum + "." + format;
                scannedImage.SaveFile(k);
            }
            else
            {
                string k = path + "scan_img0." + format;
                scannedImage.SaveFile(k);
            }
        }
    }

}
