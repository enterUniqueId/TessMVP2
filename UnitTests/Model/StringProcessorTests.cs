using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Presenter.Interfaces;
namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class StringProcessorTests
    {
        private string _ocrResult;
        private List<string> _ocrStrings;
        private Dictionary<string, List<string>> _dict;

        [TestMethod()]
        public void Initialize()
        {
            _ocrResult = "Markus Mustermann -   Musterstraße 39  ; - 64889 Musterstadt   -  Tel.:+49(0)345 -8899-0003  -  Fax:+49(0)345 -889-0001";
            _ocrStrings = new List<string>()
            {
                "Markus Mustermann",
                "Musterstraße 39",
                "64889 Musterstadt",
                "Tel.:+49(0)345 -889-0003",
                "Fax:+49(0)345 - 889 - 0001"
            };
        }

        [TestMethod()]
        public void StringProcessorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TransformResDictTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchKeyWordsTest()
        {
            Initialize();
            var stringprocessor = new StringProcessor(_ocrResult);
            var ResDict = stringprocessor.SearchKeyWords(_ocrStrings);
            Assert.IsTrue(ResDict.Count == 16);
            Assert.IsTrue(ResDict["Fax-Nummer"].Count == 1);
            Assert.IsTrue(ResDict["Telefon-Nummer"].Count == 1);
            Assert.IsTrue(ResDict["Ort"].Count == 1);
        }

        [TestMethod()]
        public void GetFieldTest()
        {
            Initialize();
            var stringprocessor = new StringProcessor(_ocrResult);
            var dict = new Dictionary<string, List<string>>()
            {
                {"Name", new List<string> {"" } },
                {"Telefon-Nummer", new List<string> { "+49(0)345 -889-0003" } },
                {"Telefon-Nummer2", new List<string> {"" } },
                {"Mobil-Nummer", new List<string> {"" } },
                {"Fax-Nummer", new List<string> { "+49(0)345 -889-0001" } },
                {"Strasse", new List<string> { "Musterstraße 39" } },
                {"Postleitzahl", new List<string> {"" } },
                {"Ort", new List<string> { "64889 Musterstadt" } },
                {"Postfach", new List<string> {"" } },
                {"Position", new List<string> {"" } },
                {"Inet", new List<string> {"" } },
                {"Firma", new List<string> {"" } },
                {"Email", new List<string> {"" } },
                {"Email2", new List<string> {""} },
                {"Email3", new List<string> {""} },
                {"EntryID", new List<string> {""} }

            };
            var resDict = stringprocessor.GetField(dict, _ocrStrings);

            Assert.IsTrue(resDict.Count == 16);
        }

        [TestMethod()]
        public void CheckRegExTest()
        {
            Assert.Fail();
        }
    }
}