using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Collections.Tests
{
    [TestClass]
    public class EnumeratorUtilityTests
    {
        [TestMethod]
        public void MoveNextBoth()
        {
            int lengthComparison;

            using (var x = Enumerable.Empty<int>().GetEnumerator())
            using (var y = Enumerable.Empty<int>().GetEnumerator())
            {
                Assert.IsFalse(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(0, lengthComparison);
            }

            using (var x = Enumerable.Empty<int>().GetEnumerator())
            using (var y = EnumerableUtility.Yield(42).GetEnumerator())
            {
                Assert.IsFalse(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(-1, Math.Sign(lengthComparison));
            }

            using (var x = EnumerableUtility.Yield(42).GetEnumerator())
            using (var y = Enumerable.Empty<int>().GetEnumerator())
            {
                Assert.IsFalse(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(1, Math.Sign(lengthComparison));
            }

            using (var x = EnumerableUtility.Yield(42).GetEnumerator())
            using (var y = EnumerableUtility.Yield(55).GetEnumerator())
            {
                Assert.IsTrue(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(0, lengthComparison);
                Assert.IsFalse(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(0, lengthComparison);
            }

            using (var x = EnumerableUtility.Yield(42, 45, 48, 49).GetEnumerator())
            using (var y = EnumerableUtility.Yield(55, 56, 57, 58, 59).GetEnumerator())
            {
                for (int i = 0; i < 4; i++)
                {
                    Assert.IsTrue(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                    Assert.AreEqual(0, lengthComparison);
                }

                Assert.IsFalse(EnumeratorUtility.MoveNextBoth(x, y, out lengthComparison));
                Assert.AreEqual(-1, Math.Sign(lengthComparison));
            }
        }
    }
}