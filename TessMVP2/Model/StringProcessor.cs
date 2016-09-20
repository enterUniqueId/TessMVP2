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
            foreach (KeyValuePair<string, List<string>> kvp in _resDict)
            {
                foreach (string sr in kvp.Value)
                    MessageBox.Show("key: " + kvp.Key + "value: " + sr);
            }
        }

        private void splitString()
        {
            //liste mit gängigen ocr fehlern; zu erweitern
            var emailReplaceList = new Dictionary<string, string>() { { "—", "-" } };
            string sr;

            _specificStrings = this._ocrResult.Split('\n').ToList();

            //leere array-felder entfernen
            while (_specificStrings.Contains("") || _specificStrings.Contains(" "))
            {
                _specificStrings.Remove("");
                _specificStrings.Remove(" ");
            }

            for (int i = 0; i < _specificStrings.Count; i++)
            {
                foreach (KeyValuePair<string, string> kvp in emailReplaceList)
                {
                    sr = _specificStrings[i].Replace(kvp.Key, kvp.Value);
                    _specificStrings[i] = sr;
                }
            }
        }

        public void SearchKeyWords()
        {
            _resDict = new Dictionary<string, List<string>>();



            foreach (KeyValuePair<string, List<string>> kvp in Dict.SearchDict)
            {
                //geforderte Felder anlegen
                _resDict.Add(kvp.Key, new List<string>());

                foreach (string searchValue in kvp.Value)
                {
                    for (int j = 0; j < _specificStrings.Count; j++)
                    {
                        //für manche, speziell kurze strings (AG) sind case insensitive conditions ungeeignet
                        if (_specificStrings[j].ToLower().Contains(searchValue))
                        {
                            _resDict[kvp.Key].Add(_specificStrings[j]);
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
                            var mail = new Regex(@"([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)");
                            //wenn mit dem wörterbuchvergleich schon etwas gefunden wurde
                            if (kvp.Value.Count > 0)
                            {
                                CheckRegEx(mail, kvp.Key, kvp.Value);
                            }
                            //wenn nicht komplett mit dem spezifischen RegEx suchen
                            else
                            {
                                CheckRegEx(mail, kvp.Key, _specificStrings);
                            }
                            break;
                        }
                    case "telefon":
                    case "fax":
                    case "mobil":
                        {
                            //mindestens 8 Stellen entspricht 3 Stellen nach Vorwahl

                            if (kvp.Value.Count > 0)
                            {
                                var tel = new Regex(@"^(\+\d{2}|0\d{4})\d+");
                                string replacePattern = @"([^\d\+]+)";
                                for (int i = kvp.Value.Count - 1; i >= 0; i--)
                                {
                                    kvp.Value[i] = Regex.Replace(kvp.Value[i], replacePattern, "");
                                }
                                CheckRegEx(tel, kvp.Key, kvp.Value);
                            }
                            else
                            {
                                var telsearch = new Regex(@"^(\+\d{2}|0\d{4})\d{3}");
                                CheckRegEx(telsearch, kvp.Key, _specificStrings);
                            }
                            break;
                        }
                    case "inet":
                        {
                            var inet = new Regex(@"^(w{3}\.|https?:\/{2}).*(\.[a-z]{2,4})$");
                            if (kvp.Value.Count > 0)
                            {
                                CheckRegEx(inet, kvp.Key, kvp.Value);
                            }
                            else
                            {
                                CheckRegEx(inet, kvp.Key, _specificStrings);
                            }
                            break;
                        }
                    case "firma":
                        {
                            if (kvp.Value.Count > 0)
                            {
                                string inetReplacePattern = @"^(w{3}\.|https?:\/{2})";
                                if (_resDict["Inet"].Count >= 0 && (CrossCompare("Inet", kvp.Value, inetReplacePattern) >= 0))
                                {
                                    inetReplacePattern = @"(\.[a-z]+)$";
                                    int res = CrossCompare("Inet", kvp.Value, inetReplacePattern);
                                    if (res >= 0){
                                        string resString = _resDict["Firma"][res];
                                        _resDict["Firma"].Clear();
                                        _resDict["Firma"].Add(resString);
                                   }
                                }
                                else
                                {
                                    string emailReplacePattern = @"^(.*\@)";
                                    if (_resDict["E-Mail"].Count >= 0 && CrossCompare("E-Mail", kvp.Value, emailReplacePattern)>=0)
                                    {
                                        emailReplacePattern = @"(\.[a-z]+)$";
                                        int res = CrossCompare("E-Mail", kvp.Value, emailReplacePattern);
                                        if (res >= 0)
                                        {
                                            string resString = _resDict["Firma"][res];
                                            _resDict["Firma"].Clear();
                                            _resDict["Firma"].Add(resString);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }



            } //end switch
        }


        private int CrossCompare(string dictField, List<string> stringsToCompare, string replacePattern)
        {
            bool replaced = false;
            int i = stringsToCompare.Count - 1;
            do
            {
                foreach (string sr in _resDict[dictField])
                {
                    if (sr.Contains(stringsToCompare[i]))
                    {
                        stringsToCompare[i] = Regex.Replace(stringsToCompare[i], replacePattern, "");
                        replaced = true;
                        break;


                    }
                }
                i--;
            } while (!replaced && i >= 0);
            if (replaced)
                return i;
            else
                return -1;
        }

        public void CheckRegEx(Regex reg, string field, List<string> searchList)
        {
            Match match;
            for (int i = searchList.Count - 1; i >= 0; i--)
            {
                match = reg.Match(searchList[i]);
                if (match.Success)
                {
                    //problem, wenn nach der prüfung noch 2 matches vorhanden sind!
                    //_resDict.Remove(field);
                    //_resDict.Add(field, new List<string>() { sr });
                    _resDict[field].Clear();
                    _resDict[field].Add(match.Value);

                    //MessageBox.Show(match.Value);
                }
            }
        }


    }//class end
}  //namespace end
