using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Model
{
    public static class Dict
    {
        public static readonly Dictionary<string, List<string>> SearchDict = new Dictionary<string, List<string>>()
        {
                {"Name", new List<string> {"|dummy|" } },
                {"Fax", new List<string> {"fax" } },
                {"Telefon", new List<string> {"tel","fon","phone" } },
                {"Mobil", new List<string> {"mobile","mobil" } },
                {"E-Mail", new List<string> {"e-mail","mail" } },
                {"Strasse", new List<string> {"weg","straße","strasse","allee","chaussee","rue","street","platz","str." } },
                {"Ort", new List<string> {"|dummy|" } },
                {"Plz", new List<string> {"|dummy|" } },
                {"Postfach", new List<string> {"postfach" } },
                {"Firma", new List<string> {"gmbh","AG","cokg","oHG","ltd","corp" } },
                {"Position", new List<string> {"einkauf","marketing","business","personal","abteilung","leiter","department","management","consultant","logistik","developer" } },
                {"Inet", new List<string> {"www.", "http://","https://" } }
        };
    }
}
