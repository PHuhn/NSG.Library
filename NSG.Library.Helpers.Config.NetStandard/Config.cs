//
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
//
// https://stackoverflow.com/questions/1469764/run-command-prompt-commands
//
namespace NSG.Library.Helpers
{
    /// <summary>
    /// The <see cref="NSG.Library.Helpers"/> namespace contains a
    /// collection of static helper methods.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }
    //
    /// <summary>
    /// Static helpers for handling AppSettings configuration.
    /// </summary>
    public static class Config
    {
        //
        /// <summary>
        /// The private hidden i-configuration for the singleton pattern.
        /// </summary>
        private static IConfiguration config = null;
        //
        /// <summary>
        /// The public i-configuration getter for the singleton pattern.
        /// </summary>
        /// <value>IConfiguration</value>
        public static IConfiguration GetAppConfiguration { get { return config; } }
        //
        /// <summary>
        /// Create and establish 
        /// </summary>
        /// <param name="appSettings">The JSON file name for general configuration.</param>
        /// <param name="appSecrets">The JSON file name for secret information configuration.</param>
        /// <returns>IConfiguration</returns>
        public static IConfiguration SetAppConfiguration(string appSettings, string appSecrets)
        {
            if (appSettings != "")
                if (!File.Exists(appSettings))
                    throw new FileNotFoundException($"Settings file: {appSettings} not found.");
            if (appSecrets != "")
                if (!File.Exists(appSecrets))
                    throw new FileNotFoundException($"Secrets file: {appSecrets} not found.");
            //
            config = new ConfigurationBuilder()
                .AddJsonFile(appSettings, optional: true, reloadOnChange: false)
                .AddJsonFile(appSecrets, optional: true, reloadOnChange: false)
                .Build();
            return config;
        }
        //
        /// <summary>
        /// Get a value from the AppSetting section of the web config.
        /// </summary>
        /// <param name="configAppKey">AppSetting key value</param>
        /// <param name="defaultValue">if not found return this value</param>
        /// <returns>string of the value in the appSetting section in the config file.</returns>
        public static string GetStringAppSettingConfigValue(string configAppKey, string defaultValue)
        {
            string[] keys = configAppKey.Split(':');
            string _value = "";
            if (config == null)
                throw new ApplicationException("static config is null, call SetAppConfiguration first.");
            try
            {
                if (keys.Length == 2)
                {
                    _value = config.GetSection(keys[0])[keys[1]].ToString();
                }
                else
                    _value = config.GetValue<string>(configAppKey);
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(configAppKey + ", " + _ex.Message);
                Console.WriteLine(configAppKey + ", " + _ex.Message);
                _value = defaultValue;
            }
            return _value;
        }
        //
        /// <summary>
        /// Get a value from the AppSetting section of the web config.
        /// </summary>
        /// <param name="configAppKey">AppSetting key value</param>
        /// <param name="defaultValue">if not found return this boolean value</param>
        /// <returns>boolean value in the appSetting section in the config file.</returns>
        public static bool GetBoolAppSettingConfigValue(string configAppKey, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(GetStringAppSettingConfigValue(configAppKey, defaultValue.ToString()));
            }
            catch
            {
                return defaultValue;
            }
        }
        //
        /// <summary>
        /// Get a value from the AppSetting section of the web config.
        /// </summary>
        /// <param name="configAppKey">AppSetting key value</param>
        /// <param name="defaultValue">if not found return this integer value</param>
        /// <returns>integer value in the appSetting section in the config file.</returns>
        public static int GetIntAppSettingConfigValue(string configAppKey, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(GetStringAppSettingConfigValue(configAppKey, defaultValue.ToString()));
            }
            catch
            {
                return defaultValue;
            }
        }
        //
    }
}
//
