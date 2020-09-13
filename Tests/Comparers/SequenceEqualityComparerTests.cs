using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
                var comparer = SequenceEqualityComparer.Create(StringComparer.CurrentCulture);
                EqualityAssert.NotEquals(x, y, comparer);
                EqualityAssert.NotEquals(y, z, comparer);

                comparer = SequenceEqualityComparer.Create(StringComparer.CurrentCultureIgnoreCase);
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
            var empty = Array.Empty<string>();
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
                var comparer = SequenceEqualityComparer.Create(StringComparer.CurrentCultureIgnoreCase);
                Assert.AreEqual(
                    comparer.GetHashCode(y),
                    comparer.GetHashCode(z));
            }
        }

        [TestMethod]
        public void Serialize_BinaryFormatter()
        {
            var x = new[] { "abc", "def" };
            var y = new[] { "abc", "DEF" };
            var z = new[] { "xyz" };

            var binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                var original = SequenceEqualityComparer.Create(StringComparer.OrdinalIgnoreCase);
                binaryFormatter.Serialize(stream, original);
                stream.Position = 0;

                var comparer = (SequenceEqualityComparer<string>)binaryFormatter.Deserialize(stream);
                EqualityAssert.Equals(x, y, comparer);
                EqualityAssert.NotEquals(x, z, comparer);
            }
        }
    }
}
