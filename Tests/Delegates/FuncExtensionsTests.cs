using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class FuncExtensionsTests
    {        
        [TestMethod]
        public async Task WrapAsync()
        {
            string status = "initial";
            Func<int> func = () =>
            {
                status = "executed";
                return 42;
            };

            var result = await func.WrapAsync()();

            Assert.AreEqual("executed", status);
            Assert.AreEqual(42, result);
        }
    }
}
