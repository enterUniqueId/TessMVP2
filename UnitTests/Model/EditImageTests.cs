using Microsoft.VisualStudio.TestTools.UnitTesting;
using TessMVP2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Moq;
using TessMVP2.Model.Interfaces;
using System.Drawing;


namespace TessMVP2.Model.Tests
{
    [TestClass()]
    public class EditImageTests:IImgFileSave
    {
        public string EncodeAndSave(string path, Bitmap bmp)
        {
            return Directory.GetCurrentDirectory() + @"\temp\testImgGS.jpg";
        }

        [TestMethod()]
        public void ImgBWTest()
        {
            //arrange
            var bmp = new Bitmap(30, 30);
            var mockFilesave = new Mock<IImgFileSave>();
            string currentDir = Directory.GetCurrentDirectory();
            string filepathFrom = Directory.GetCurrentDirectory() + @"\temp\testImg.jpg";
            //action
            var editImg = new EditImage();
            editImg.ImgBW(filepathFrom);
            //assert
            Assert.AreEqual(currentDir + @"\temp\testImgGS.jpg", editImg.NewFilepath);
        }
    }
}