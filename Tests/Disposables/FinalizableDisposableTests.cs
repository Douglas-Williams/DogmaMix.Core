using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Disposables.Tests
{
    [TestClass]
    public class FinalizableDisposableTests
    {
        [TestMethod]
        public void FinalizeDisposable()
        {
            var disposable = new SampleFinalizableDisposable();
            bool isFinalized = false;
            disposable.OnFinalize = () => 
                isFinalized = true;

            disposable = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(isFinalized);            
        }

        private class SampleFinalizableDisposable : FinalizableDisposable
        {
            public Action OnFinalize;

            protected override void Dispose(bool disposing)
            {
                Assert.IsFalse(HasDisposeStarted);
                OnFinalize();
            }
        }
    }
}