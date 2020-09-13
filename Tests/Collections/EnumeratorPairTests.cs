using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Collections;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Collections.Tests
{
    [TestClass]
    public class EnumeratorPairTests
    {
        [TestMethod]
        public void MoveNext()
        {
            var x = Enumerable.Empty<int>();
            var y = Enumerable.Empty<int>();
            int lengthComparison;

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                Assert.IsFalse(enumeratorPair.MoveNext());
                Assert.AreEqual(0, enumeratorPair.LengthComparison);
            }

            x = Enumerable.Empty<int>();
            y = EnumerableUtility.Yield(42);

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                Assert.IsFalse(enumeratorPair.MoveNext());
                Assert.AreEqual(42, enumeratorPair.Current2);
                Assert.AreEqual(-1, Math.Sign(enumeratorPair.LengthComparison));
            }

            x = EnumerableUtility.Yield(42);
            y = Enumerable.Empty<int>();

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                Assert.IsFalse(enumeratorPair.MoveNext());
                Assert.AreEqual(42, enumeratorPair.Current1);
                Assert.AreEqual(1, Math.Sign(enumeratorPair.LengthComparison));
            }

            x = EnumerableUtility.Yield(42);
            y = EnumerableUtility.Yield(55);

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                Assert.IsTrue(enumeratorPair.MoveNext());
                Assert.AreEqual(42, enumeratorPair.Current1);
                Assert.AreEqual(55, enumeratorPair.Current2);
                Assert.AreEqual(42, enumeratorPair.Current.Element1);
                Assert.AreEqual(55, enumeratorPair.Current.Element2);
                ExceptionAssert.Throws<InvalidOperationException>(() =>
                    lengthComparison = enumeratorPair.LengthComparison);

                Assert.IsFalse(enumeratorPair.MoveNext());
                Assert.AreEqual(0, enumeratorPair.LengthComparison);
            }

            x = EnumerableUtility.Yield(42, 43, 45, 47, 49);
            y = EnumerableUtility.Yield(55, 56, 57, 58);

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                for (int i = 0; i < 4; i++)
                {
                    Assert.IsTrue(enumeratorPair.MoveNext());
                    Assert.AreEqual(x.ElementAt(i), enumeratorPair.Current1);
                    Assert.AreEqual(y.ElementAt(i), enumeratorPair.Current2);
                    Assert.AreEqual(x.ElementAt(i), enumeratorPair.Current.Element1);
                    Assert.AreEqual(y.ElementAt(i), enumeratorPair.Current.Element2);
                    ExceptionAssert.Throws<InvalidOperationException>(() =>
                        lengthComparison = enumeratorPair.LengthComparison);
                }

                Assert.IsFalse(enumeratorPair.MoveNext());
                Assert.AreEqual(1, Math.Sign(enumeratorPair.LengthComparison));
            }
        }

        [TestMethod]
        public void Reset()
        {
            // Calling reset on an iterator throws NotSupportedException.
            var x = EnumerableUtility.Yield(42, 43, 45, 47, 49).ToList();
            var y = EnumerableUtility.Yield(55, 56, 57, 58).ToList();

            using (var enumeratorPair = new EnumeratorPair<int>(x, y))
            {
                for (int run = 0; run < 3; run++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int lengthComparison;
                        Assert.IsTrue(enumeratorPair.MoveNext());
                        Assert.AreEqual(x.ElementAt(i), enumeratorPair.Current1);
                        Assert.AreEqual(y.ElementAt(i), enumeratorPair.Current2);
                        ExceptionAssert.Throws<InvalidOperationException>(() =>
                            lengthComparison = enumeratorPair.LengthComparison);
                    }

                    Assert.IsFalse(enumeratorPair.MoveNext());
                    Assert.AreEqual(1, Math.Sign(enumeratorPair.LengthComparison));

                    enumeratorPair.Reset();
                }
            }
        }
    }
}