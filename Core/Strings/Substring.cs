using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Strings
{
    /// <summary>
    /// Provides utility methods for the operating on substrings within <see cref="string"/> instances
    /// without instantiating them as new instances, thereby avoiding the overhead of memory allocation
    /// (on the managed heap) and garbage collection.
    /// </summary>
    public static class Substring
    {
        /// <summary>
        /// Compares substrings of two specified strings using the specified comparison rules, 
        /// and returns an integer that indicates their relative position in the sort order.
        /// </summary>        
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The zero-based starting character position of the substring within <paramref name="strA"/>.</param>
        /// <param name="lengthA">The number of characters constituting the substring from <paramref name="strA"/>.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The zero-based starting character position of the substring within <paramref name="strB"/>.</param>
        /// <param name="lengthB">The number of characters constituting the substring from <paramref name="strB"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>
        /// A signed integer that indicates the lexical relationship between the two comparands.
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Condition</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>The substring in <paramref name="strA"/> is less than the substring in <paramref name="strB"/>.</term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>The substrings are equal, or <paramref name="lengthA"/> and <paramref name="lengthB"/> are both zero.</term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>The substring in <paramref name="strA"/> is greater than the substring in <paramref name="strB"/>.</term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is similar to the <see cref="string.Compare(string, int, string, int, int, StringComparison)"/> method
        /// in the .NET Framework Class Library, but allows different lengths to be specified for the two substrings.
        /// It is implemented by calling the <see cref="CompareInfo.Compare(string, int, int, string, int, int, CompareOptions)"/> method
        /// on the appropriate <see cref="CompareInfo"/> instance with the appropriate <see cref="CompareOptions"/> value
        /// for each known value of <paramref name="comparisonType"/>.
        /// For performance, substring instantiation is avoided, working with the start indexes and lengths instead.
        /// </para>
        /// <para>
        /// The implementation of this method is adapted from the internal implementations for
        /// <see cref="string.Compare(string, int, string, int, int, StringComparison)"/> 
        /// (<see href="https://referencesource.microsoft.com/#mscorlib/system/string.cs,1ae4d07b01230bb6">source</see>)
        /// and <see cref="string.IndexOf(string, int, int, StringComparison)"/>
        /// (<see href="https://referencesource.microsoft.com/#mscorlib/system/string.cs,ef82268cfee756fe">source</see>).
        /// </para>
        /// </remarks>
        public static int Compare(string strA, int indexA, int lengthA, string strB, int indexB, int lengthB, StringComparison comparisonType)
        {
            ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));

            if (strA == null)
                return strB == null ? 0 : -1;
            if (strB == null)
                return 1;

            ArgumentValidate.StringIndexLength(strA, nameof(strA), indexA, nameof(indexA), lengthA, nameof(lengthA));
            ArgumentValidate.StringIndexLength(strB, nameof(strB), indexB, nameof(indexB), lengthB, nameof(lengthB));

            if (lengthA == 0 && lengthB == 0)
                return 0;
            if (string.ReferenceEquals(strA, strB) && indexA == indexB && lengthA == lengthB)
                return 0;

            return CompareInner(strA, indexA, lengthA, strB, indexB, lengthB, comparisonType);
        }

        internal static int CompareInner(string strA, int indexA, int lengthA, string strB, int indexB, int lengthB, StringComparison comparisonType)
        { 
            switch (comparisonType)
            {
                case StringComparison.CurrentCulture:
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);

                case StringComparison.CurrentCultureIgnoreCase:
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase);

                case StringComparison.InvariantCulture:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None);

                case StringComparison.InvariantCultureIgnoreCase:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase);

                case StringComparison.Ordinal:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.Ordinal);

                case StringComparison.OrdinalIgnoreCase:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.OrdinalIgnoreCase);

                // Might only happen if the StringComparison enumeration is extended 
                // in future or alternative implementations of the .NET Framework.
                // Assumes that all callers of this internal method have same parameter name
                // for "comparisonType".
                default:
                    throw new ArgumentException("The string comparison type is not supported.", nameof(comparisonType));
            }
        }
    }
}
