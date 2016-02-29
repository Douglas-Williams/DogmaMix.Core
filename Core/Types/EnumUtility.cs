using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Types
{
    /// <summary>
    /// Provides utility methods for enumeration types,
    /// similar to the static methods in the <see cref="Enum"/> class.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Returns a sequence of the values of the constants in the current enumeration type.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type whose values to return.</typeparam>
        /// <returns>
        /// A sequence that contains the values. The elements of the sequence are sorted by the
        /// binary values (that is, the unsigned values) of the enumeration constants.
        /// </returns>
        /// <exception cref="ArgumentException">The specified type is not an enumeration.</exception>
        /// <remarks>
        /// This utility method serves as a strongly typed wrapper over the <see cref="Enum.GetValues"/> method
        /// of the <see cref="Enum"/> class in the .NET Framework Class Library.
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/q/1398664/1149773">Enum.GetValues() Return Type</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
