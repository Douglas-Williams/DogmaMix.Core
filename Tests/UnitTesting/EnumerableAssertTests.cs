using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.UnitTesting.Tests
{
    [TestClass]
    public class EnumerableAssertTests
    {
        [TestMethod]
        public void IsEmpty()
        {
            var bytes = new byte[] { 243, 75, 31, 139, 26 };
            EnumerableAssert.IsEmpty(new byte[0]);
            EnumerableAssert.IsEmpty(bytes.Where(b => b == 0));
            ExceptionAssert.ThrowsAssertFailed(() => EnumerableAssert.IsEmpty(bytes));
            ExceptionAssert.ThrowsAssertFailed(() => EnumerableAssert.IsEmpty(bytes.Where(b => b == 31)));
        }

        [TestMethod]
        public void HasCount()
        {
            var bytes = new byte[] { 243, 75, 31, 139, 26 };
            EnumerableAssert.HasCount(0, new byte[0]);
            EnumerableAssert.HasCount(5, bytes);
            EnumerableAssert.HasCount(3, bytes.Where(b => b < 100));
            ExceptionAssert.ThrowsAssertFailed(() => EnumerableAssert.HasCount(6, bytes));
            ExceptionAssert.ThrowsAssertFailed(() => EnumerableAssert.HasCount(2, bytes.Where(b => b < 100)));
        }
    }
}
