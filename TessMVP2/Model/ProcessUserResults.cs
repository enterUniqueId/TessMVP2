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
        private Dictionary<string, List<string>> _resDict;
        public Dictionary<string, List<string>> ResDict { get { return this._resDict; } private set { this._resDict = value; } }

        public ProcessUserResults(Control contList)
        {
            this._clist = contList;
        }

        private List<Control> getControls(Control cont, List<Control> clist = null)
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

            /* foreach (var currControl in cont.Controls.All())
            {                
                test += currControl.GetType().ToString() + "  ";                
                // Apply logic to the textbox here
            }
                MessageBox.Show(test);

                mit extension*/
        }

        public void bla()
        {
            string test = "";
            var clist = getControls(_clist);
            string k;
            foreach (Control c in clist)
            {
                if(c.GetType()!=typeof(Control))
                test += c.GetType().ToString();
                //if (c.GetType() == typeof(TextBox))
                  //  k=(c as TextBox).Text;
            }
            MessageBox.Show(test);
        }

    }
}
