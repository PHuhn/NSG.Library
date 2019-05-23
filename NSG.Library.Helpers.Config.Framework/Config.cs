//
using System;
using System.Configuration;
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
    public static partial class Config
    {
        //
        /// <summary>
        /// Get a value from the AppSetting section of the web config.
        /// </summary>
        /// <param name="configAppKey">AppSetting key value</param>
        /// <param name="defaultValue">if not found return this value</param>
        /// <returns>string of the value in the appSetting section in the config file.</returns>
        public static string GetStringAppSettingConfigValue(string configAppKey, string defaultValue)
        {
            string _value = "";
            try
            {
                _value = System.Configuration.ConfigurationManager.AppSettings[
                    configAppKey].ToString();
            }
            catch( Exception _ex )
            {
                System.Diagnostics.Debug.WriteLine( _ex.Message );
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
