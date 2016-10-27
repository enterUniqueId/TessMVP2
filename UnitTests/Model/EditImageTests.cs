using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class EditImageTests
    {

        [TestMethod()]
        public void ImgBWTest()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filepath = currentDir+@"\temp\testImg.jpg";
            var editImg = new EditImage();
            editImg.ImgBW(filepath);
            Assert.IsNotNull(editImg.NewFilepath);
        }
    }
}