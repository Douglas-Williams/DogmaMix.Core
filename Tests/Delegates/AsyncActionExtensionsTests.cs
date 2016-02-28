using DogmaMix.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class AsyncActionExtensionsTests
    {
        [TestMethod]
        public async Task ReturnAsync()
        {
            string status = "initial";
#pragma warning disable 1998
            Func<Task> asyncAction = async () => { status = "executed"; };
#pragma warning restore 1998

            var asyncFunc = asyncAction.ReturnAsync(42);
            var result = await asyncFunc();

            Assert.AreEqual("executed", status);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void Return_SyncException()
        {
            Func<Task> asyncAction = () => { throw new InvalidOperationException(); };

            var asyncFunc = asyncAction.ReturnAsync(42);

            ExceptionAssert.Throws<InvalidOperationException>(() => asyncFunc(), "Exception was not delivered synchronously.");
        }

        [TestMethod]
        public async Task ReturnAsync_AsyncException()
        {
            Func<Task> asyncAction = () => { throw new InvalidOperationException(); };

            var asyncFunc = asyncAction.ReturnAsync(42);

            await ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => asyncFunc(), "Exception was not thrown from asynchronous operation.");
        }

        [TestMethod]
        public void ReturnAsync_WriteAllBytesAsync()
        {
            Func<Task> asyncAction = () => WriteAllBytesAsync(null, null);

            var asyncFunc = asyncAction.ReturnAsync(42);

            ExceptionAssert.Throws<ArgumentNullException>(() => asyncFunc(), "Exception was not delivered synchronously.");
        }

        [TestMethod]
        public void DeliverAsync_SyncException()
        {
            Func<Task> asyncAction = () => { throw new InvalidOperationException(); };

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver();   // exception would otherwise be thrown here

            ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => task);
        }

        [TestMethod]
        public void DeliverAsync_AsyncException()
        {
#pragma warning disable 1998
            Func<Task> asyncAction = async () => { throw new InvalidOperationException(); };
#pragma warning restore 1998

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver();

            ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => task);
        }

        [TestMethod]
        public void DeliverAsync_WriteAllBytesAsync()
        {
            Func<Task> asyncAction = () => WriteAllBytesAsync(null, null);

            var asyncDeliver = asyncAction.DeliverAsync();
            var task = asyncDeliver();   // exception would otherwise be thrown here

            ExceptionAssert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        private static Task WriteAllBytesAsync(string filePath, byte[] bytes)
        {
            if (filePath == null)
                throw new ArgumentNullException(filePath, nameof(filePath));
            if (bytes == null)
                throw new ArgumentNullException(filePath, nameof(bytes));

            return WriteAllBytesAsyncInner(filePath, bytes);
        }

        private static async Task WriteAllBytesAsyncInner(string filePath, byte[] bytes)
        {
            using (var fileStream = File.OpenWrite(filePath))
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}