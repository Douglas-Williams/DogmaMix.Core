using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Strings
{
    /// <summary>
    /// Provides conversion methods for hexadecimal strings,
    /// similar to the <see cref="Convert"/> class.
    /// </summary>
    public static class HexadecimalConvert
    {
        /// <summary>
        /// Contains the hexadecimal character pairs for all possible values of a <see langword="byte"/>.
        /// The array indexes correspond to the byte values.
        /// </summary>
        private static readonly HexPair[] hexPairs =
            Enumerable.Range(0, 256)
                      .Select(v => new HexPair(v.ToString("X2")))
                      .ToArray();

        /// <summary>
        /// Contains the converted numeric values of all hexadecimal characters (uppercase and lowercase) in an array.
        /// The array indexes correspond to the Unicode code points of the characters;
        /// for this subrange, these also correspond to ASCII codes.
        /// Array locations for non-hexadecimal characters contain <c>255</c>.
        /// </summary>
        private static readonly byte[] hexValues =
            Enumerable.Range(0, 'f' + 1)
                      .Select(c =>
                          c >= '0' && c <= '9' ? (byte)(c - '0') :
                          c >= 'A' && c <= 'F' ? (byte)(c - 'A' + 10) :
                          c >= 'a' && c <= 'f' ? (byte)(c - 'a' + 10) : 
                          (byte)255)
                      .ToArray();

        /// <summary>
        /// Converts the specified byte array of 8-bit unsigned integers to an equivalent hexadecimal string.
        /// </summary>
        /// <param name="vals">The byte array of 8-bit unsigned integers to convert.</param>
        /// <returns>A hexadecimal string that is equivalent to <paramref name="vals"/>.</returns>
        /// <remarks>
        /// <para>
        /// This method implementation uses a <see href="https://en.wikipedia.org/wiki/Lookup_table">lookup table</see>,
        /// and is similar to <see href="http://stackoverflow.com/a/24343727/1149773">CodesInChaos's solution</see>.
        /// <see href="http://stackoverflow.com/a/624379/1149773">Performance Analysis</see> shows that this approach can
        /// be an order of magnitude faster than <see href="http://stackoverflow.com/a/311179/1149773">popular solutions</see>
        /// that create <see langword="string"/> instances for each two-character substring in the hexadecimal string, 
        /// due to the large quantity of object allocations incurred by the latter.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="vals"/> is <see langword="null"/>.</exception>
        public static string ToHexadecimal(byte[] vals)
        {
            ArgumentValidate.NotNull(vals, nameof(vals));

            char[] hex = new char[vals.Length * 2];

            for (int i = 0; i < vals.Length; i++)
            {
                var hexPair = hexPairs[vals[i]];
                hex[i * 2] = (char)hexPair.Hi;
                hex[i * 2 + 1] = (char)hexPair.Lo;
            }

            return new string(hex);
        }

        /// <summary>
        /// Converts the specified hexadecimal string to an equivalent byte array of 8-bit unsigned integers.
        /// </summary>
        /// <param name="hex">The source hexadecimal string to convert.</param>
        /// <returns>A byte array of 8-bit unsigned integers that is equivalent to <paramref name="hex"/>.</returns>
        /// <remarks>
        /// <para>
        /// Both uppercase (<c>'A'</c> to <c>'F'</c>) and lowercase (<c>'a'</c> to <c>'f'</c>) hexadecimal characters are allowed.
        /// Since each hexadecimal character represents a nibble (four bits),
        /// a <i>pair</i> of hexadecimal characters is required to represent a byte (eight bits).
        /// For this reason, the source string must have an even number of characters;
        /// see <see href="http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa/24343727#comment2392703_311179">Stack Overflow comment</see>.
        /// </para>
        /// <para>
        /// This method implementation uses a <see href="https://en.wikipedia.org/wiki/Lookup_table">lookup table</see>.
        /// Refer to the remarks on the <see cref="ToHexadecimal"/> method for a performance discussion.
        /// Whilst <see cref="ToHexadecimal"/> performs lookups on a per-byte level,
        /// this methods does so on a per-nibble level.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="hex"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">
        /// The source string does not have an even number of characters. -or-
        /// The source string contains non-hexadecimal characters.
        /// </exception>
        public static byte[] FromHexadecimal(string hex)
        {
            ArgumentValidate.NotNull(hex, nameof(hex));
            if (hex.Length % 2 != 0)
                throw new FormatException("The source string must have an even number of characters.");

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                char hiChar = hex[i * 2];
                char loChar = hex[i * 2 + 1];
                if (hiChar > 'f' || loChar > 'f')
                    throw new FormatException("The source string contains non-hexadecimal characters.");

                byte hi = hexValues[hiChar];
                byte lo = hexValues[loChar];
                if (hi == 255 || lo == 255)
                    throw new FormatException("The source string contains non-hexadecimal characters.");

                result[i] = (byte)((hi << 4) | lo);
            }

            return result;
        }

        /// <summary>
        /// A pair of hexadecimal characters, representing the high and low nibbles of a byte.
        /// </summary>
        private struct HexPair
        {
            /// <summary>
            /// The Unicode code point (or ASCII code) for the hexadecimal character for the high nibble.
            /// </summary>
            public readonly byte Hi;

            /// <summary>
            /// The Unicode code point (or ASCII code) for the hexadecimal character for the low nibble.
            /// </summary>
            public readonly byte Lo;

            /// <summary>
            /// Initializes a new hexadecimal pair from the specified two-character string.
            /// </summary>
            /// <param name="hex">The two-character string representing the hexadecimal pair.</param>
            public HexPair(string hex)
            {
                Hi = (byte)hex[0];
                Lo = (byte)hex[1];
            }
        }
    }
}
