// ===========================================================================
// File: Helper_OSCommands_Tests
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
            byte[] _buffer = new byte[] { 49, 10, 50, 10, 51, 10 };
            string _expected = $"1\r\n2\r\n3\r\n\r\n";
            string _fileName = @".\123.txt";
            string _cmd = @"TYPE " + _fileName;
            Console.WriteLine(_fileName);
            Files.ByteArrayToFile(_buffer, _fileName);
            Assert.IsTrue(File.Exists(_fileName));
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", -1);
            Console.WriteLine(_output);
            Console.WriteLine(string.Join("",
                _expected.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Console.WriteLine(string.Join("",
                _output.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Assert.AreEqual(_expected, _output);
        }
        //
        [TestMethod]
        public void Helpers_OSCommands_CallOperatingSystemFindCmd_Test()
        {
            // string CallOperatingSystemCmd(string cmdStr, string workingDirectory)
            // create file...
            string _fileName = @".\App_Data\TextFile.txt";
            string _cmd = @"find ""xyz"" " + _fileName;
            string _expected = $"\r\n---------- .\\APP_DATA\\TEXTFILE.TXT\r\n\r\n";
            Assert.IsTrue(File.Exists(_fileName));
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", 10000);
            Console.WriteLine(_output);
            Console.WriteLine(string.Join("",
                _expected.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Console.WriteLine(string.Join("",
                _output.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Assert.AreEqual(_expected, _output);
        }
        //
        [TestMethod]
        public void Helpers_OSCommands_CallOperatingSystemTimeOutCmd_Test()
        {
            // string CallOperatingSystemCmd(string cmdStr, string workingDirectory, int timeOut)
            string _cmd = @"ping 127.0.0.1 -n 19 > nul";
            string _expected = $"";
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", 2000);
            Console.WriteLine(_output);
            Console.WriteLine(string.Join("",
                _expected.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Assert.AreEqual(_expected, _output);
        }
        //
        [TestMethod]
        public void Helpers_OSCommands_CallOperatingSystemErrorCmd_Test()
        {
            // string CallOperatingSystemCmd(string cmdStr, string workingDirectory, int timeOut)
            string _cmd = @"xxyyzx";
            string _expected = $"\r\n-- error ---\r\n'xxyyzx' is not recognized as an internal or external command,\r\noperable program or batch file.\r\n\r\n";
            string _output = OS.CallOperatingSystemCmd(_cmd, ".", 2000);
            Console.WriteLine(_output);
            Console.WriteLine(string.Join("",
                _expected.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Console.WriteLine(string.Join("",
                _output.Select(c => String.Format(" {0:X2}", Convert.ToInt32(c)))));
            Assert.AreEqual(_expected, _output);
        }
        //
    }
}
// ===========================================================================
