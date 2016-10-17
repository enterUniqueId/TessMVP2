using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;

namespace TessMVP2.Model
{
    public class OutlookWork : TessMainModel
    {
        private Dictionary<string, string> _resultDict;
        private Dictionary<string, string> _hits;
        private List<Dictionary<string, string>> _outlookContacts;
        private int _currentContact;
        private string _entryID;
        public string EntryID { get { return this._entryID; } set { this._entryID = value; } }
        public Dictionary<string, string> ResultDict { get { return this._resultDict; } }
        public List<Dictionary<string, string>> OutlookContacts { get { return this._outlookContacts; } set { this._outlookContacts = value; } }
        public Dictionary<string,string> Hits { get { return this._hits; } private set { this._hits = value; } }
        public int CurrentContact { get { return this._currentContact; } }

        public delegate void DuplicateHitHandler(object sender, EventArgs e);
        public event DuplicateHitHandler DuplicateHit;


        public OutlookWork(Dictionary<string, string> inputResults, IMyPresenterOutlookCallbacks callback)
        {
            this._resultDict = new Dictionary<string, string>();
            this._resultDict = inputResults;
            this.Hits = new Dictionary<string, string>();
            Attach(callback);
            NormalizeResultDict();
        }
        private void NormalizeResultDict()
        {
            var outlookApplication = new ApplicationClass();
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            MAPIFolder contacts = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            ContactItem contact = outlookApplication.CreateItem(OlItemType.olContactItem) as ContactItem;
            var td = BuildOlDict(contact);
            foreach (var kvp in _resultDict)
            {
                td[kvp.Key] = kvp.Value;
            }
            td["Homepage"] = _resultDict["Inet"];
            _resultDict = td;
        }

        public void GetContacts()
        {
            OutlookContacts = new List<Dictionary<string, string>>();
            var outlookApplication = new ApplicationClass();  //outlook interop einbetten false; wie bei WIA
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            MAPIFolder contacts = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            foreach (ContactItem cont in contacts.Items)
            {
                OutlookContacts.Add(BuildOlDict(cont));
            }
            CheckContact();
        }

        public void CreateContact()
        {
            bool hasCustomProps = false;
            var outlookApplication = new ApplicationClass();
            var customFields = new Dictionary<string, string>();
            //################ evtl. noch implementieren
            //MAPIFolder oContactsFolder = mapiNamespace.PickFolder();###############
            ContactItem contact = outlookApplication.CreateItem(OlItemType.olContactItem) as ContactItem;

            //zuordnung der ergebnisse zu den outlook contacts feldern
            foreach (var kvp in _resultDict)
            {
                switch (kvp.Key.ToLower())
                {
                    case "name":
                        contact.FullName = kvp.Value;
                        break;
                    case "fax-nummer":
                        contact.BusinessFaxNumber = kvp.Value;
                        break;
                    case "telefon-nummer":
                        contact.BusinessTelephoneNumber = kvp.Value;
                        break;
                    case "telefon-nummer2":
                        contact.Business2TelephoneNumber = kvp.Value;
                        break;
                    case "mobil-nummer":
                        contact.MobileTelephoneNumber = kvp.Value;
                        break;
                    case "email":
                        contact.Email1Address = kvp.Value;
                        break;
                    case "email2":
                        contact.Email2Address = kvp.Value;
                        break;
                    case "email3":
                        contact.Email3Address = kvp.Value;
                        break;
                    case "strasse":
                        contact.BusinessAddressStreet = kvp.Value;
                        break;
                    case "ort":
                        contact.BusinessAddressCity = kvp.Value;
                        break;
                    case "postleitzahl":
                        contact.BusinessAddressPostalCode = kvp.Value;
                        break;
                    case "postfach":
                        contact.BusinessAddressPostOfficeBox = kvp.Value;
                        break;
                    case "firma":
                        contact.CompanyName = kvp.Value;
                        break;
                    case "position":
                        contact.JobTitle = kvp.Value;
                        break;
                    case "homepage":
                        contact.BusinessHomePage = kvp.Value;
                        break;
                    default:
                        hasCustomProps = true;
                        customFields.Add(kvp.Key, kvp.Value);
                        break;
                }//end switch
            }//end foreach
            contact.Save();
            if (hasCustomProps)
                CreateCustomFields(contact.EntryID, customFields);
        }

