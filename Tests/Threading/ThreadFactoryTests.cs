using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DogmaMix.Core.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Threading.Tests
{
    [TestClass]
    public class ThreadFactoryTests
    {
        [TestMethod]
        public void StartNew()
        {
            bool executed = false;
            int threadId = 0;
            var thread = ThreadFactory.StartNew(() =>
            {
                executed = true;
                threadId = Thread.CurrentThread.ManagedThreadId;
            });
            thread.Join();

            Assert.IsTrue(executed);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
        }

        [TestMethod]
        public void StartNew_Parameterized()
        {
            bool executed = false;
            string argument = null;
            int threadId = 0;
            var thread = ThreadFactory.StartNew("Test", str =>
            {
                executed = true;
                argument = str;
                threadId = Thread.CurrentThread.ManagedThreadId;
            });
            thread.Join();

            Assert.IsTrue(executed);
            Assert.AreEqual("Test", argument);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
        }

        [TestMethod]
        public void StartNew_Multiple()
        {
            const int threadCount = 32;

            var nums = new ConcurrentBag<int>();
            var threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
                threads[i] = ThreadFactory.StartNew(i, nums.Add);
            for (int i = 0; i < threadCount; i++)
                threads[i].Join();

            EnumerableAssert.AreEquivalent(Enumerable.Range(0, threadCount), nums);

            nums = new ConcurrentBag<int>();

            Enumerable.Range(0, threadCount)
                      .Select(i => ThreadFactory.StartNew(() => nums.Add(i)))
                      .ToList()
                      .ForEach(thread => thread.Join());

            EnumerableAssert.AreEquivalent(Enumerable.Range(0, threadCount), nums);
        }
    }
}