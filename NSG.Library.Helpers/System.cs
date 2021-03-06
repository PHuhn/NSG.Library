﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml;
//
namespace System
{
    //
    /// <summary>
    /// Static helpers extension methods, namespaced to system, so it extends
    /// system class if included.
    /// </summary>
    public static class Extensions
    {
        //
        //  ==================================================================
        //  ENum Extension methods
        //  *   GetName
        //  *   GetDescription
        //  *   GetValues
        //  *   ToDictionary
        //
        #region "enum methods"
        //
        /// <summary>
        /// Get simple name of the enum value
        /// </summary>
        /// <param name="value">an enum value</param>
        /// <returns>string of literal name of the enum</returns>
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
        //
        /// <summary>
        /// Convert the enum description to the literal name of the enum.
        /// [System.ComponentModel.Description("Warehouse Shipping")]Shipping
        /// </summary>
        /// <param name="value">an enum value</param>
        /// <returns>string of literal description of the enum</returns>
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var descrAttribute = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false).FirstOrDefault()
                as DescriptionAttribute;
            return ((descrAttribute == null) ?
                (string)value.GetName() : descrAttribute.Description);
        }
        //
        /// <summary>
        /// Get an enumeration of the enum type
        /// </summary>
        /// <typeparam name="T">an enum type</typeparam>
        /// <param name="enumType">an enum type</param>
        /// <returns>IEnumerable of T</returns>
        public static IEnumerable<T> GetValues<T>(this Type enumType)
        {
            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type must be an Enum");
            foreach (object item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }
        //
        /// <summary>
        /// Dictionary of all values and descriptions
        /// </summary>
        /// <param name="enumType">an enum type</param>
        /// <returns>Dictionary of all values and descriptions</returns>
        public static Dictionary<int, string> ToDictionary(this Type enumType)
        {
            if (!enumType.IsEnum) return null;
            Dictionary<int, string> _return = new Dictionary<int, string>();
            foreach (Enum _e in Enum.GetValues(enumType))
                _return.Add(Convert.ToInt32(_e), _e.GetDescription());
            return _return;
        }
        //
        #endregion
        //
        //  *   ToLineFeedString
        //
        #region "Exception"
        //
        /// <summary>
        /// An exception ToString using linefeeds.
        /// </summary>
        /// <param name="ex">Some exception</param>
        /// <returns>A linefeed formated string</returns>
        public static string ToLineFeedString(this Exception ex)
        {
            StringBuilder _return = new StringBuilder();
            _return.AppendFormat("Exception Date: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()).AppendLine();
            _return.AppendFormat("Message: {0}", ex.Message).AppendLine();
            if (!(ex.InnerException is null))
                _return.AppendFormat("Inner Message: {0}", ex.InnerException.Message).AppendLine();
            _return.AppendFormat("Source: {0}", ex.Source).AppendLine();
            _return.AppendFormat("Type: {0}", ex.GetBaseException().GetType().FullName.ToString()).AppendLine();
            foreach (DictionaryEntry _item in ex.Data)
            {
                _return.AppendFormat("Data: {0} = {1}", _item.Key, _item.Value.ToString()).AppendLine();
            }
            if (!(ex.HelpLink is null))
                _return.AppendFormat("Help Link: {0}", ex.HelpLink.ToString()).AppendLine();
            if (!(ex.TargetSite is null))
                _return.AppendFormat("Target Site: {0}", ex.TargetSite.ToString()).AppendLine();
            _return.AppendFormat("H-Result: {0}", ex.HResult.ToString()).AppendLine();
            if (!(ex.StackTrace is null))
            {
                _return.AppendLine("Stack Trace:");
                _return.AppendLine(ex.StackTrace.ToString());
            }
            var _frames = new StackTrace(ex, true);
            if (_frames.FrameCount > 0)
            {
                _return.AppendLine("Stack Frame:");
                foreach (var _frame in _frames.GetFrames())
                {
                    var _method = _frame.GetMethod();
                    var _fullName = _method.DeclaringType != null
                        ? $"{_method.DeclaringType.FullName}.{_method.Name}"
                        : _method.Name;
                    _return.AppendFormat("   {0}: {1}", _fullName, _frame.ToString());
                }
            }
            return _return.ToString();
        }
        //
        #endregion
        //
    }
}
