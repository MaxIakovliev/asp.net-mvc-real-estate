using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlParser.Infrastructure;
using NUnit.Framework;

namespace TestHtmlParser.Infrastructure
{
    [TestFixture]
    class TestFileAvailability
    {
        private string successPath = AppDomain.CurrentDomain.BaseDirectory + "\\FilesCheck";
        private string failPath = AppDomain.CurrentDomain.BaseDirectory + "\\failDir";
        private FileAvailability fileChecker = new FileAvailability();

        [Test]
        public void TestFileExistSuccess()
        {           
            string errMsg=string.Empty;
            bool result = fileChecker.CheckFilePaths(successPath, out errMsg);
            Console.WriteLine(errMsg);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestFileExistSuccessOutMsg()
        {
            string errMsg = string.Empty;
            bool result = fileChecker.CheckFilePaths(successPath, out errMsg);
            Console.WriteLine(errMsg);
            Assert.AreEqual(string.Empty, errMsg);
        }

        [Test]
        public void TestFileExistFail()
        {
            string errMsg = string.Empty;
            bool result = fileChecker.CheckFilePaths(failPath, out errMsg);
            Console.WriteLine(errMsg);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void TestGetFileCacheHash()
        {
            string result=this.fileChecker.GetFileCacheHash();
            Assert.AreEqual(successPath+"\\CacheHash.txt", result);
        }

        [Test]
        public void TestGetStopWordsFile()
        {
            string result = this.fileChecker.GetStopWordsFile();
            Assert.AreEqual(successPath+"\\stopWords.txt", result);
        }

        [Test]
        public void TestGetLogLinksFile()
        {
            string result = this.fileChecker.GetLogLinksFile();
            Assert.AreEqual(successPath+"\\logLinks.txt", result);
        }
    }
}
