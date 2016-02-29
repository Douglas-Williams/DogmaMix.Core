using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DogmaMix.Core
{
    /// <summary>
    /// Provides utility methods for validating arguments to methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Parameter names should always be supplied by the caller using the <see langword="nameof"/> operator, introduced in C# 6.
    /// If this keyword is not available, one could use lambda expressions for retrieving the names in a refactor-safe manner
    /// (see links below).
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://msdn.microsoft.com/en-us/library/dn986596.aspx">nameof</see>, <i>MSDN Library</i></item>
    /// <item><see href="http://stackoverflow.com/q/11063502/1149773">Getting names of local variables (and parameters) at run-time through lambda expressions</see>, <i>Stack Overflow</i></item>
    /// <item><see href="http://stackoverflow.com/q/10759632/1149773">Lambda expressions for refactor-safe ArgumentException</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    public static class ArgumentValidate
    {
        /// <summary>
        /// Verifies that the specified parameter value is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to validate.</typeparam>
        /// <param name="value">The value of the parameter to validate.</param>
        /// <param name="paramName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method will not throw an exception if <typeparamref name="T"/> is a non-nullable value type.
        /// According to Section 7.10.6 of the C# Language Specification (version 5.0):
        /// <blockquote>
        /// The <c>x == null</c> construct is permitted even though <c>T</c> could represent a value type, 
        /// and the result is simply defined to be <see langword="false"/> when <c>T</c> is a value type.
        /// </blockquote>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/a/8824259/1149773">Comparing a generic against null that could be a value or reference type?</see> (answer) by Eric Lippert, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// public static class EnumerableExtensions
        /// {
        ///     public static int IndexOf&lt;TSource&gt;(this IEnumerable&lt;TSource&gt; source, Func&lt;TSource, bool&gt; predicate)
        ///     {
        ///         ArgumentValidate.NotNull(source, nameof(source));
        ///         ArgumentValidate.NotNull(predicate, nameof(predicate));
        ///         
        ///         // rest of method implementation
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void NotNull<T>(T value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Verifies that the specified parameter value exists in the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The enumeration type of the parameter to validate.</typeparam>
        /// <param name="value">The enumeration value of the parameter to validate.</param>
        /// <param name="paramName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> is not an enumeration. -or-
        /// <paramref name="value"/> is not a member of the enumeration type <typeparamref name="T"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// public static bool Contains(this string source, string value, StringComparison comparisonType)
        /// {
        ///     ArgumentValidate.EnumDefined(comparisonType, nameof(comparisonType));
        /// 
        ///     // rest of method implementation
        /// }
        /// </code>
        /// </example>
        public static void EnumDefined<T>(T value, string paramName)
            where T : struct
        {
            if (!typeof(T).IsEnumDefined(value))
                throw new ArgumentException($"The value \"{value}\" is not a member of the \"{typeof(T).Name}\" enumeration type.", paramName);
        }
        
        /// <summary>
        /// Verifies that the specified parameter value is not less than zero.
        /// </summary>
        /// <param name="value">The value of the parameter to validate.</param>
        /// <param name="paramName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than zero.</exception>
        public static void NotNegative(int value, string paramName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName, "Value cannot be less than zero.");
        }
        
        /// <summary>
        /// Verifies that the specified index refers to a character position within the specified string.
        /// </summary>
        /// <param name="str">The string to which the index applies.</param>
        /// <param name="strParamName">The name of the parameter for <paramref name="str"/>.</param>
        /// <param name="index">The zero-based character position in <paramref name="str"/>.</param>
        /// <param name="indexParamName">The name of the parameter for <paramref name="index"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> refers to a position beyond the end of the string. -or-
        /// <paramref name="index"/> is less than zero.
        /// </exception>
        public static void StringIndex(string str, string strParamName, int index, string indexParamName)
        {
            NotNull(str, strParamName);
            NotNegative(index, indexParamName);

            if (index > str.Length)
                throw new ArgumentOutOfRangeException(indexParamName, "Index cannot be greater than the length of the string.");
        }

        /// <summary>
        /// Verifies that the specified index and length refer to a substring within the specified string.
        /// </summary>
        /// <param name="str">The string from which the substring will be read.</param>
        /// <param name="strParamName">The name of the parameter for <paramref name="str"/>.</param>
        /// <param name="index">The zero-based starting character position of the substring in <paramref name="str"/>.</param>
        /// <param name="indexParamName">The name of the parameter for <paramref name="index"/>.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="lengthParamName">The name of the parameter for <paramref name="length"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> plus <paramref name="length"/> refers to a position beyond the end of the string. -or-
        /// <paramref name="index"/> or <paramref name="length"/> is less than zero.
        /// </exception>
        public static void StringIndexLength(string str, string strParamName, int index, string indexParamName, int length, string lengthParamName)
        {
            StringIndex(str, strParamName, index, indexParamName);

            NotNegative(length, lengthParamName);
            
            if (length > str.Length)
                throw new ArgumentOutOfRangeException(lengthParamName, "Length cannot be greater than the length of the string.");
            if (index + length > str.Length)
                throw new ArgumentOutOfRangeException(lengthParamName, "Index plus length refers to a position beyond the end of the string.");
        }
    }
}
