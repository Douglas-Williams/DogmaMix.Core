using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            bool isFinalized = false;
            CreateDisposable(() => isFinalized = true);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(isFinalized);            
        }

        // Object needs to be created in a non-inlined helper method for it to get garbage-collected.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CreateDisposable(Action onFinalize)
        {
            var disposable = new SampleFinalizableDisposable(onFinalize);
        }

        private class SampleFinalizableDisposable : FinalizableDisposable
        {
            private readonly Action _onFinalize;

            public SampleFinalizableDisposable(Action onFinalize)
            {
                _onFinalize = onFinalize;
            }

            protected override void Dispose(bool disposing)
            {
                Assert.IsFalse(HasDisposeStarted);
                _onFinalize();
            }
        }
    }
}