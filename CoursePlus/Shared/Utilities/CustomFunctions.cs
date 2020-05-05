using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Utilities
{
    public class CustomFunctions
    {
        public static bool EnumTryParse<T>(string input, out T theEnum)
        {
            foreach (string en in Enum.GetNames(typeof(T)))
            {
                if (en.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    theEnum = (T)Enum.Parse(typeof(T), input, true);
                    return true;
                }
            }

            theEnum = default(T);
            return false;
        }

        public static bool NullableEnumTryParse<T>(string input, out T theEnum)
        {
            Type t = Nullable.GetUnderlyingType(typeof(T));

            foreach (string en in Enum.GetNames(t))
            {
                if (en.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    theEnum = (T)Enum.Parse(t, input, true);
                    return true;
                }
            }

            theEnum = default(T);
            return false;
        }

        public static bool IsEnumerableType(Type type)
        {
            return (type.GetInterface(nameof(IEnumerable)) != null);
        }
    }

    public static class CustomStaticFunctions
    {
        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}
