using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class SequenceComparerTests
    {
        [TestMethod]
        public void Compare()
        {
            var x = new[] { "Madrid", "Añasco" };
            var y = new[] { "Madrid", "Athens" };

            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var message = "'ñ' should be less than 't' for default comparison (current culture, en-US).";
                var comparer = SequenceComparer<string>.Default;
                CompareAssert.IsLessThan(x, y, comparer, message);
                CompareAssert.IsEqualTo(x, x, comparer);
                CompareAssert.IsGreaterThan(y, x, comparer, message);

                message = "'ñ' should be greater than 't' for ordinal comparison.";
                comparer = new SequenceComparer<string>(StringComparer.Ordinal);
                CompareAssert.IsGreaterThan(x, y, comparer, message);
                CompareAssert.IsEqualTo(x, x, comparer);
                CompareAssert.IsLessThan(y, x, comparer, message);
            }
        }

        [TestMethod]
        public void Compare_Nulls()
        {
            var x = new[] { "Madrid", "Añasco" };

            var comparer = SequenceComparer<string>.Default;
            CompareAssert.IsLessThan(null, x, comparer);
            CompareAssert.IsEqualTo(null, null, comparer);
            CompareAssert.IsGreaterThan(x, null, comparer);
        }

        [TestMethod]
        public void Sorting()
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
                    source.OrderBy(s => s, SequenceComparer<string>.Default));

                EnumerableAssert.AreEqual(
                    new[] { a, c, d, b, e, g, f, l, m, h, i, j, k },
                    source.OrderBy(s => s, new SequenceComparer<string>(StringComparer.Ordinal)));
            }
        }
    }
}
