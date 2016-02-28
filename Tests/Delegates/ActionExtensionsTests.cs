using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class ActionExtensionsTests
    {
        [TestMethod]
        public void Return()
        {
            string status = "initial";
            Action action = () => status = "executed";

            var func = action.Return(44);
            var result = func();

            Assert.AreEqual("executed", status);
            Assert.AreEqual(44, result);
        }

        [TestMethod]
        public void Return_5()
        {
            string status = "initial";
            Action<int, char, string, double, byte> action = (p1, p2, p3, p4, p5) => status = $"{p1} {p2} {p3} {p4:F2} {p5}";

            var func = action.Return(48);
            var result = func(365, 'x', "foo", 3.14159, 255);

            Assert.AreEqual("365 x foo 3.14 255", status);
            Assert.AreEqual(48, result);
        }
        
        [TestMethod]
        public async Task WrapAsync()
        {
            string status = "initial";
            Action action = () => status = "executed";

            await action.WrapAsync()();

            Assert.AreEqual("executed", status);
        }

        [TestMethod]
        public void WrapAsync_SyncException()
        {
            Action action = () => { throw new InvalidOperationException(); };

            var asyncFunc = action.WrapAsync();
            
            ExceptionAssert.Throws<InvalidOperationException>(() => asyncFunc(), "Exception was not delivered synchronously.");
        }

        [TestMethod]
        public async Task WrapAsync_AsyncException()
        {
            Action action = () => { throw new InvalidOperationException(); };

            var asyncFunc = action.WrapAsync();

            await ExceptionAssert.ThrowsAsync<InvalidOperationException>(() => asyncFunc(), "Exception was not thrown from asynchronous operation.");
        }
    }
}
