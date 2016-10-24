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
        private List<Control> _clist;
        public List<Control> Clist { get { return this._clist; } set { this._clist = value; } }
        private Dictionary<string, string> _resDict;
        public Dictionary<string, string> ResDict { get { return this._resDict; } private set { this._resDict = value; } }

        public ProcessUserResults()
        {
            
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
            _clist = clist;
            return clist;
        }

        public void GetInputs(Control rootControl, bool isUpdate = false, int subStrIndex = 4)
        {
            var clist = new List<Control>();
            if (rootControl!=null)
            {
                clist = getControls(rootControl);
            }
            else
            {
                clist = this._clist;
            }
            //testclist(clist);
            string k;
            if (!isUpdate)
            {
                foreach (Control c in clist)
                {
                    if (c.GetType() == typeof(TextBox))
                    {
                        k = (c as TextBox).Text;
                        if (k != "")
                        {
                            _resDict.Add(c.Name.Substring(subStrIndex), k);
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
            }
            //Update
            else
            {
                foreach (Control c in clist)
                {
                    if (c.GetType() == typeof(Label))
                    {
                        k = (c as Label).Text;
                        if (k != "")
                        {
                            
                            if (k.Contains(c.Name.Substring(subStrIndex)))
                                k = k.Replace(c.Name.Substring(subStrIndex)+":", "");
                            k.Trim();
                            _resDict.Add(c.Name.Substring(subStrIndex), k);
                        }
                    }
                }
            }
        }

    }
}
