using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting
{
    /// <summary>
    /// Verifies true/false propositions associated with sort-order comparisons in unit tests.
    /// Such comparisons involve <see cref="IComparer{T}"/> comparers.
    /// </summary>
    public static class CompareAssert
    {
        /// <summary>
        /// Verifies that <paramref name="x"/> is less than <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The object that should be the lesser of the two.</param>
        /// <param name="y">The object that should be the greater of the two.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void IsLessThan<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison < 0, "less", message);
            CompareInner(y, x, comparer, comparison => comparison > 0, "greater", message);
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> is less than or equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The object that should be the lesser of the two, unless both are equal.</param>
        /// <param name="y">The object that should be the greater of the two, unless both are equal.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void IsLessThanOrEqualTo<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison <= 0, "less or equal", message);
            CompareInner(y, x, comparer, comparison => comparison >= 0, "greater or equal", message);
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The object that should be the greater of the two.</param>
        /// <param name="y">The object that should be the lesser of the two.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void IsGreaterThan<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison > 0, "greater", message);
            CompareInner(y, x, comparer, comparison => comparison < 0, "less", message);
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> is greater than or equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The object that should be the greater of the two, unless both are equal.</param>
        /// <param name="y">The object that should be the lesser of the two, unless both are equal.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void IsGreaterThanOrEqualTo<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison >= 0, "greater or equal", message);
            CompareInner(y, x, comparer, comparison => comparison <= 0, "less or equal", message);
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <remarks>
        /// Equality comparisons are usually performed using <see cref="IEqualityComparer{T}"/> comparers,
        /// not <see cref="IComparer{T}"/>.
        /// Consider using the <see cref="EqualityAssert.Equals{T}(T, T, IEqualityComparer{T}, string)"/> method instead.
        /// </remarks>
        public static void IsEqualTo<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison == 0, "equal", message);
            CompareInner(y, x, comparer, comparison => comparison == 0, "equal", message);
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">
        /// The comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        /// <remarks>
        /// Equality comparisons are usually performed using <see cref="IEqualityComparer{T}"/> comparers,
        /// not <see cref="IComparer{T}"/>.
        /// Consider using the <see cref="EqualityAssert.NotEquals{T}(T, T, IEqualityComparer{T}, string)"/> method instead.
        /// </remarks>
        public static void IsNotEqualTo<T>(T x, T y, IComparer<T> comparer = null, string message = null)
        {
            CompareInner(x, y, comparer, comparison => comparison != 0, "not equal", message);
            CompareInner(y, x, comparer, comparison => comparison != 0, "not equal", message);
        }

        private static void CompareInner<T>(T x, T y, IComparer<T> comparer, Func<int, bool> comparisonCheck, string expectedResult, string message)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int comparisonResult = comparer.Compare(x, y);
            if (!comparisonCheck(comparisonResult))
            {
                var xStr = AssertUtility.FormatArgument(x);
                var yStr = AssertUtility.FormatArgument(y);
                var comparerStr = GetComparerName(comparer);
                var comparisonStr =
                    comparisonResult == 0 ? "equal to" :
                    comparisonResult < 0 ? "less than" : "greater than";

                throw new AssertFailedException($"Value {xStr} is {comparisonStr} {yStr} under the {comparerStr} comparer. Expected to be {expectedResult}. {message}");
            }
        }

        private static string GetComparerName<T>(IComparer<T> comparer)
        {
            string comparerStr = comparer.ToString();
            if (comparerStr == comparer.GetType().ToString())
                comparerStr = comparer.GetType().Name;

            comparerStr = AssertUtility.FormatArgument(comparerStr);

            if (comparer == EqualityComparer<T>.Default)
                comparerStr += " (default)";

            return comparerStr;
        }
    }
}
