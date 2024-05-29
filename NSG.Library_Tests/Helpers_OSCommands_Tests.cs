using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using NSG.Library.Helpers;
//
namespace NSG.Library_Tests
{
    [TestClass]
    public class NSG_Library_Helper_OSCommands_Tests
    {
        //
        [TestMethod]
        public void Helpers_OSCommands_CallOperatingSystemCmd_Test()
        {
            // string CallOperatingSystemCmd(string cmdStr, string workingDirectory)
            // create file...
            byte[] _buffer = new byte[] { 239, 187, 191, 49, 10, 50, 10, 51, 10 };
            string _fileName = @".\123.txt";
            string _cmd = @"TYPE " + _fileName;
            Console.WriteLine(_fileName);
            Files.ByteArrayToFile(_buffer, _fileName);
            Assert.IsTrue(File.Exists(_fileName));
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", -1);
            Console.WriteLine(_output);
            Assert.AreEqual("1\n2\n3\n", _output);
        }
        //
        [TestMethod]
        public void Helpers_OSCommands_CallOperatingSystemFindCmd_Test()
        {
            // string CallOperatingSystemCmd(string cmdStr, string workingDirectory)
            // create file...
            string _fileName = @".\App_Data\TextFile.txt";
            string _cmd = @"find ""xyz"" " + _fileName;
            string _expected = $"\r\n---------- .\\APP_DATA\\TEXTFILE.TXT\r\n";
            Assert.IsTrue(File.Exists(_fileName));
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", 10000);
            Console.WriteLine(_output);
            Console.WriteLine(string.Join("",
                _expected.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Assert.AreEqual(_expected, _output);
        }
        //
    }
}
//
