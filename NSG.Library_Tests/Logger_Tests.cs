using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using NSG.Library.Logger;
//
namespace NSG.Library_Tests
{
    [TestClass]
    public class NSG_Library_Logger_Tests
    {
        //
        [TestMethod]
        public void Logger_ListString_Basic_Test()
        {
            // the parameter Testing will be set in the Application column
            ILogger _logger = new ListLogger("Testing");
            long id1 = _logger.Log((byte)3, "Tester", "Logger_Test", "This is a test");
            long id2 = _logger.Log(LoggingLevel.Warning, "Tester", MethodBase.GetCurrentMethod(), "This is a warning");
            Console.WriteLine(String.Format("Id 1: {0}, Id 2: {1}", id1, id2));
            List<string> _logs = _logger.ListString(5);
            Assert.AreEqual(2, _logs.Count);
            foreach( string _log in _logs )
                Console.WriteLine(_log);
        }
        //
        [TestMethod]
        public void Logger_ListString_Test()
        {
            // the parameter Testing will be set in the Application column
            ILogger _logger = new ListLogger("Testing");
            long id1 = _logger.Log(LoggingLevel.Error, "Tester", MethodBase.GetCurrentMethod(), "This is a Error");
            long id2 = _logger.Log(LoggingLevel.Warning, "Tester", MethodBase.GetCurrentMethod(), "This is a warning");
            long id3 = _logger.Log(LoggingLevel.Info, "Tester", MethodBase.GetCurrentMethod(), "This is a info");
            long id4 = _logger.Log(LoggingLevel.Debug, "Tester", MethodBase.GetCurrentMethod(), "This is a debug");
            Console.WriteLine(String.Format("Id 1: {0}, Id 2: {1}, Id 3: {2}, Id 4: {3}",
                id1, id2, id3, id4));
            List<string> _logs = _logger.ListString(3);
            Assert.AreEqual(3, _logs.Count);
            foreach (string _log in _logs)
                Console.WriteLine(_log);
        }
        //
        [TestMethod]
        public void Logger_ListString_DefaultStatic_Test()
        {
            // Log.Logger = new ListLogger("Testing");
            long id1 = Log.Logger.Log(LoggingLevel.Error, "Tester", MethodBase.GetCurrentMethod(), "This is a Error");
            long id2 = Log.Logger.Log(LoggingLevel.Warning, "Tester", MethodBase.GetCurrentMethod(), "This is a warning");
            long id3 = Log.Logger.Log(LoggingLevel.Info, "Tester", MethodBase.GetCurrentMethod(), "This is a info");
            long id4 = Log.Logger.Log(LoggingLevel.Debug, "Tester", MethodBase.GetCurrentMethod(), "This is a debug");
            Console.WriteLine(String.Format("Id 1: {0}, Id 2: {1}, Id 3: {2}, Id 4: {3}",
                id1, id2, id3, id4));
            List<string> _logs = Log.Logger.ListString(3);
            Assert.AreEqual(3, _logs.Count);
            foreach (string _log in _logs)
                Console.WriteLine(_log);
        }
        //
        [TestMethod]
        public void Logger_ListString_StaticAppTesting_Test()
        {
            string _application = "TTTTesting";
            // the paramer Testing will be set in the Application column
            Log.Logger = new ListLogger(_application);
            long id1 = Log.Logger.Log(LoggingLevel.Error, "Tester", MethodBase.GetCurrentMethod(), "This is a Error");
            Console.WriteLine(String.Format("Id 1: {0}", id1 ));
            List<string> _logs = Log.Logger.ListString(10);
            Assert.AreEqual(1, _logs.Count);
            foreach (string _log in _logs)
                Console.WriteLine(_log);
            Assert.IsTrue(_logs[0].Contains(_application));
        }
        //
        [TestMethod]
        public void Logger_ListString_Max_Test()
        {
            Log.Logger = new ListLogger("Testing", 10);
            for ( int _i =0; _i < 12; _i++)
            {
                long id1 = Log.Logger.Log(LoggingLevel.Warning, "Tester", MethodBase.GetCurrentMethod(), "This is a Error");
                Console.WriteLine(String.Format("Id 1: {0}", id1));
            }
            List<string> _logs = Log.Logger.ListString(100);
            Assert.AreEqual(10, _logs.Count);
            foreach (string _log in _logs)
                Console.WriteLine(_log);
        }
        //
    }
}
