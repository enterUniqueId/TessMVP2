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
                {"Telefon-Nummer", new List<string> {"tel","fon","phone" } },
                {"Telefon-Nummer2", new List<string> {"|dummy|" } },
                {"Mobil-Nummer", new List<string> {"mobile","mobil" } },
                {"Fax-Nummer", new List<string> {"fax" } },
                {"Strasse", new List<string> {"weg","straße","strasse","allee","chaussee","rue","street","platz","str.","5tr." } },
                {"Postleitzahl", new List<string> {"|dummy|" } },
                {"Ort", new List<string> {"|dummy|" } },
                {"Postfach", new List<string> {"postfach" } },
                {"Position", new List<string> {"einkauf","marketing","business","personal","abteilung","leiter","department","management","consultant","logistik","developer" } },
                {"Inet", new List<string> {"www.", "http://","https://" } },
                {"Firma", new List<string> {"GmBH","AG","Co. KG","oHG","ltd","corp" } },
                {"Email", new List<string> {"e-mail","mail" } },
                {"Email2", new List<string> {"|dummy|"} },
                {"Email3", new List<string> {"|dummy|"} },
                {"EntryID", new List<string> {"|dummy|"} }
        };

        public static readonly Dictionary<string, string> TranslateDict = new Dictionary<string, string>()
         {
             {"FullName","Name" },
             {"BusinessTelephoneNumber","Telefon-Nummer" },
             {"Business2TelephoneNumber","Telefon-Nummer" },
             {"MobileTelephoneNumber", "Mobil-Nummer" },
             { "BusinessFaxNumber","Fax-Nummer"},
             { "BusinessAddressStreet", "Strasse" },
             { "BusinessAddressPostalCode", "Postleitzahl"},
             { "BusinessAddressCity", "Ort"},
             { "BusinessAddressPostOfficeBox","Postfach"},
             { "JobTitle","Position" },
             { "BusinessHomePage", "Inet" },
             {"CompanyName", "Firma"},
             { "Email1Address", "Email"},
             {  "Email2Address","Email2" },
             {  "Email3Address","Email3" },
             { "EntryID2", "EntryID" }
         };
    }
}
