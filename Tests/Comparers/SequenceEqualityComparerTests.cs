using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Collections;
using DogmaMix.Core.Globalization;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class SequenceEqualityComparerTests
    {
        private static IEnumerable<string> x = new[] { "Madrid", "Añasco" };
        private static IEnumerable<string> y = new[] { "Madrid", "Añasco", "Paris" };
        private static IEnumerable<string> z = new[] { "Madrid", "AÑASCO", "Paris" };

        [TestMethod]
        public void Equals()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var comparer = new SequenceEqualityComparer<string>(StringComparer.CurrentCulture);
                EqualityAssert.NotEquals(x, y, comparer);
                EqualityAssert.NotEquals(y, z, comparer);

                comparer = new SequenceEqualityComparer<string>(StringComparer.CurrentCultureIgnoreCase);
                EqualityAssert.NotEquals(x, y, comparer);
                EqualityAssert.Equals(y, z, comparer);
            }
        }

        [TestMethod]
        public void Equals_DifferentTypes()
        {
            var comparer = SequenceEqualityComparer<string>.Default;
            EqualityAssert.Equals(x.ToArray(), x.ToList(), comparer);
            EqualityAssert.Equals(y.ToArray(), y.Select(s => s), comparer);
        }

        [TestMethod]
        public void Equals_Nulls()
        {
            var comparer = SequenceEqualityComparer<string>.Default;
            EqualityAssert.NotEquals(null, x, comparer);
            EqualityAssert.Equals(null, null, comparer);
            EqualityAssert.NotEquals(x, null, comparer);
        }

        [TestMethod]
        public void Equals_Empty()
        {
            var empty = ArrayUtility<string>.Empty;
            var comparer = SequenceEqualityComparer<string>.Default;
            EqualityAssert.NotEquals(empty, null, comparer);
            EqualityAssert.NotEquals(empty, x, comparer);
            EqualityAssert.Equals(empty, x.Take(0), comparer);
            EqualityAssert.Equals(empty, empty, comparer);
        }

        [TestMethod]
        public new void GetHashCode()
        {
            using (new CultureSwapper(PredefinedCulture.EnglishUnitedStates))
            {
                var comparer = new SequenceEqualityComparer<string>(StringComparer.CurrentCultureIgnoreCase);
                Assert.AreEqual(
                    comparer.GetHashCode(y),
                    comparer.GetHashCode(z));
            }
        }
    }
}
