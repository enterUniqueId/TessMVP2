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
        private Dictionary<string, string> _outlookCurrentContact;
        private Dictionary<string, string> _resultDict;
        private Dictionary<string, string> _hits;
        private List<Dictionary<string, string>> _outlookContacts;
        private string _entryID;
        private ContactItem contact;
        private NameSpace _mapiNamespace;
        private MAPIFolder _contactFolder;
        private ApplicationClass _outlookApplication;
        public string EntryID { get { return this._entryID; } set { this._entryID = value; } }
        public Dictionary<string, string> ResultDict { get { return this._resultDict; } }
        public List<Dictionary<string, string>> OutlookContacts { get { return this._outlookContacts; } set { this._outlookContacts = value; } }
        public Dictionary<string, string> Hits { get { return this._hits; } private set { this._hits = value; } }
        public Dictionary<string,string> OutlookCurrentContact{ get { return _outlookCurrentContact; }}

        public delegate void DuplicateHitHandler(object sender, EventArgs e);
        public event DuplicateHitHandler DuplicateHit;
        public delegate void NoDuplicateHitHandler(object sender, EventArgs e);
        public event NoDuplicateHitHandler NoDuplicatesFound;

        public OutlookWork(Dictionary<string, string> inputResults, IMyPresenterOutlookCallbacks callback)
        {
            this._resultDict = new Dictionary<string, string>();
            this._resultDict = inputResults;
            this.Hits = new Dictionary<string, string>();
            _outlookApplication = new ApplicationClass();
            _mapiNamespace = _outlookApplication.GetNamespace("MAPI");
            _contactFolder = _mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            contact = _outlookApplication.CreateItem(OlItemType.olContactItem) as ContactItem;
            Attach(callback);
            NormalizeResultDict();
        }


        private void NormalizeResultDict()
        {
            var td = BuildOlDict(contact);
            foreach (var kvp in _resultDict)
            {
                if (kvp.Key == "Inet")
                    td["Homepage"] = _resultDict["Inet"];
                td[kvp.Key] = kvp.Value;
            }
            _resultDict = td;
        }

        public void GetContacts()
        {
            bool hit = false;
            foreach (ContactItem cont in _contactFolder.Items)
            {
               if(CheckContact(BuildOlDict(cont)))
                hit = true;
            }
            if (!hit)
            {
                var dummy = new Dictionary<string, string>() { { "name", "dummy" } };
                var contact=CreateContact(dummy);
                _outlookCurrentContact= BuildOlDict(contact);
                if (NoDuplicatesFound != null)
                    NoDuplicatesFound(this, EventArgs.Empty);
            }
        }

        public void DeleteContact(ContactItem contact)
        {
            contact.Delete();
        }

        public Items GetAllContacts()
        {
            return _contactFolder.Items;
        }

        public ContactItem CreateContact(Dictionary<string,string> contactToCreate)
        {
            bool hasCustomProps = false;
            
            var customFields = new Dictionary<string, string>();


            //zuordnung der ergebnisse zu den outlook contacts feldern
            foreach (var kvp in contactToCreate)
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
                    case "inet":
                        contact.BusinessHomePage = kvp.Value;
                        break;
                    case "entryid":
                        //
                        break;
                    default:
                        hasCustomProps = true;
                        customFields.Add(kvp.Key, kvp.Value);
                        break;
                }//end switch
            }//end foreach
            contact.Save();
            if (hasCustomProps)
                CreateCustomFields(contact, customFields);
            return contact;
        }

        public void CreateCustomFields(ContactItem contact, Dictionary<string, string> fields)
        {
            foreach (var kvp in fields)
            {
                contact.UserProperties.Add(kvp.Key, OlUserPropertyType.olText);
                contact.UserProperties[kvp.Key].Value = kvp.Value;
            }
            contact.Save();
        }

        public void UpdateExistingContact(Dictionary<string, string> someFields)
        {
            var fields = TranslateKeys(someFields);

            foreach (ContactItem contact in _contactFolder.Items)
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

        private bool CheckContact(Dictionary<string, string> outlookContact)
        {
            bool hit = false;
            for (int i = 0; i < outlookContact.Count; i++)
            {
                var kvp = outlookContact.ElementAt(i);
                if (kvp.Key != "Homepage" && kvp.Key != "Firma" && kvp.Key != "Position" && kvp.Key != "Postleitzahl" &&
                   kvp.Key != "Fax" && kvp.Key != "Ort" && kvp.Key != "Strasse" && kvp.Key != "Inet")
                {
                    if (kvp.Value == _resultDict.ElementAt(i).Value && _resultDict.ElementAt(i).Value != null && kvp.Value != null)
                    {
                        _hits.Add(kvp.Key, kvp.Value);
                        hit = true;
                    }
                }
            }
            if (hit)
            {
                this._entryID = outlookContact["EntryID"];
                _outlookCurrentContact = outlookContact;
                if (DuplicateHit != null)
                        DuplicateHit(this, EventArgs.Empty);
                _hits.Clear();
            }
            return hit;
        }


        private void Attach(IMyPresenterOutlookCallbacks callback)
        {
            DuplicateHit += (sender, e) => callback.OnRedundantEntryFound();
            NoDuplicatesFound += (sender, e) => callback.OnNoDuplicatesFound();
        }

        public ContactItem GetContactItemFromID(string entryID)
        {

            foreach(ContactItem c in _contactFolder.Items)
            {
                if (c.EntryID == entryID)
                    return c;
            }
            return null;
            //return contacts.Items.Find(String.Format("[EntryID]='{0}'",entryID)) as ContactItem;
        }

        public List<string> BuildLabelFromContact(ContactItem contact)
        {
            var list = new List<string>();
            var dict = BuildOlDict(contact);
            foreach (var kvp in dict)
                list.Add(kvp.Key + ":" + kvp.Value);
            return list;
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
