using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace TessMVP2.Model
{
    public class ProcessUserResults
    {
        private Control _clist;
        public Control Clist { get { return this._clist; } set { this._clist = value; } }
        private Dictionary<string, string> _resDict;
        public Dictionary<string, string> ResDict { get { return this._resDict; } private set { this._resDict = value; } }

        public ProcessUserResults(Control contList)
        {
            this._clist = contList;
            _resDict = new Dictionary<string, string>();
        }

        public List<Control> getControls(Control cont, List<Control> clist = null)
        {
            if (clist == null)
                clist = new List<Control>();
            var controls = cont.Controls.Cast<Control>();
            foreach (Control c in controls)
            {
                if (c.HasChildren)
                    getControls(c, clist);
                clist.Add(c);
            }
            return clist;
        }

        public void testclist(List<Control> clist)
        {
            string testout = "";
            foreach (Control c in clist)
            {
                testout += c.ToString() + "\n";
            }
            MessageBox.Show(testout);
        }

        public void GetInputs()
        {
            var clist = getControls(_clist);
            //testclist(clist);
            string k;
            foreach (Control c in clist)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    k = (c as TextBox).Text;
                    if (k != "")
                    {
                        _resDict.Add(c.Name.Substring(4), k);

                    }
                }
                else if (c.GetType() == typeof(RichTextBox))
                {
                    var rtbText = (c as RichTextBox).Text;
                    if (rtbText != "")
                    {
                        var fields = new List<string>();
                        fields = rtbText.Split('\n').ToList();

                        foreach (string sr in fields)
                        {
                            string[] kvp = sr.Split(':');
                            _resDict.Add(kvp[0], kvp[1]);
                        }
                    }
                }
            }
            //test();
        }

        private void test()
        {
            string outp = "";
            foreach (var kvp in _resDict)
            {
                outp += kvp.Key + "=>" + kvp.Value + "\n";
            }
            MessageBox.Show(outp);
        }

    }
}
