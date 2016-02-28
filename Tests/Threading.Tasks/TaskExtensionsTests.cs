using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public class TaskExtensionsTests
    {
        [TestMethod]
        public void GetResult()
        {
            string status = "initial";
            var task = Task.Run(() =>
            {
                status = "executed";
            });

            task.GetResult();

            Assert.AreEqual("executed", status);
        }

        [TestMethod]
        public void GetResult_TResult()
        {
            string status = "initial";
            var task = Task.Run(() =>
            {
                status = "executed";
                return 42;
            });

            var result = task.GetResult();

            Assert.AreEqual("executed", status);
            Assert.AreEqual(42, result);
        }
    }
}