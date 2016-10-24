using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

        [TestMethod]
        public void GetResultTest()
        {
            int number = Number();
            Assert.AreEqual(number, 40);
            //Assert.
        }


        public int Number()
        {
            return 42;
        }
    }
}
