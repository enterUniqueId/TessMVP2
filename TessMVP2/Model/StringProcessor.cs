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

        public StringProcessor(TessMainModel parentModel)
        {
            this._mainModel = parentModel;
            this._ocrResult = _mainModel.OcrResult;
        }
        public Dictionary<string, List<string>> ResDict { get { return this._resDict; } }
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
            _specificStrings = this._ocrResult.Split('\n').ToList();
            //leere array-felder entfernen
            while (_specificStrings.Contains("") || _specificStrings.Contains(" "))
            {
                _specificStrings.Remove("");
                _specificStrings.Remove(" ");
            }

            for (int i = 0; i < _specificStrings.Count; i++)
            {
                //strings korrigieren und reinigen
                //œ
                _specificStrings[i] = _specificStrings[i].Replace("æ", "es");
                _specificStrings[i] = _specificStrings[i].Replace(":", "");
                _specificStrings[i] = _specificStrings[i].Replace(";", "");
                foreach (KeyValuePair<string, string> kvp in emailReplaceList)
                {
                    _specificStrings[i] = _specificStrings[i].Replace(kvp.Key, kvp.Value);
                }
            }
        }

        public void SearchKeyWords()
        {
            _resDict = new Dictionary<string, List<string>>();
            foreach (var kvp in Dict.SearchDict)
            {
                //Felder anlegen
                _resDict.Add(kvp.Key, new List<string>());

                //erkannte strings nach Schlüsselworten durchsuchen
                foreach (string searchValue in kvp.Value)
                {
                    for (int j = 0; j < _specificStrings.Count; j++)
                    {
                        if (_specificStrings[j].ToLower().Contains(searchValue))
                        {
                            if (kvp.Key != "Inet" && kvp.Key != "Position")
                                //bezeichnung aus ergebnisstrin entfernen
                                _specificStrings[j] = _specificStrings[j].ToLower().Replace(searchValue, "");
                            _resDict[kvp.Key].Add(_specificStrings[j]);
                            // MessageBox.Show(sr + " <-kartenstring  " + kvp.Key);
                        }
                    }
                }
            }
        }

        //Feldzuordnung
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
                        {
                            //mindestens 8 Stellen entspricht 3 Stellen nach Vorwahl
                            //wenn ergebnisse im array sind, bleiben sie dort, werden aber noch gereinigt
                            if (kvp.Value.Count > 0)  //treffer aus dictvergleich
                            {
                                var tel = new Regex(@"^(\+\d{2}|0\d{4})(\d ?)+");
                                string replacePattern = @"([^\d\+]+)";
                                for (int i = kvp.Value.Count - 1; i >= 0; i--)
                                {
                                    kvp.Value[i] = Regex.Replace(kvp.Value[i], replacePattern, "");
                                }
                                if (!CheckRegEx(tel, kvp.Key, kvp.Value))
                                {

                                }
                            }
                            else //kein Treffer aus dictVergleich
                            {

                                var telsearch = new Regex(@"((^\+\d{2}).*|(^01[5-7]\d).*)");
                                //var telsearch = new Regex(@"^(\+\d{2}|0\d{4}|\+\d{2} ?\(0\) ?)(\d{3} ?)+");
                                if (CheckRegEx(telsearch, kvp.Key, _specificStrings) && _resDict["Telefon-Nummer"].Count > 1)
                                {
                                    //bei mehreren Treffern
                                    var mobil = new Regex(@"^(01[5-7])(\d ?\-?)+|^(\+\d{2} ?\(0\) ?)(1[5-7]\d)( ?\/?\-?\d)+");
                                    if (CheckRegEx(mobil, "Mobil-Nummer", _resDict["Telefon-Nummer"]))
                                    {
                                        _resDict["Telefon-Nummer"].Remove(_resDict["Mobil-Nummer"][0]);
                                    }
                                    else
                                    {
                                        _resDict["Telefon-Nummer2"].Add(_resDict["Telefon-Nummer"][0]);
                                        _resDict["Telefon-Nummer"].RemoveAt(0);
                                    }
                                }
                                //kein Treffer
                                else if (_resDict["Telefon-Nummer"].Count < 1)
                                {

                                }
                            }
                            break;
                        }
                    case "inet":  //
                        {
                            //foreach (var vals in kvp.Value)
                            for (int i = kvp.Value.Count - 1; i >= 0; i--)
                                if (kvp.Value[i].Contains("internet"))
                                {
                                    kvp.Value[i]=kvp.Value[i].Replace("internet", "");
                                    kvp.Value[i]=kvp.Value[i].Replace(": ", "");
                                }
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
                                string inetReplacePattern = @"^(.*w{3}\.|.*https?:\/{2})";
                                if (_resDict["Inet"].Count >= 0 && (CrossCompare("Inet", kvp.Value, inetReplacePattern) >= 0))
                                {
                                    //failed bei subdomains
                                    inetReplacePattern = @"(\.[a-z]+)";
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
                                    if (_resDict["Email"].Count >= 0 && CrossCompare("Email", kvp.Value, emailReplacePattern) >= 0)
                                    {
                                        emailReplacePattern = @"(\.[a-z]+)$";
                                        int res = CrossCompare("Email", kvp.Value, emailReplacePattern);
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
                                //orientierung an plz; dann plz+rest
                                var city = new Regex(@"(\d{5})( ?[A-ZÄÖÜ][a-zäöü]+)([\-\/ ])?([A-ZÖÜÄ])?([a-zöüä]+)?");
                                if (CheckRegEx(city, kvp.Key, _specificStrings))
                                {
                                    var plz = new Regex(@"(\d{5})");
                                    CheckRegEx(plz,"Postleitzahl", kvp.Value);

                                }
                                else
                                {
                                    //negative lookahead => sucht nach nach 5-stelliger Nummer+ws, welche nicht durch andere nummer gefolgt werden darf
                                    city = new Regex(@"(?<![\d|\-])(\d{5})(?![\d])");
                                    //FillStreetCity = new Regex(@" ");
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
            GetName();
        }

        //muss extra wegen der frühen Stellung im Resdict
        private void GetName()
        {

            string pattern = @"\@.*";
            if (_resDict["Email"].Count >= 1)
            {
                //hinteren Teil der email Adresse entfernen
                string name = Regex.Replace(_resDict["Email"][0], pattern, "");
                //alles bis zum Punkt entfernen(meistens vorname oder abk. vorname);
                string name2 = Regex.Replace(name, @".*\.", "");
                name = name.Replace(".", " ");
                foreach (var sr in _specificStrings)
                {
                    if (sr.ToLower().Contains(name.ToLower()) && !sr.Contains("@"))
                    {
                        name = sr;
                        break;
                    }
                    else if (sr.ToLower().Contains(name2.ToLower()) && !sr.Contains("@"))
                    {
                        name = sr;
                        break;
                    }
                }
                //mindestens der Treffer aus der email wird übernommen
                _resDict["Name"].Add(name);
            }
            // wenn keine email vorhanden ist/gefunden wurde, versuchen über die Position auf der Visitenkarte den Namen zu finden
            //oft steht der Name über der "Position" in der Firma
            else
            {
                //position-1 über "Position"
                if (_resDict["Position"].Count >= 1)
                {
                    int i;
                    for (i = 0; i < _specificStrings.Count; i++)
                    {
                        if (_specificStrings[i].ToLower().Contains(_resDict["Position"][0].ToLower()))
                            break;
                    }
                    if (_resDict["Name"].Count > 1 && _specificStrings[i - 1].ToLower().Contains(_resDict["Name"][0]))
                    {
                        _resDict["Name"].Add(_specificStrings[i - 1]);
                    }
                }
            }
        }

        private void FillStreetCity()
        {
            //patter wie Asdfs 45 (/3 oder -3)// max vierstellige hausnummer mit einstelligem \d Zusatz
            var street = new Regex(@"([A-ZÖÄÜ][a-züäöß\- ]+\.? )(\d{1,4})([\/|\-]\d)?");
            var plz = new Regex(@"(\d{5})");
            var ort = new Regex(@"([a-zA-ZäöüÖÄÜß])([ \w\-]+)$");
            CheckRegEx(plz, "Postleitzahl", _resDict["Ort"]);
            CheckRegEx(ort, "Ort", _resDict["Ort"]);
            CheckRegEx(street, "Strasse", _resDict["Ort"]);
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
                    stringsToCompare[i] = Regex.Replace(stringsToCompare[i], replacePattern, "");
                    if (sr.Contains(stringsToCompare[i]))
                    {
                        
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
                    //bei vorherigen/mehreren Treffern
                    if (!didMatch && !DoesExist(match.Value))
                        _resDict[field].Clear();
                    _resDict[field].Add(match.Value);
                    didMatch = true;

                    //MessageBox.Show(match.Value);
                }
            }
            return didMatch;
        }

        private bool DoesExist(string match)
        {
            bool exists = false;
            foreach (var kvp in _resDict)
            {
                if (kvp.Value.Count > 0 && kvp.Value[0] == match)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }


    }//class end
}  //namespace end
