using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using NSG.Library.Helpers;
//
namespace NSG.Library_Tests
{
    [TestClass]
    public class NSG_Library_Helper_System_Tests
    {
        //
        public enum LogLevel
        {
            Audit = 0,
            Error = 1,
            Warning = 2,
            [System.ComponentModel.Description("Information")] Info = 3,
            Debug = 4,
            Verbose = 5
        };
        [TestMethod]
        public void Enum_GetName_Test()
        {
            string _actual = LogLevel.Warning.GetName();
            Console.WriteLine(_actual);
            Assert.AreEqual("Warning", _actual);
        }
        //
        [TestMethod]
        public void Enum_GetDescription_Test()
        {
            string _actual = LogLevel.Info.GetDescription();
            Console.WriteLine(_actual);
            Assert.AreEqual("Information", _actual);
        }
        //
        [TestMethod]
        public void Enum_ToDictionary_Test()
        {
            Dictionary<int, string> _dict =
                      typeof(LogLevel).ToDictionary();
            foreach (var _e in _dict)
                Console.WriteLine(String.Format("{0} - {1}", _e.Key, _e.Value));
            Assert.AreEqual(6, _dict.Count);
        }
        //
    }
}
//
