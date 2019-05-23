﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
//
using NSG.Library.Helpers;
//
namespace NSG.Library.Helpers_Config_NetStandard_Tests
{
    [TestClass]
    public class NSG_Library_Helper_AppSettings_Tests
    {
        // Use TestInitialize to execute code before running each test 
        [TestInitialize]
        public void NSG_Library_Helper_Initialize()
        {
            Config.SetAppConfiguration("appsettings.json", "appsecrets.json");
            Console.WriteLine("Class initialize");
        }
        //
        #region "String"
        //
        [TestMethod]
        public void Helpers_AppSettings_GetStringAppSettingConfigValueWhois_Test()
        {
            string _dir = Config.GetStringAppSettingConfigValue("Services:WhoisDir", "");
            Console.WriteLine(_dir);
            Assert.IsTrue(_dir != "");
        }
        //
        [TestMethod]
        public void Helpers_AppSettings_GetStringAppSettingConfigValuePing_Test()
        {
            string _dir = Config.GetStringAppSettingConfigValue("Services:PingDir", "");
            Console.WriteLine(_dir);
            Assert.IsTrue(_dir != "");
        }
        //
        #endregion // String
        //
        #region "Bool"
        //
        [TestMethod]
        public void Helpers_AppSettings_GetBoolAppSettingConfigValueProd_Test()
        {
            bool _prod = Config.GetBoolAppSettingConfigValue("appSettings:production", true);
            Console.WriteLine(_prod);
            Assert.AreEqual(_prod, false);
        }
        //
        [TestMethod]
        public void Helpers_AppSettings_GetBoolAppSettingConfigValueProd_Bad_Test()
        {
            bool _prod = Config.GetBoolAppSettingConfigValue("prod_XXXXX", true);
            Console.WriteLine(_prod);
            Assert.AreEqual(_prod, true);
        }
        //
        [TestMethod]
        public void Helpers_AppSettings_GetBoolAppSettingConfigValueProd_BadValue_Test()
        {
            bool _prod = Config.GetBoolAppSettingConfigValue("Services:WhoisDir", true);
            Console.WriteLine(_prod);
            Assert.AreEqual(_prod, true);
        }
        //
        #endregion // Bool
        //
        #region "Int"
        //
        [TestMethod]
        public void Helpers_AppSettings_GetIntAppSettingConfigValueProd_Test()
        {
            int _level = Config.GetIntAppSettingConfigValue("appSettings:LogLevel", 2);
            Console.WriteLine(_level);
            Assert.AreEqual(_level, 4);
        }
        //
        [TestMethod]
        public void Helpers_AppSettings_GetIntAppSettingConfigValueProd_Bad_Test()
        {
            int _level = Config.GetIntAppSettingConfigValue("Log_XXXXX", 2);
            Console.WriteLine(_level);
            Assert.AreEqual(_level, 2);
        }
        //
        [TestMethod]
        public void Helpers_AppSettings_GetIntAppSettingConfigValueProd_BadValue_Test()
        {
            int _level = Config.GetIntAppSettingConfigValue("production", 2);
            Console.WriteLine(_level);
            Assert.AreEqual(_level, 2);
        }
        //
        #endregion // Int
        //
    }
}
//
