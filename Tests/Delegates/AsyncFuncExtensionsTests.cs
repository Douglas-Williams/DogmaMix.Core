using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class AsyncFuncExtensionsTests
    {
        [TestMethod]
        public async Task DeliverAsync_NoException()
        {
#pragma warning disable 1998
            Func<int, string, char, Task<string>> asyncAction = async (p1, p2, p3) => $"{p1} {p2} {p3}";
#pragma warning restore 1998

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver(42, "foo", 'X');
            var result = await task;

            Assert.AreEqual("42 foo X", result);
        }

        [TestMethod]
        public void DeliverAsync_SyncException()
        {
            Func<int, string, char, Task<string>> asyncAction = (p1, p2, p3) => { throw new InvalidOperationException(); };

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver(42, "foo", 'X');   // exception would otherwise be thrown here

            ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => task);
        }

        [TestMethod]
        public void DeliverAsync_AsyncException()
        {
#pragma warning disable 1998
            Func<int, string, char, Task<string>> asyncAction = async (p1, p2, p3) => { throw new InvalidOperationException(); };
#pragma warning restore 1998

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver(42, "foo", 'X');

            ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => task);
        }
    }
}