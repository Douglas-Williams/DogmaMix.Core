using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Extensions;
using DogmaMix.Core.Threading;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Randomization.Tests
{
    [TestClass]
    public class RandomExtensionsTests
    {
        [TestMethod]
        public void GetBytes()
        {
            var length = 16 * 1024;
            Random random = new Random();
            var bytes = random.NextBytes(length);
            Assert.IsNotNull(bytes);
            EnumerableAssert.HasCount(length, bytes);

            length = 255;
            bytes = random.NextBytes(length);
            EnumerableAssert.HasCount(length, bytes);
            Assert.IsTrue(bytes.Distinct().Count() >= length / 5, "Less than 20% of the elements are unique. This is highly unlikely to occur in a random sequence.");
        }
    }
}