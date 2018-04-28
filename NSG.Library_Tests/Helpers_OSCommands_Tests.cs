using System;
using System.IO;
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
            string _output = OS.CallOperatingSystemCmd(_cmd, ".");
            Console.WriteLine(_output);
            Assert.AreEqual("1\n2\n3\n", _output);
        }
        //
    }
}
//
