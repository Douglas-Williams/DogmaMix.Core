using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Strings;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the specified substring occurs within the source string,
        /// using the specified string comparison for the search.
        /// </summary>
        /// <param name="source">The source string in which to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="value"/> parameter occurs
        /// within the <paramref name="source"/> string, 
        /// or if <paramref name="value"/> is the empty string (<c>""</c>);
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="comparisonType"/> is not a valid <see cref="StringComparison"/> value.
        /// </exception>
        /// <remarks>
        /// The built-in <see cref="string.Contains(string)"/> method performs an ordinal
        /// (case-sensitive and culture-insensitive) comparison.
        /// This extension method allows the comparison type to be specified.
        /// </remarks>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(value, nameof(value));
            ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));

            return source.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Reports the zero-based index and length of the first occurrence of the specified substring in the source string.
        /// </summary>
        /// <param name="source">The source string in which to search.</param>
        /// <param name="searchValue">The substring to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="matchIndex">
        /// When this method returns, contains the zero-based starting character position of the match, if found;
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), the value will be 0.
        /// </param>
        /// <param name="matchLength">
        /// When this method returns, contains the length (in characters) of the match, if found; 
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), the value will be 0.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a match for <paramref name="searchValue"/> is found in the source string;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Refer to the remarks on the 
        /// <see cref="Find(string, string, int, int, StringComparison, out int, out int)"/> overload.
        /// </remarks>
        public static bool Find(this string source, string searchValue, StringComparison comparisonType, out int matchIndex, out int matchLength)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(searchValue, nameof(searchValue));
            ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));

            return FindInner(source, searchValue, 0, source.Length, comparisonType, out matchIndex, out matchLength);
        }

        /// <summary>
        /// Reports the zero-based index and length of the first occurrence of the specified substring in the source string.
        /// </summary>
        /// <param name="source">The source string in which to search.</param>
        /// <param name="searchValue">The substring to seek.</param>
        /// <param name="searchIndex">The zero-based starting character position in <paramref name="source"/> to search from.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="matchIndex">
        /// When this method returns, contains the zero-based starting character position of the match, if found;
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), 
        /// the value will be <paramref name="searchIndex"/>.
        /// </param>
        /// <param name="matchLength">
        /// When this method returns, contains the length (in characters) of the match, if found; 
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), the value will be 0.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a match for <paramref name="searchValue"/> is found in the source string;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Refer to the remarks on the 
        /// <see cref="Find(string, string, int, int, StringComparison, out int, out int)"/> overload.
        /// </remarks>
        public static bool Find(this string source, string searchValue, int searchIndex, StringComparison comparisonType, out int matchIndex, out int matchLength)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(searchValue, nameof(searchValue));
            ArgumentValidate.StringIndex(source, nameof(source), searchIndex, nameof(searchIndex));
            ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));

            return FindInner(source, searchValue, searchIndex, source.Length - searchIndex, comparisonType, out matchIndex, out matchLength);
        }

        /// <summary>
        /// Reports the zero-based index and length of the first occurrence of the specified substring in the source string.
        /// </summary>
        /// <param name="source">The source string in which to search.</param>
        /// <param name="searchValue">The substring to seek.</param>
        /// <param name="searchIndex">The zero-based starting character position in <paramref name="source"/> to search from.</param>
        /// <param name="searchLength">The number of character positions in <paramref name="source"/> to search through.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="matchIndex">
        /// When this method returns, contains the zero-based starting character position of the match, if found;
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), 
        /// the value will be <paramref name="searchIndex"/>.
        /// </param>
        /// <param name="matchLength">
        /// When this method returns, contains the length (in characters) of the match, if found; 
        /// or -1 if no match is found.
        /// If <paramref name="searchValue"/> is the empty string (<c>""</c>), the value will be 0.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a match for <paramref name="searchValue"/> is found in the source string;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method builds upon the <see cref="string.IndexOf(string, int, int, StringComparison)"/> method
        /// from the .NET Framework Class Library, but extends it to also return the <i>length</i> of the match,
        /// allowing string manipulation operations to subsequently be performed correctly.
        /// </para>
        /// <para>
        /// Culture-sensitive comparisons can result in a match that has a different length
        /// than the specified <paramref name="searchValue"/> argument. 
        /// For example, under the en-US culture, <c>"æ"</c> and <c>"ae"</c> are considered equal. 
        /// <c>"Encyclopædia".IndexOf("aedia")</c> evaluates to 8, indicating a match.
        /// However, the length of the matched substring, <c>"ædia"</c>, is 4,
        /// whilst the length of the searched-for parameter, <c>"aedia"</c>, is 5.
        /// This can lead to subtle bugs. 
        /// Consider the following code for removing the first occurrence of substring from a string, 
        /// taken from a <see href="https://stackoverflow.com/a/2201648/1149773">highly-upvoted answer</see> on Stack Overflow:
        /// <code>
        /// int index = sourceString.IndexOf(removeString);
        /// string cleanPath = index &lt; 0 ? sourceString : sourceString.Remove(index, removeString.Length);
        /// </code>
        /// If one were to run the above code snippet with <c>sourceString = "Encyclopædia"</c> and <c>removeString = "aedia"</c>,
        /// then it would throw an <see cref="ArgumentOutOfRangeException"/>.
        /// On the other hand, one would get correct results by using the current extension method:       
        /// <code>
        /// int index, length;
        /// sourceString.Find(removeString, StringComparison.CurrentCulture, out index, out length);
        /// string cleanPath = index &lt; 0 ? sourceString : sourceString.Remove(index, length);
        /// </code>
        /// </para>
        /// <para>
        /// There is no public functionality provided in the .NET Framework Class Library that performs such substring searches.
        /// The current method first calls <see cref="string.IndexOf(string, int, int, StringComparison)"/> to get the
        /// starting position of the match, then iteratively attempts to identify its length.
        /// It begins with the most likely case (hot path) of the match having the same length as <paramref name="searchValue"/>,
        /// verifying this through a call to <see cref="Substring.Compare(string, int, int, string, int, int, StringComparison)"/>.
        /// If not equal, it would attempt to decrement and increment the length of the match by one character each time,
        /// calling the aforementioned method until equality is confirmed.
        /// </para>
        /// <para>
        /// The approach of iterating over the substring's length is endorsed by 
        /// <see href="https://stackoverflow.com/q/15980310/1149773#comment22956089_16062528">usr</see>:
        /// </para>
        /// <blockquote>
        /// I have solved a similar problem once like this (search-string highlighting in HTML). I did it similarly. 
        /// You can tune the loop and search strategy in a way that makes it completed very quickly by checking the likely cases first. 
        /// The nice thing about this is that it seems to be totally correct and no Unicode details leak into your code.
        /// </blockquote>
        /// <para>
        /// An alternative to this approach sacrifices portability for performance by executing a P/Invoke call to the
        /// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/winnls/nf-winnls-findnlsstring"><c>FindNLSString</c></see> function
        /// (or related), as is done internally within the <see cref="string"/> class implementation.
        /// This approach is described under <see href="https://stackoverflow.com/a/20484094/1149773">this Stack Overflow answer</see>.
        /// </para>
        /// <para>
        /// Another alternative approach involves subjecting the strings to Unicode normalization 
        /// (through the <see cref="string.Normalize(NormalizationForm)"/> method) before comparison,
        /// as suggested in <see href="https://stackoverflow.com/a/16001302/1149773">this Stack Overflow answer</see>.
        /// However, this approach is undesirable since the returned results would only apply to the <i>normalized</i> forms
        /// of <paramref name="source"/> and <paramref name="searchValue"/>, requiring the original strings to be discarded 
        /// and replaced by their normalized forms for all subsequent processing and storage.
        /// </para>
        /// <para>
        /// Furthermore, Unicode normalization would not always yield results consistent with 
        /// culture-sensitive comparisons in .NET (such as <see cref="string.Compare(string, string)"/>
        /// or <see cref="string.Equals(string, string, StringComparison)"/> 
        /// with <see cref="StringComparison.CurrentCulture"/>).
        /// As mentioned in the <see href="https://unicode.org/reports/tr15/">Unicode Normalization Forms</see> annex,
        /// <see cref="NormalizationForm.FormC"/> and <see cref="NormalizationForm.FormD"/> only support <i>canonical</i> mappings, 
        /// such as between precomposed characters and combining character sequences – for example, <c>"é"</c> and <c>"e\u0301"</c>.
        /// However, the said forms do not perform <i>compatibility</i> mappings, as is required for ligatures.
        /// For example, <c>"æ"</c> is not decomposed to <c>"ae"</c>, nor <c>"ﬃ"</c> to <c>"ffi"</c>, despite that 
        /// the said ligatures are considered to be equal to their corresponding character sequences under the en-US culture.
        /// <see cref="NormalizationForm.FormKC"/> and <see cref="NormalizationForm.FormKD"/> handle compatibility mappings,
        /// and can decompose some ligatures, such as <c>"ﬃ"</c>, but miss others, such as <c>"æ"</c>.
        /// (A <see href="https://stackoverflow.com/a/15485970/1149773">Stack Overflow answer</see> mentions that
        /// “Unicode 6.2 doesn't appear to contain a normative mapping from Æ to AE.”)
        /// The issue is made worse by the discrepancies between cultures – <c>"æ"</c> is equal to <c>"ae"</c> under en-US, 
        /// but not under da-DK, as discussed under the MSDN documentation for 
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.string?view=netframework-4.7#comparison">string comparison</see>.
        /// Thus, normalization (to any form) would not give results that are consistent with <see cref="StringComparison.CurrentCulture"/> comparisons.
        /// </para>
        /// <para>
        /// Yet another alternative involves iterating over the strings as a sequence of <i>text elements</i>, 
        /// rather than UTF-16 code units, using the <see cref="StringInfo.GetNextTextElement(string, int)"/> method,
        /// as presented in <see href="https://stackoverflow.com/a/22513015/1149773">this Stack Overflow answer</see>.
        /// Results would be similar to those obtained from Unicode normalization: canonical mappings are honored,
        /// but compatibility mappings are not.
        /// </para>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="https://stackoverflow.com/q/35485677/1149773">Get substring from string using culture-sensitive comparison</see>, <i>Stack Overflow</i></item>
        /// <item><see href="https://stackoverflow.com/q/20480016/1149773">Length of substring matched by culture-sensitive String.IndexOf method</see>, <i>Stack Overflow</i></item>
        /// <item><see href="https://stackoverflow.com/q/15980310/1149773">How can I perform a culture-sensitive “starts-with” operation from the middle of a string?</see> by Jon Skeet, <i>Stack Overflow</i></item>
        /// <item><see href="https://stackoverflow.com/q/9376621/1149773">Folding/Normalizing Ligatures (e.g. Æ to ae) Using (Core)Foundation</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static bool Find(this string source, string searchValue, int searchIndex, int searchLength, StringComparison comparisonType, out int matchIndex, out int matchLength)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(searchValue, nameof(searchValue));
            ArgumentValidate.StringIndexLength(source, nameof(source), searchIndex, nameof(searchIndex), searchLength, nameof(searchLength));
            ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));

            return FindInner(source, searchValue, searchIndex, searchLength, comparisonType, out matchIndex, out matchLength);
        }

        private static bool FindInner(string source, string searchValue, int searchIndex, int searchLength, StringComparison comparisonType, out int matchIndex, out int matchLength)
        {
            matchIndex = source.IndexOf(searchValue, searchIndex, searchLength, comparisonType);
            if (matchIndex == -1)
            {
                matchLength = -1;
                return false;
            }

            matchLength = FindMatchLength(source, searchValue, searchIndex, searchLength, comparisonType, matchIndex);

            // Defensive programming, but should never happen
            if (matchLength == -1)
            {
                matchIndex = -1;
                return false;
            }

            return true;
        }

        private static int FindMatchLength(string source, string searchValue, int searchIndex, int searchLength, StringComparison comparisonType, int matchIndex)
        {
            int matchLengthMaximum = searchLength - (matchIndex - searchIndex);
            int matchLengthInitial = Math.Min(searchValue.Length, matchLengthMaximum);

            // Hot path: match length is same as specified search string
            if (Substring.CompareInner(source, matchIndex, matchLengthInitial, searchValue, 0, searchValue.Length, comparisonType) == 0)
                return matchLengthInitial;

            int matchLengthDecrementing = matchLengthInitial - 1;
            int matchLengthIncrementing = matchLengthInitial + 1;

            while (matchLengthDecrementing >= 0 || matchLengthIncrementing <= matchLengthMaximum)
            {
                if (matchLengthDecrementing >= 0)
                {
                    if (Substring.CompareInner(source, matchIndex, matchLengthDecrementing, searchValue, 0, searchValue.Length, comparisonType) == 0)
                        return matchLengthDecrementing;

                    matchLengthDecrementing--;
                }

                if (matchLengthIncrementing <= matchLengthMaximum)
                {
                    if (Substring.CompareInner(source, matchIndex, matchLengthIncrementing, searchValue, 0, searchValue.Length, comparisonType) == 0)
                        return matchLengthIncrementing;

                    matchLengthIncrementing++;
                }
            }

            // Should never happen
            return -1;
        }
        
        /// <summary>
        /// Removes the specified prefix from the beginning of the source string.
        /// </summary>
        /// <param name="source">The source string from which to remove the prefix.</param>
        /// <param name="prefix">The prefix to remove from the beginning of <paramref name="source"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how the strings are compared.</param>
        /// <returns>
        /// The source string with <paramref name="prefix"/> removed.
        /// If <paramref name="source"/> does not start with <paramref name="prefix"/>,
        /// then <paramref name="source"/> is returned unchanged.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="prefix"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="comparisonType"/> is not a valid <see cref="StringComparison"/> value.</exception>
        /// <remarks>
        /// <para>
        /// This method only removes at most one occurrence of <paramref name="prefix"/>,
        /// even if the said prefix occurs multiple times consecutively in <paramref name="source"/>.
        /// </para>
        /// <para>
        /// This method works even if <paramref name="prefix"/> has a different number of characters than its matching substring
        /// in <paramref name="source"/>. This may arise for culture-sensitive comparisons involving different representations
        /// of the same string – for example, <c>"é"</c> and <c>"e\u0301"</c>, or <c>"æ"</c> and <c>"ae"</c>.
        /// Thus, <c>"Æble".RemoveStart("aeb", StringComparison.CurrentCultureIgnoreCase)</c> evaluates to <c>"le"</c>.
        /// Refer to the remarks on the <see cref="Find(string, string, int, int, StringComparison, out int, out int)"/> method,
        /// on which the implementation of the current method relies, for more details.
        /// </para>
        /// </remarks>
        public static string RemovePrefix(this string source, string prefix, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(prefix, nameof(prefix));
            
            int index, length;
            source.Find(prefix, comparisonType, out index, out length);

            if (index != 0)
                return source;

            return source.Substring(length);
        }

        /// <summary>
        /// Removes the specified suffix from the end of the source string.
        /// </summary>
        /// <param name="source">The source string from which to remove the suffix.</param>
        /// <param name="suffix">The suffix to remove from the end of <paramref name="source"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how the strings are compared.</param>
        /// <returns>
        /// The source string with <paramref name="suffix"/> removed.
        /// If <paramref name="source"/> does not end with <paramref name="suffix"/>,
        /// then <paramref name="source"/> is returned unchanged.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="suffix"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="comparisonType"/> is not a valid <see cref="StringComparison"/> value.</exception>
        /// <remarks>
        /// <para>
        /// This method only removes at most one occurrence of <paramref name="suffix"/>,
        /// even if the said suffix occurs multiple times consecutively in <paramref name="source"/>.
        /// </para>
        /// <para>
        /// Refer to the remarks on the <see cref="RemovePrefix"/> method regarding culture-sensitive comparisons.
        /// </para>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="https://stackoverflow.com/a/4101583/1149773">C# removing strings from end of string</see> (answer), <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static string RemoveSuffix(this string source, string suffix, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            ArgumentValidate.NotNull(source, nameof(source));
            ArgumentValidate.NotNull(suffix, nameof(suffix));

            if (suffix == "")
                return source;

            if (source.EndsWith(suffix, comparisonType))
            {
                int index = source.LastIndexOf(suffix, comparisonType);
                return source.Substring(0, index);
            }

            return source;
        }        
        
        /// <summary>
        /// Creates a <see cref="string"/> from the specified sequence of Unicode characters.
        /// </summary>
        /// <param name="chars">The sequence of Unicode characters from which to create the string.</param>
        /// <returns>A string that corresponds to <paramref name="chars"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="chars"/> is <see langword="null"/>.</exception>
        public static string AsString(this IEnumerable<char> chars)
        {
            ArgumentValidate.NotNull(chars, nameof(chars));

            return chars as string ?? new string(chars.AsArray());
        }
    }
}
