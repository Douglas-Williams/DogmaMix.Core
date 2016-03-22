using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Comparers.Tests
{
    [TestClass]
    public class EqualityComparerBaseTests
    {
        private const string allHashCodesZero =
            "Hash codes for all values were computed as 0. This should be highly unlikely to occur.";

        [TestMethod]
        public void Equals_NullableInt()
        {
            var comparer = new NullableIntComparer();
            EqualityAssert.Equals(null, null, comparer);
            EqualityAssert.NotEquals(null, 4, comparer);
            EqualityAssert.NotEquals(4, null, comparer);
            EqualityAssert.Equals(4, 4, comparer);
            EqualityAssert.NotEquals(4, 7, comparer);
        }

        [TestMethod]
        public void Equals_NullableInt_NonGeneric()
        {
            IEqualityComparer comparer = new NullableIntComparer();
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsFalse(comparer.Equals(null, 4));
            Assert.IsFalse(comparer.Equals(4, null));
            Assert.IsTrue(comparer.Equals(4, 4));
            Assert.IsFalse(comparer.Equals(4, 7));

            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals(null, "abc"));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals("abc", null));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals("abc", "abc"));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals("abc", "xyz"));
        }

        [TestMethod]
        public void Equals_String()
        {
            var comparer = new OrdinalStringComparer();
            EqualityAssert.Equals(null, null, comparer);
            EqualityAssert.NotEquals(null, "abc", comparer);
            EqualityAssert.NotEquals("abc", null, comparer);
            EqualityAssert.Equals("abc", "abc", comparer);
            EqualityAssert.NotEquals("abc", "xyz", comparer);
        }

        [TestMethod]
        public void Equals_String_NonGeneric()
        {
            IEqualityComparer comparer = new OrdinalStringComparer();
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsFalse(comparer.Equals(null, "abc"));
            Assert.IsFalse(comparer.Equals("abc", null));
            Assert.IsTrue(comparer.Equals("abc", "abc"));
            Assert.IsFalse(comparer.Equals("abc", "xyz"));

            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals(null, 4));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals(4, null));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals(4, 4));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Equals(4, 7));
        }
        
        [TestMethod]
        public void GetHashCode_NullableInt()
        {
            var comparer = new NullableIntComparer();
            Assert.AreEqual(0, comparer.GetHashCode(null));

            var anyNonZero = new int?[] { 3, 7, 12, 45 }
                .Select(comparer.GetHashCode)
                .Any(hc => hc != 0);
            Assert.IsTrue(anyNonZero, allHashCodesZero);
        }

        [TestMethod]
        public void GetHashCode_NullableInt_NonGeneric()
        {
            IEqualityComparer comparer = new NullableIntComparer();
            Assert.AreEqual(0, comparer.GetHashCode(null));

            var anyNonZero = new int?[] { 3, 7, 12, 45 }
                .Select(x => comparer.GetHashCode(x))
                .Any(hc => hc != 0);
            Assert.IsTrue(anyNonZero, allHashCodesZero);

            ExceptionAssert.Throws<ArgumentException>(() => comparer.GetHashCode("abc"));
        }

        [TestMethod]
        public void GetHashCode_String()
        {
            var comparer = new OrdinalStringComparer();
            Assert.AreEqual(0, comparer.GetHashCode(null));

            var anyNonZero = new [] { "abc", "xyz", "pqr" }
                .Select(comparer.GetHashCode)
                .Any(hc => hc != 0);
            Assert.IsTrue(anyNonZero, allHashCodesZero);
        }

        [TestMethod]
        public void GetHashCode_String_NonGeneric()
        {
            IEqualityComparer comparer = new OrdinalStringComparer();
            Assert.AreEqual(0, comparer.GetHashCode(null));

            var anyNonZero = new[] { "abc", "xyz", "pqr" }
                .Select(comparer.GetHashCode)
                .Any(hc => hc != 0);
            Assert.IsTrue(anyNonZero, allHashCodesZero);

            ExceptionAssert.Throws<ArgumentException>(() => comparer.GetHashCode(4));
        }

        private class NullableIntComparer : EqualityComparerBase<int?>
        {
            public override bool EqualsNonNull(int? x, int? y)
            {
                if (x == null)
                    Assert.Fail($"{nameof(x)} is null.");
                if (y == null)
                    Assert.Fail($"{nameof(y)} is null.");

                return x.Value.Equals(y.Value);
            }

            public override int GetHashCodeNonNull(int? obj)
            {
                if (obj == null)
                    Assert.Fail($"{nameof(obj)} is null.");

                return obj.Value.GetHashCode();
            }
        }

        private class OrdinalStringComparer : EqualityComparerBase<string>
        {
            public override bool EqualsNonNull(string x, string y)
            {
                if (x == null)
                    Assert.Fail($"{nameof(x)} is null.");
                if (y == null)
                    Assert.Fail($"{nameof(y)} is null.");

                return string.Equals(x, y, StringComparison.Ordinal);
            }

            public override int GetHashCodeNonNull(string obj)
            {
                if (obj == null)
                    Assert.Fail($"{nameof(obj)} is null.");

                return obj.GetHashCode();
            }
        }
    }
}