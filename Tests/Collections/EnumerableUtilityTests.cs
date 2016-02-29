using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Collections.Tests
{
    [TestClass]
    public class EnumerableUtilityTests
    {
        [TestMethod]
        public void Yield()
        {
            EnumerableAssert.AreEqual(new object[] { null }, EnumerableUtility.Yield((object)null));
            EnumerableAssert.AreEqual(new int?[] { null }, EnumerableUtility.Yield((int?)null));
            EnumerableAssert.AreEqual(new[] { 42 }, EnumerableUtility.Yield(42));
        }

        [TestMethod]
        public void Yield_Multiple()
        {
            EnumerableAssert.AreEqual(new int?[] { null, 42 }, EnumerableUtility.Yield<int?>(null, 42));
            EnumerableAssert.AreEqual(new[] { 42, 65, 12 }, EnumerableUtility.Yield(42, 65, 12));
        }
    }
}