        public void CreateCustomFields(string id, Dictionary<string, string> fields)
        {
            var outlookApplication = new ApplicationClass();
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            MAPIFolder folder = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            foreach (ContactItem contact in folder.Items)
            {
                if (contact.EntryID == id)
                {
                    foreach (var kvp in fields)
                    {
                        contact.UserProperties.Add(kvp.Key, OlUserPropertyType.olText);
                        contact.UserProperties[kvp.Key].Value = kvp.Value;
                    }
                    contact.Save();
                    break;
                }
            }

        }

        public void UpdateExistingContact(Dictionary<string, string> someFields)
        {
            var outlookApplication = new ApplicationClass();
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            MAPIFolder folder = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            ContactItem dummy = outlookApplication.CreateItem(OlItemType.olContactItem) as ContactItem;
            var fields = TranslateKeys(someFields);

            foreach (ContactItem contact in folder.Items)
            {
                if (contact.EntryID == fields.ElementAt(fields.Count - 1).Value)
                {
                    foreach (var kvp in fields)
                    {
                        foreach (ItemProperty prop in contact.ItemProperties)
                        {
                            if (prop.Name == kvp.Key && prop.Name != "EntryID")
                            {
                                prop.Value = kvp.Value;
                            }
                        }
                    }
                    contact.Save();
                    break;
                }
            }
        }

        private Dictionary<string, string> TranslateKeys(Dictionary<string, string> oldDict)
        {
            var newDict = new Dictionary<string, string>();
            for (int i = 0; i < oldDict.Count; i++)
                foreach (var kvp in Dict.TranslateDict)
                {
                    if (kvp.Value == oldDict.ElementAt(i).Key)
                    {
                        newDict.Add(kvp.Key, oldDict.ElementAt(i).Value);
                        break;
                    }
                }
            return newDict;
        }

        private void CheckContact()
        {
            bool hit = false;
            bool hitonce = false;
            for (int i = 0; i < OutlookContacts.Count; i++)
            {
                hit = false;
                foreach (var oprop in OutlookContacts[i])
                {
                    foreach (var kvp in _resultDict)
                    {
                        if (kvp.Value == oprop.Value && (oprop.Value != null && kvp.Value != null))
                        {
                            //nicht eindeutige Merkmale werden nicht herangezogen
                            if (kvp.Key != "Homepage" || kvp.Key != "Firma" || kvp.Key != "Position" || kvp.Key != "Postleitzahl" || 
                                kvp.Key != "Fax" || kvp.Key != "Ort" || kvp.Key != "Strasse")
                            {
                                _hits.Add(kvp.Key, kvp.Value);
                                hit = true;
                                hitonce = true;
                            }
                        }
                    }
                }
                if (hit)
                {
                    //Evalhits(hits, i, OutlookContacts[i]);
                    this._entryID = OutlookContacts[i]["EntryID"];
                    _currentContact = i;
                    if (this.DuplicateHit != null)
                        this.DuplicateHit(this, EventArgs.Empty);
                }
                _hits.Clear();
            }
            DuplicateHit = null;
            if (!hitonce)
            {
                CreateContact();
            }
        }


        private void Attach(IMyPresenterOutlookCallbacks callback)
        {
            DuplicateHit += (sender, e) => callback.OnRedundantEntryFound();
        }

        private Dictionary<string, string> BuildOlDict(ContactItem contact)
        {

            var dict = new Dictionary<string, string>(){
                                                { "Name", contact.FullName }, {"Telefon-Nummer", contact.BusinessTelephoneNumber }, {"Telefon-Nummer2",contact.Business2TelephoneNumber },
                                                { "Mobil-Nummer",contact.MobileTelephoneNumber }, {"Fax-Nummer",contact.BusinessFaxNumber},
                                                { "Strasse", contact.BusinessAddressStreet }, { "Postleitzahl", contact.BusinessAddressPostalCode},
                                                { "Ort", contact.BusinessAddressCity}, { "Postfach", contact.BusinessAddressPostOfficeBox },
                                                { "Position", contact.JobTitle },{ "Homepage", contact.BusinessHomePage },{ "Firma", contact.CompanyName},
                                                { "Email", contact.Email1Address }, { "Email2", contact.Email2Address }, { "Email3", contact.Email3Address },
                                                { "EntryID", contact.EntryID }
            };
            return dict;
        }
    }
}
