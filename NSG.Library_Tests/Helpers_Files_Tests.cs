using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using NSG.Library.Helpers;
//
namespace NSG.Library_Tests
{
    [TestClass]
    public class NSG_Library_Helper_Files_Tests
    {
        private string _path = @"App_Data\";
        //
        // Library contains:
        // string TempFileName(string prefix = "tmp", string extent = "txt")
        // void DeleteFile(string fullFilePathAndName)
        // void MakeDirectory(string directoryName)
        // byte[] FileToByteArray(string filePath)
        // void ByteArrayToFile(byte[] file, string fullFileName)
        // string FileNameCleaner(string fileName)
        //
        [TestMethod]
        public void Helpers_Files_TempFileName_Text_Test()
        {
            string _fileName = Files.TempFileName();
            Console.WriteLine(_fileName);
            Assert.AreEqual("tmp", _fileName.Substring(0, 3));
            Assert.AreEqual(".txt", Path.GetExtension( _fileName ));
        }
        //
        [TestMethod]
        public void Helpers_Files_DeleteFile_Text_Test()
        {
            // create file...
            byte[] _buffer = new byte[] { 239, 187, 191, 32, 32, 32 };
            string _fileName = _path + @"spaces.txt";
            Console.WriteLine(_fileName);
            Files.ByteArrayToFile(_buffer, _fileName);
            Assert.IsTrue(File.Exists(_fileName));
            // test DeleteFile...
            Files.DeleteFile(_fileName);
            Assert.IsFalse(File.Exists(_fileName));
        }
        //
        [TestMethod]
        public void Helpers_Files_MakeDirectory_Text_Test()
        {
            // void MakeDirectory(string directoryName)
            // create file...
            string _directoryName = _path + @"TestDir";
            Console.WriteLine(_directoryName);
            Assert.IsFalse(Directory.Exists(_directoryName));
            Files.MakeDirectory(_directoryName);
            Assert.IsTrue(Directory.Exists(_directoryName));
        }
        //
        [TestMethod]
        public void Helpers_Files_FileNameCleaner_Text_Test()
        {
            //   (less than)
            // > (greater than)
            // : (colon)
            // " (double quote)
            // / (forward slash)
            // \ (backslash)
            // | (vertical bar or pipe)
            // ? (question mark)
            // * (asterisk)<br />
            // , (comma)
            // (amperstand)
            string _fileName = @"file<>:/\|?*,&" + "\"";
            Console.WriteLine(_fileName);
            string _actual = Files.FileNameCleaner(_fileName);
            Console.WriteLine(_actual);
            Assert.AreEqual("file", _actual.Trim());
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void Helpers_Files_ByteArrayRoundTrip_Text_Test()
        {
            // BOM UTF-8  EF BB BF    239 187 191
            string _textPath = _path + "TextFile.txt";
            string _toPath = _path + "TextFile_Out.txt";
            byte[] _buffer = Files.FileToByteArray(_textPath);
            Assert.IsNotNull(_buffer);
            Files.ByteArrayToFile(_buffer, _toPath);
            Assert.IsTrue( File.Exists(_toPath) );
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void Helpers_Files_ByteArrayRoundTrip_Rtf_Test()
        {
            string _rtfPath = _path + "Test_Rtf.rtf";
            string _toPath = _path + "Test_Rtf_Out.rtf";
            byte[] _buffer = Files.FileToByteArray(_rtfPath);
            Assert.IsNotNull(_buffer);
            Files.ByteArrayToFile(_buffer, _toPath);
            Assert.IsTrue(File.Exists(_toPath));
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void Helpers_Files_ByteArrayRoundTrip_Pdf_Test()
        {
            string _pdfPath = _path + "GoodPdf.pdf";
            string _toPath = _path + "GoodPdf_Out.pdf";
            byte[] _buffer = Files.FileToByteArray(_pdfPath);
            Assert.IsNotNull(_buffer);
            Files.ByteArrayToFile(_buffer, _toPath);
            Assert.IsTrue(File.Exists(_toPath));
        }
        //
    }
}
//
