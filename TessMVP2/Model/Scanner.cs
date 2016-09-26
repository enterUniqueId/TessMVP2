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


        private Device device { get; set; }
        private WIA.CommonDialog dialog { get; set; }

        public void selectDevice()
        {
            this.dialog = new WIA.CommonDialog();
            //kann nicht eingebettet werden, deswegen WIA interop einbetten auf false
            DeviceManager deviceManager = new DeviceManagerClass();
            this.device = null;
            try
            {
                this.device = dialog.ShowSelectDevice(
                WiaDeviceType.ScannerDeviceType, true, false);
                Scan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Konnte keine Verbindung zu Scanner herstellen. "+ ex.Message);
            }

        }

        private void SetWiaProps()
        {
            foreach (Property item in this.device.Items[1].Properties)
            {
                switch (item.PropertyID)
                {
                    case 6146: //4 is Black-white,gray is 2, color 1
                        SetProperty(item, 1);
                        break;
                    case 6147: //dots per inch/horizontal 
                        SetProperty(item, 300);
                        break;
                    case 6148: //dots per inch/vertical 
                        SetProperty(item, 300);
                        break;
                    case 6149: //x point where to start scan 
                        SetProperty(item, 0);
                        break;
                    case 6150: //y-point where to start scan 
                        SetProperty(item, 0);
                        break;
                    case 6151: //horizontal exent 
                        SetProperty(item, (int)(8.5 * 300));
                        break;
                    case 6152: //vertical extent 
                        SetProperty(item, (int)(11 * 300));
                        break;
                }
            }
        }

        private void SetProperty(Property property, int value)
        {
            IProperty x = (IProperty)property;
            Object val = value;
            x.set_Value(ref val);
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
            //SetWIAProperty(this.device.Properties, WIA_DEVICE_PROPERTY_PAGES_ID, 1);
            Item item = this.device.Items[1];
            SetA4(item.Properties, 300);
            //SetWiaProps();
            //var props = item.Properties;
            scannedImage = this.dialog.ShowTransfer(item, FormatID.wiaFormatTIFF, false) as ImageFile;
            string path = Environment.CurrentDirectory + "\\temp\\";
            string format = "bmp";
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
                    catch (Exception ex)
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
