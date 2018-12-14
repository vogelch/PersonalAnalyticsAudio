using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns the description for a given enums
        /// (requires that a DescriptionAttribute is assigned to the items in the enum)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionForEnum(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            DescriptionAttribute[] stringDescription = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return stringDescription.Length > 0 ? stringDescription[0].Description : null;
        }

        /// <summary>
        /// Returns all values of an enum type
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
