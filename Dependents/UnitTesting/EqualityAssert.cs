using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting
{
    /// <summary>
    /// Verifies true/false propositions associated with equality comparisons in unit tests.
    /// Such comparisons involve <see cref="IEqualityComparer{T}"/> comparers.
    /// </summary>
    public static class EqualityAssert
    {
        /// <summary>
        /// Verifies that <paramref name="x"/> and <paramref name="y"/> are equal.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">
        /// The equality comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void Equals<T>(T x, T y, IEqualityComparer<T> comparer = null, string message = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            if (!comparer.Equals(x, y))
            {
                var xStr = AssertUtility.FormatArgument(x);
                var yStr = AssertUtility.FormatArgument(y);
                var comparerStr = GetComparerName(comparer);
                Assert.Fail($"Value {xStr} is not equal to {yStr} under the {comparerStr} comparer. Expected to be equal. {message}");
            }

            var xHash = comparer.GetHashCode(x);
            var yHash = comparer.GetHashCode(y);
            if (xHash != yHash)
            {
                var xStr = AssertUtility.FormatArgument(x);
                var yStr = AssertUtility.FormatArgument(y);
                var comparerStr = GetComparerName(comparer);
                Assert.Fail($"The hash code for {xStr} ({xHash}) is not equal to the hash code for {yStr} ({yHash}) under the {comparerStr} comparer. Expected to be equal. {message}");
            }
        }

        /// <summary>
        /// Verifies that <paramref name="x"/> and <paramref name="y"/> are not equal.
        /// </summary>
        /// <typeparam name="T">The type of the two objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">
        /// The equality comparer to use for comparing the two objects,
        /// or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer for type <typeparamref name="T"/>.
        /// </param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void NotEquals<T>(T x, T y, IEqualityComparer<T> comparer = null, string message = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            if (comparer.Equals(x, y))
            {
                var xStr = AssertUtility.FormatArgument(x);
                var yStr = AssertUtility.FormatArgument(y);
                var comparerStr = GetComparerName(comparer);
                Assert.Fail($"Value {xStr} is equal to {yStr} under the {comparerStr} comparer. Expected not to be equal. {message}");
            }
        }

        private static string GetComparerName<T>(IEqualityComparer<T> comparer)
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
