using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Comparers;
using DogmaMix.Core.Extensions;
using DogmaMix.Core.Threading;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Randomization.Tests
{
    [TestClass]
    public class CachedRandomTests
    {
        [TestMethod]
        public void Default()
        {
            Assert.IsNotNull(CachedRandom.Current);

            const int length = 255;
            var randoms = new ConcurrentBag<Random>();
            var sequences = new ConcurrentBag<byte[]>();

            Enumerable
                .Range(0, 8)
                .Select(_ => ThreadFactory.StartNew(() =>
                {
                    randoms.Add(CachedRandom.Current);
                    sequences.Add(CachedRandom.Current.NextBytes(length));
                }))
                .ToList()
                .ForEach(thread => thread.Join());

            EnumerableAssert.HasCount(8, randoms);
            EnumerableAssert.AllItemsAreUnique(randoms);

            foreach (var x in sequences)
                Assert.IsTrue(x.Distinct().Count() >= length / 5,
                    "Less than 20% of the elements are unique. This is highly unlikely to occur in a random sequence.");

            foreach (var x in sequences)
                foreach (var y in sequences)
                    if (!object.ReferenceEquals(x, y))
                        EqualityAssert.NotEquals(x, y, SequenceEqualityComparer<byte>.Default,
                            "Two sequences are identical. This is highly unlikely to occur in random sequences.");
        }
    }
}
