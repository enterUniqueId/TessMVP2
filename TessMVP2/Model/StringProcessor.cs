using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace TessMVP2.Model
{
    class StringProcessor
    {

        private List<string> _specificStrings;
        private TessMainModel _mainModel;
        private string _ocrResult;
        private Dictionary<string, List<string>> _resDict;
        public Dictionary<string, List<string>> ResDict
        {
            get { return _resDict; } 
            set { this._resDict = value; }
        }

        public StringProcessor(TessMainModel parentModel)
        {
            this._mainModel = parentModel;
            this._ocrResult = _mainModel.OcrResult;
        }

        public void Start()
        {
            splitString();
            //GetEmail();
            SearchKeyWords();
            GetField();
        }

        private void splitString()
        {
            _specificStrings = this._ocrResult.Split('\n').ToList();

            while (_specificStrings.Contains("") || _specificStrings.Contains(" "))
            {
                _specificStrings.Remove("");
                _specificStrings.Remove(" ");
                
            }            
        }
                
        public void SearchKeyWords()
        {
            _resDict = new Dictionary<string, List<string>>();

            foreach(KeyValuePair<string, List<string>> kvp in Dict.SearchDict)
            {
                foreach(string searchValue in kvp.Value)
                {
                    for(int j=0;j<_specificStrings.Count;j++)
                    {
                        if (_specificStrings[j].ToLower().Contains(searchValue)){
                            if(!_resDict.ContainsKey(kvp.Key))
                            {
                                _resDict.Add(kvp.Key, new List<string> { _specificStrings[j] });
                            }
                            else
                            {
                                _resDict[kvp.Key].Add(_specificStrings[j]);
                            }
                           // MessageBox.Show(sr + " <-kartenstring  " + kvp.Key);
                        }
                    }
                }
            }
        }

        public void GetField()
        {
            foreach (KeyValuePair<string, List<string>> kvp in _resDict)
            {
                //Treffer aus SearchKeyWords
                switch (kvp.Key.ToLower())
                {
                    case "e-mail":
                    {
                            //bekannte Fehler in der OCR ersetzen
                            var emailReplaceList = new Dictionary<string, string>(){ { "—","-"} };
                            var mail = new Regex(@"([\w\.\-]+)@([\w\-]+)((\.(\w){2,5})+)");
                            CleanString(kvp.Value, mail,emailReplaceList,kvp.Key);
                            break;
                    }
                    case "Telefon":
                        {
                            var telReplaceList= new Dictionary<string, string>() { { "—", ""  }};
                            var tel = new Regex(@"([\+]+)@([\w\-]+)((\.(\w){2,5})+)");
                            break;
                        }
                }
            }
        }

        public void CleanString(List<string> values, Regex reg, Dictionary<string, string> replaceList, string field)
        {
            string cleanString;
            
            Match match;

            foreach (string sr in values)
            {
                foreach(KeyValuePair<string,string> kvp in replaceList)
                {
                    //wenn in Ersatzliste dann ersetzen
                    if (sr.Contains(kvp.Key))
                    {
                        cleanString = sr.Replace(kvp.Key, kvp.Value);
                        match = reg.Match(cleanString);
                       
                    }
                    else
                    {
                        cleanString = sr;
                    }
                    match = reg.Match(cleanString);
                    if (match.Success)
                    {
                        //problem, wenn nach der prüfung noch 2 matches vorhanden sind!
                        _resDict.Remove(field);
                        _resDict.Add(field, new List<string>() { cleanString });
                        MessageBox.Show(cleanString);
                    }
                }
            }
            

        }

    }//class end
}  //namespace end
