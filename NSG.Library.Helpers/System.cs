using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType">an enum type</param>
        /// <returns></returns>
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
    }
}
