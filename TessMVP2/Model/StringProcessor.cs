﻿using System;
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

        public StringProcessor(TessMainModel parentModel)
        {
            this._mainModel = parentModel;
            this._ocrResult = _mainModel.OcrResult;
        }
        public Dictionary<string,List<string>> ResDict { get { return this._resDict; } }
        public void Start()
        {
            splitString();
            //GetEmail();
            SearchKeyWords();
            GetField();
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
                    case "email":
                        {
                            var mail = new Regex(@"([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)");
                            //wenn mit dem wörterbuchvergleich schon etwas gefunden wurde
                            if (kvp.Value.Count > 0)
                            {
                                CheckRegEx(mail, kvp.Key, kvp.Value);
                            }
                            //wenn nicht, komplettes ocr-ergebnis mit dem spezifischen RegEx suchen
                            else
                            {
                                CheckRegEx(mail, kvp.Key, _specificStrings);
                            }
                            break;
                        }
                    case "telefon-nummer":
                    case "fax-nummer":
                    case "mobil-nummer":
                        {
                            //mindestens 8 Stellen entspricht 3 Stellen nach Vorwahl
                            //wenn ergebnisse im array sind, bleiben sie dort, werden aber noch gereinigt
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
                    case "inet":  //
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
                            {   //versuche firma aus inet addresse ziehen       
                                string inetReplacePattern = @"^(.*w{3}\.|https?:\/{2})";
                                if (_resDict["Inet"].Count >= 0 && (CrossCompare("Inet", kvp.Value, inetReplacePattern) >= 0))
                                {
                                    inetReplacePattern = @"(\.[a-z]+)$";
                                    int res = CrossCompare("Inet", kvp.Value, inetReplacePattern);
                                    if (res >= 0)
                                    {
                                        string resString = _resDict["Firma"][res];
                                        _resDict["Firma"].Clear();
                                        _resDict["Firma"].Add(resString);
                                    }
                                }
                                else
                                {
                                    //versuche firma aus email-domain zu ziehen
                                    string emailReplacePattern = @"^(.*\@)";
                                    if (_resDict["E-Mail"].Count >= 0 && CrossCompare("E-Mail", kvp.Value, emailReplacePattern) >= 0)
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
                            break;
                        }
                    case "postfach":
                        {
                            if (kvp.Value.Count > 0)
                            {
                                var pfach = new Regex(@"([\d|\d ]+)");

                                CheckRegEx(pfach, kvp.Key, kvp.Value);
                            }
                            break;
                        }
                    case "ort":
                        {

                            if (kvp.Value.Count > 0)
                            {
                                //
                            }
                            //wenn nicht komplett mit dem spezifischen RegEx suchen
                            else
                            {
                                //oft kommen straße, plz und ort in einer zeile vor
                                //evtl. mit ^(.*)(?<!\d{5}
                                var city = new Regex(@"^([\w\-üäöÜÄÖß \.]+)([\d|\d\/\d]+)([ \-| \/| \,|\|]+)?(\d{5})([ \w\-\.]+)$");
                                if (CheckRegEx(city, kvp.Key, _specificStrings))
                                {
                                    FillStreetCity();

                                }
                                else
                                {
                                    //negative lookahead => sucht nach nach 5-stelliger Nummer+ws, welche nicht durch andere nummer gefolgt werden darf
                                    city = new Regex(@"(?<![\d|\-])(\d{5})(?![\d])");
                                    foreach (string s in _specificStrings)
                                    {
                                        Match match = city.Match(s);
                                        if (match.Success)
                                        {
                                            _resDict["Ort"].Clear();
                                            _resDict["Ort"].Add(s);
                                            FillStreetCity();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            } //end switch
            getName();
        }

        //muss extra wegen der frühen Stellung im Resdict
        private void getName()
        {
            string pattern = @"\@.*";
            string name = Regex.Replace(_resDict["Email"][0], pattern, "");
            name.Replace(".", " ");
            _resDict["Name"].Add(name);
        }

        private void FillStreetCity()
        {
            var street = new Regex(@"^([\w\-üäöÜÄÖß \.]+)([\d|\d\/\d]+)");
            CheckRegEx(street, "Strasse", _resDict["Ort"]);
            var plz = new Regex(@"(\d{5})");
            CheckRegEx(plz, "Postleitzahl", _resDict["Ort"]);
            var ort = new Regex(@"([a-zA-ZäöüÖÄÜß])([ \w\-]+)$");
            CheckRegEx(ort, "Ort", _resDict["Ort"]);
            _resDict["Ort"][0] = _resDict["Ort"][0].Trim();
            _resDict["Ort"][0] = _resDict["Ort"][0].Replace("0", "o");
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
                return i + 1;
            else
                return -1;
        }

        public bool CheckRegEx(Regex reg, string field, List<string> searchList)
        {
            bool didMatch = false;
            for (int i = searchList.Count - 1; i >= 0; i--)
            {
                Match match = reg.Match(searchList[i]);
                if (match.Success)
                {
                    //problem, wenn nach der prüfung noch 2 matches vorhanden sind!
                    //_resDict.Remove(field);
                    //_resDict.Add(field, new List<string>() { sr });
                    _resDict[field].Clear();
                    _resDict[field].Add(match.Value);
                    didMatch = true;

                    //MessageBox.Show(match.Value);
                }
            }
            return didMatch;
        }


    }//class end
}  //namespace end
