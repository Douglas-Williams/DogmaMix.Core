using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Collections;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.Types;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class SequenceComparerTests
    {
        [TestMethod]
        public void Compare_Lexicographical_Arrays()
        {
            var comparer = SequenceComparer<int>.Lexicographical;
            CompareAssert.IsEqualTo(new int[] { }, new int[] { }, comparer);
            CompareAssert.IsLessThan(new int[] { }, new int[] { 42 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { 42 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { 42, 98 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34 }, comparer);
            CompareAssert.IsEqualTo(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16, 65 }, comparer);
        }

        [TestMethod]
        public void Compare_Lexicographical_Enumerables()
        {
            var comparer = SequenceComparer<int>.Lexicographical;
            CompareAssert.IsEqualTo(Enumerable.Empty<int>(), Enumerable.Empty<int>(), comparer);
            CompareAssert.IsLessThan(Enumerable.Empty<int>(), EnumerableUtility.Yield(42), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), Enumerable.Empty<int>(), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34), comparer);
            CompareAssert.IsEqualTo(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16, 65), comparer);
        }

        [TestMethod]
        public void Compare_Shortlex_Arrays()
        {
            var comparer = SequenceComparer<int>.Shortlex;
            CompareAssert.IsEqualTo(new int[] { }, new int[] { }, comparer);
            CompareAssert.IsLessThan(new int[] { }, new int[] { 42 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { 42 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59 }, new int[] { 42, 98 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59, 23 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 42, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 98, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 11 }, comparer);
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34 }, comparer);
            CompareAssert.IsEqualTo(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16 }, comparer);
            CompareAssert.IsLessThan(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16, 65 }, comparer);
        }

        [TestMethod]
        public void Compare_Shortlex_Enumerables()
        {
            var comparer = SequenceComparer<int>.Shortlex;
            CompareAssert.IsEqualTo(Enumerable.Empty<int>(), Enumerable.Empty<int>(), comparer);
            CompareAssert.IsLessThan(Enumerable.Empty<int>(), EnumerableUtility.Yield(42), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), Enumerable.Empty<int>(), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59, 23), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(42, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 98, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 11), comparer);
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34), comparer);
            CompareAssert.IsEqualTo(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16), comparer);
            CompareAssert.IsLessThan(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16, 65), comparer);
        }

        [TestMethod]
        public void Compare_SameLength_Arrays()
        {
            var comparer = SequenceComparer<int>.SameLength;
            CompareAssert.IsEqualTo(new int[] { }, new int[] { }, comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { }, new int[] { 42 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59 }, new int[] { }));
            CompareAssert.IsGreaterThan(new int[] { 59 }, new int[] { 42 }, comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59 }, new int[] { 42, 98 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59 }, new int[] { 42, 98, 11 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23 }, new int[] { 42, 98, 11 }));
            CompareAssert.IsGreaterThan(new int[] { 59, 23, 34 }, new int[] { 42, 98, 11 }, comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23, 34, 16 }, new int[] { 42, 98, 11 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23, 34, 16 }, new int[] { 59, 98, 11 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 11 }));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34 }));
            CompareAssert.IsEqualTo(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16 }, comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(new int[] { 59, 23, 34, 16 }, new int[] { 59, 23, 34, 16, 65 }));
        }

        [TestMethod]
        public void Compare_SameLength_Enumerables()
        {
            var comparer = SequenceComparer<int>.SameLength;
            CompareAssert.IsEqualTo(Enumerable.Empty<int>(), Enumerable.Empty<int>(), comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(Enumerable.Empty<int>(), EnumerableUtility.Yield(42)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59), Enumerable.Empty<int>()));
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42), comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59), EnumerableUtility.Yield(42, 98, 11)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23), EnumerableUtility.Yield(42, 98, 11)));
            CompareAssert.IsGreaterThan(EnumerableUtility.Yield(59, 23, 34), EnumerableUtility.Yield(42, 98, 11), comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(42, 98, 11)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 98, 11)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 11)));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34)));
            CompareAssert.IsEqualTo(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16), comparer);
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(EnumerableUtility.Yield(59, 23, 34, 16), EnumerableUtility.Yield(59, 23, 34, 16, 65)));
        }

        [TestMethod]
        public void Compare_Strings()
        {
            var x = new[] { "Madrid", "Añasco" };
            var y = new[] { "Madrid", "Athens" };

            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                foreach (var comparisonType in EnumUtility.GetValues<SequenceComparison>())
                {
                    var message = "'ñ' should be less than 't' for default comparison (current culture, en-US).";
                    var comparer = SequenceComparer.Create<string>(comparisonType);
                    CompareAssert.IsLessThan(x, y, comparer, message);
                    CompareAssert.IsEqualTo(x, x, comparer);
                    CompareAssert.IsGreaterThan(y, x, comparer, message);

                    message = "'ñ' should be greater than 't' for ordinal comparison.";
                    comparer = SequenceComparer.Create(comparisonType, StringComparer.Ordinal);
                    CompareAssert.IsGreaterThan(x, y, comparer, message);
                    CompareAssert.IsEqualTo(x, x, comparer);
                    CompareAssert.IsLessThan(y, x, comparer, message);
                }
            }
        }

        [TestMethod]
        public void Compare_Nulls()
        {
            var x = new[] { "Madrid", "Añasco" };

            foreach (var comparisonType in EnumUtility.GetValues<SequenceComparison>())
            {
                var comparer = SequenceComparer.Create<string>(comparisonType);
                CompareAssert.IsLessThan(null, x, comparer);
                CompareAssert.IsEqualTo(null, null, comparer);
                CompareAssert.IsGreaterThan(x, null, comparer);
            }
        }

        [TestMethod]
        public void Sorting_Lexicographical()
        {
            var a = new string[0];
            var b = new[] { "Añasco", "Madrid" };
            var c = new[] { "Athens" };
            var d = new[] { "Athens", "Madrid", "Añasco" };
            var e = new[] { "Madrid", "Añasco" };
            var f = new[] { "Madrid", "Paris", "Añasco" };
            var g = new[] { "Madrid", "Paris", "Athens", "New York" };
            var h = new[] { "Paris", "Añasco" };
            var i = new[] { "Paris", "Añasco", "Athens" };
            var j = new[] { "Paris", "Añasco", "Athens", "Madrid", "New York" };
            var k = new[] { "Paris", "Añasco", "Athens", "New York" };
            var l = new[] { "Paris", "Athens" };
            var m = new[] { "Paris", "Athens", "Añasco" };
            
            var source = new[] { h, d, e, l, g, a, m, c, f, j, b, k, i };
            
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                EnumerableAssert.AreEqual(
                    new[] { a, b, c, d, e, f, g, h, i, j, k, l, m },
                    source.OrderBy(s => s, SequenceComparer<string>.Lexicographical));
            
                EnumerableAssert.AreEqual(
                    new[] { a, c, d, b, e, g, f, l, m, h, i, j, k },
                    source.OrderBy(s => s, SequenceComparer.Create(SequenceComparison.Lexicographical, StringComparer.Ordinal)));
            }
        }

        [TestMethod]
        public void Sorting_Shortlex()
        {
            var a = new string[0];
            var b = new[] { "Athens" };
            var c = new[] { "Añasco", "Madrid" };
            var d = new[] { "Madrid", "Añasco" };
            var e = new[] { "Paris", "Añasco" };
            var f = new[] { "Paris", "Athens" };
            var g = new[] { "Athens", "Madrid", "Añasco" };
            var h = new[] { "Madrid", "Paris", "Añasco" };
            var i = new[] { "Paris", "Añasco", "Athens" };
            var j = new[] { "Paris", "Athens", "Añasco" };
            var k = new[] { "Madrid", "Paris", "Athens", "New York" };
            var l = new[] { "Paris", "Añasco", "Athens", "New York" };
            var m = new[] { "Paris", "Añasco", "Athens", "Madrid", "New York" };

            var source = new[] { h, d, e, l, g, a, m, c, f, j, b, k, i };

            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                EnumerableAssert.AreEqual(
                    new[] { a, b, c, d, e, f, g, h, i, j, k, l, m },
                    source.OrderBy(s => s, SequenceComparer<string>.Shortlex));

                EnumerableAssert.AreEqual(
                    new[] { a, b, c, d, f, e, g, h, j, i, k, l, m },
                    source.OrderBy(s => s, SequenceComparer.Create(SequenceComparison.Shortlex, StringComparer.Ordinal)));
            }
        }
    }
}
