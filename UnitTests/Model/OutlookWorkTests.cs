﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using TessMVP2.Presenter.Interfaces;
using System.Threading;
using Moq;


namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class OutlookWorkTests : IMyPresenterOutlookCallbacks
    {
        private bool _isRedundand;
        private ManualResetEventSlim _me;
        private MAPIFolder _contacts;
        private OutlookWork _outlookObj;
        private Dictionary<string, string> _testDict;
        private delegate bool Predicate(string y, string x);

        public OutlookWorkTests()
        {
            var dict = new Dictionary<string, string>();
            _me = new ManualResetEventSlim(false);
            var outlookApplication = new ApplicationClass();
            NameSpace mapiNamespace = outlookApplication.GetNamespace("MAPI");
            _contacts = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            _testDict = CreateTestDict();
            //StandardObjekt für insUpDel-Funktionen etc.
            _outlookObj = new OutlookWork(dict, this);
        }

        private bool FindContact(string entryID)
        {
            foreach (ContactItem contact in _contacts.Items)
            {
                if (contact.EntryID == entryID)
                    return true;
            }
            return false;
        }

        [TestMethod()]
        public void OutlookWorkTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetContactsTest()
        {
            _isRedundand = false;

            //bereits vorhandene Kontaktdaten generieren
            var contact = _contacts.Items[1] as ContactItem;
            var dict = new Dictionary<string, string>();
            foreach (ItemProperty prop in contact.ItemProperties)
                dict.Add(prop.Name, prop.Value.ToString());
            var outlookObj = new OutlookWork(dict, this);
            //auf event warten
            outlookObj.DuplicateHit += (sender, e) => OnRedundantEntryFound();
            outlookObj.GetContacts();
            _me.Wait(250);
            bool a = _isRedundand;
            _isRedundand = true;
            //zweite Möglichkeit der Funktion testen
            outlookObj = new OutlookWork(_testDict, this);
            outlookObj.NoDuplicatesFound += (sender, e) => OnNoDuplicatesFound();
            outlookObj.GetContacts();
            _me.Wait(250);
            bool b = !_isRedundand;
            Assert.IsTrue(a && b);
        }

        [TestMethod()]
        public void DeleteContactTest()
        {
            var createdContact = _outlookObj.CreateContact(_testDict);
            bool a = (FindContact(createdContact.EntryID));
            string sr = createdContact.EntryID;
            createdContact.Delete();
            Assert.IsTrue(!FindContact(sr) && a);

        }

        [TestMethod()]
        public void GetAllContactsTest()
        {
            var contactItems = _outlookObj.GetAllContacts();
            Assert.IsNotNull(contactItems);
        }

        [TestMethod()]
        public void CreateContactTest()
        {
            var ns = new Mock<NameSpace>();
            var folder = new Mock<MAPIFolder>();
            var contact = new Mock<ContactItem>();
            ns.Setup(r => r.GetDefaultFolder(OlDefaultFolders.olPublicFoldersAllPublicFolders).Equals(folder));
            folder.Setup(r => r.Items).Equals(contact);


            //contact.Setup(r => r.Save(It.IsAny<ContactItem>() )).ret






            //var createdContact = _outlookObj.CreateContact(_testDict);
            //Assert.IsTrue(FindContact(createdContact.EntryID));
            //createdContact.Delete();
        }

        [TestMethod()]
        public void CreateCustomFieldsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateExistingContactTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetContactItemFromIDTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void BuildLabelFromContactTest()
        {
            Assert.Fail();
        }

        public void OnRedundantEntryFound()
        {
            _isRedundand = true;
            _me.Set();
        }

        public void OnNoDuplicatesFound()
        {
            _isRedundand = false;
            _me.Set();
        }


        private Dictionary<string, string> CreateTestDict()
        {
            var dict = new Dictionary<string, string>()
            {
                { "Name","UnitTest" },
                { "Telefon-Nummer","" },
                { "Telefon-Nummer2","" },
                { "Mobil-Nummer","" },
                { "Fax-Nummer",""},
                { "Strasse","" },
                { "Postleitzahl",""},
                { "Ort",""},
                { "Postfach",""},
                { "Position","" },
                { "Inet","" },
                { "Firma",""},
                { "Email",""},
                { "Email2","" },
                { "Email3","" }
        };
            return dict;
        }
    }
}