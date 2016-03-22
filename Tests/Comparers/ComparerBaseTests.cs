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
    public class ComparerBaseTests
    {
        [TestMethod]
        public void Compare_NullableInt()
        {
            var comparer = new NullableIntComparer();
            CompareAssert.IsEqualTo(null, null, comparer);
            CompareAssert.IsLessThan(null, 4, comparer);
            CompareAssert.IsGreaterThan(4, null, comparer);
            CompareAssert.IsEqualTo(4, 4, comparer);
            CompareAssert.IsLessThan(4, 7, comparer);
            CompareAssert.IsGreaterThan(4, 2, comparer);
        }

        [TestMethod]
        public void Compare_NullableInt_NonGeneric()
        {
            IComparer comparer = new NullableIntComparer();
            Assert.AreEqual(0, Math.Sign(comparer.Compare(null, null)));
            Assert.AreEqual(-1, Math.Sign(comparer.Compare(null, 4)));
            Assert.AreEqual(1, Math.Sign(comparer.Compare(4, null)));
            Assert.AreEqual(0, Math.Sign(comparer.Compare(4, 4)));
            Assert.AreEqual(-1, Math.Sign(comparer.Compare(4, 7)));
            Assert.AreEqual(1, Math.Sign(comparer.Compare(4, 2)));

            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(null, "abc"));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare("abc", null));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare("abc", "abc"));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare("abc", "xyz"));
        }

        [TestMethod]
        public void Compare_String()
        {
            var comparer = new OrdinalStringComparer();
            CompareAssert.IsEqualTo(null, null, comparer);
            CompareAssert.IsLessThan(null, "abc", comparer);
            CompareAssert.IsGreaterThan("abc", null, comparer);
            CompareAssert.IsEqualTo("abc", "abc", comparer);
            CompareAssert.IsLessThan("abc", "xyz", comparer);
            CompareAssert.IsGreaterThan("abc", "a", comparer);
        }

        [TestMethod]
        public void Compare_String_NonGeneric()
        {
            IComparer comparer = new OrdinalStringComparer();
            Assert.AreEqual(0, Math.Sign(comparer.Compare(null, null)));
            Assert.AreEqual(-1, Math.Sign(comparer.Compare(null, "abc")));
            Assert.AreEqual(1, Math.Sign(comparer.Compare("abc", null)));
            Assert.AreEqual(0, Math.Sign(comparer.Compare("abc", "abc")));
            Assert.AreEqual(-1, Math.Sign(comparer.Compare("abc", "xyz")));
            Assert.AreEqual(1, Math.Sign(comparer.Compare("abc", "a")));

            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(null, 4));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(4, null));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(4, 4));
            ExceptionAssert.Throws<ArgumentException>(() => comparer.Compare(4, 7));
        }

        private class NullableIntComparer : ComparerBase<int?>
        {
            protected override int CompareNonNull(int? x, int? y)
            {
                if (x == null)
                    Assert.Fail($"{nameof(x)} is null.");
                if (y == null)
                    Assert.Fail($"{nameof(y)} is null.");

                return x.Value.CompareTo(y.Value);
            }
        }

        private class OrdinalStringComparer : ComparerBase<string>
        {
            protected override int CompareNonNull(string x, string y)
            {
                if (x == null)
                    Assert.Fail($"{nameof(x)} is null.");
                if (y == null)
                    Assert.Fail($"{nameof(y)} is null.");

                return string.Compare(x, y, StringComparison.Ordinal);
            }
        }
    }
}