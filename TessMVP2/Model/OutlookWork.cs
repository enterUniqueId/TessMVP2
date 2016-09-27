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
    public class OutlookWork:IOutlookModel
    {

        private Dictionary<string, string> _resultDict;
        public Dictionary<string, string> ResultDict { get { return this._resultDict; } }
        private List<List<string>> _outlookContacts;
        public List<List<string>> OutlookContacts { get { return this._outlookContacts; } set { this._outlookContacts = value; } }
        public List<string> Hits { get; private set; }
        public int CurrentContact { get; private set; }

        public delegate void DuplicateHitHandler(object sender, EventArgs e);
        public event DuplicateHitHandler DuplicateHit;



        public OutlookWork(Dictionary<string, string> inputResults)
        {
            this._resultDict = new Dictionary<string, string>();
            this._resultDict = inputResults;
            this.Hits = new List<string>();
        }

        public void GetContacts()
        {
            OutlookContacts = new List<List<string>>();
            var outlookApplication = new ApplicationClass();  //outlook interop einbetten false; wie bei WIA
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            MAPIFolder contacts = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);

            foreach (ContactItem contact in contacts.Items)
            {
                string olID = contact.EntryID;
                string name = contact.FullName;
                string tel1 = contact.BusinessTelephoneNumber;
                string tel2 = contact.Business2TelephoneNumber;
                string mobilNummer = contact.MobileTelephoneNumber;
                string fax = contact.BusinessFaxNumber;
                string street = contact.BusinessAddressStreet;
                string plz = contact.BusinessAddressPostalCode;
                string ort = contact.BusinessAddressCity;
                string postfach = contact.BusinessAddressPostOfficeBox;
                string position = contact.JobTitle;
                string inetAdd = contact.BusinessHomePage;
                string firma = contact.CompanyName;
                string email = contact.Email1Address;
                string email2 = contact.Email2Address;
                string email3 = contact.Email3Address;
                OutlookContacts.Add(new List<string>() { olID, name, tel1, tel2, mobilNummer, fax, street, plz, ort, postfach, position, inetAdd, firma, email, email2, email3 });
            }
            //test();
            CheckContact();
        }

        private void test()
        {
            string contactData = "";
            foreach (List<string> li in OutlookContacts)
            {
                foreach (string sr in li)
                {
                    contactData += sr + " | ";
                }
                MessageBox.Show(contactData);
                contactData = "";
            }
        }

        public void CreateContactExample()
        {
            bool hasCustomProps = false;
            var outlookApplication = new ApplicationClass();
            var customFields = new Dictionary<string, string>();
            ContactItem contact = outlookApplication.CreateItem(OlItemType.olContactItem) as ContactItem;

            //zuordnung der ergebnisse zu den outlook contacts feldern
            foreach (var kvp in _resultDict)
            {
                switch (kvp.Key.ToLower())
                {
                    case "Name":
                        contact.FullName = kvp.Value;
                        break;
                    case "fax":
                        contact.BusinessFaxNumber = kvp.Value;
                        break;
                    case "telefon":
                        contact.BusinessTelephoneNumber = kvp.Value;
                        break;
                    case "tel2":
                        contact.Business2TelephoneNumber = kvp.Value;
                        break;
                    case "mobil":
                        contact.MobileTelephoneNumber = kvp.Value;
                        break;
                    case "e-mail":
                        contact.Email1Address = kvp.Value;
                        break;
                    case "e-mail2":
                        contact.Email2Address = kvp.Value;
                        break;
                    case "e-mail3":
                        contact.Email3Address = kvp.Value;
                        break;
                    case "strasse":
                        contact.BusinessAddressStreet = kvp.Value;
                        break;
                    case "ort":
                        contact.BusinessAddressCity = kvp.Value;
                        break;
                    case "plz":
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
            var idList = new List<string>();  //weg?
            int i = 0;
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
                idList.Add(contact.EntryID);
                i++;
            }

        }

        private void CheckContact()
        {
            bool hit = false;
            var hits = new List<string>();
            for (int i = 0; i < OutlookContacts.Count; i++)
            {
                foreach (string oprop in OutlookContacts[i])
                {
                    foreach (var kvp in _resultDict)
                    {
                        if (kvp.Value == oprop && (oprop != null && kvp.Value != null))
                        {
                            hits.Add(kvp.Value);
                            hit = true;
                        }
                    }
                }
                if (hit)
                {
                    Evalhits(hits, i, OutlookContacts[i]); // hier form bauen
                }
                hits.Clear();
                hit = false;
            }
        }

        private void Evalhits(List<string> hitlist, int contactID, List<string> oldContact)
        {
            int percent = hitlist.Count*100/_resultDict.Count;
            DialogResult result = MessageBox.Show("Der neue Kontakt stimmte zu " + percent.ToString() + "% mit Kontakt-Nr. " + contactID +
                            " (OL-ID: " + oldContact[0] + "überein.\nDatensatz anzeigen?", "Übereinstimmung",
                            MessageBoxButtons.OKCancel,MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                this.Hits = hitlist;
                this.CurrentContact = contactID;
            }
        }

        private void Start(IMyPresenterOutlookCallbacks callbacks)
        {
            DuplicateHit += (sender, e) => callbacks.OnRedundandEntryFound();
        }

        public void Attach(IMyPresenterOutlookCallbacks presenter)
        {
            throw new NotImplementedException();
        }
    }
}
