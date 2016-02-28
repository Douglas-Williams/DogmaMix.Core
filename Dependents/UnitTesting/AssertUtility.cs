using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.UnitTesting
{
    internal static class AssertUtility
    {
        /// <summary>
        /// Formats the specified argument to be displayed in the message for an assertion that has failed.
        /// </summary>
        public static string FormatArgument(object item, bool quote = true)
        {
            if (item == null)
                return "(null)";

            var str = item.ToString();
            if (str == null)
                return "(object)";

            if (quote)
                str = $"\"{str}\"";

            return str;
        }
    }
}
