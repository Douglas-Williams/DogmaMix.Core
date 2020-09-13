using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Disposables.Tests
{
    [TestClass]
    public class DisposableTests
    {
        [TestMethod]
        public void Dispose()
        {
            var disposable = new SampleDisposable();
            Assert.IsFalse(disposable.HasDisposeStarted);
            Assert.IsFalse(disposable.HasDisposeCompleted);
            Assert.IsFalse(disposable.HasDisposeFailed);

            disposable.Dispose();
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsTrue(disposable.HasDisposeCompleted);
            Assert.IsFalse(disposable.HasDisposeFailed);
            Assert.AreEqual(1, disposable.DisposeCount);

            disposable.Dispose();
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsTrue(disposable.HasDisposeCompleted);
            Assert.IsFalse(disposable.HasDisposeFailed);
            Assert.AreEqual(1, disposable.DisposeCount);

            disposable = new SampleDisposable();
            disposable.ThrowException = true;
            ExceptionAssert.Throws<InvalidOperationException>(() => disposable.Dispose());
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsFalse(disposable.HasDisposeCompleted);
            Assert.IsTrue(disposable.HasDisposeFailed);
            Assert.AreEqual(1, disposable.DisposeCount);

            ExceptionAssert.Throws<InvalidOperationException>(() => disposable.Dispose());
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsFalse(disposable.HasDisposeCompleted);
            Assert.IsTrue(disposable.HasDisposeFailed);
            Assert.AreEqual(2, disposable.DisposeCount);

            disposable.ThrowException = false;
            disposable.Dispose();
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsTrue(disposable.HasDisposeCompleted);
            Assert.IsFalse(disposable.HasDisposeFailed);
            Assert.AreEqual(3, disposable.DisposeCount);

            disposable.Dispose();
            Assert.IsTrue(disposable.HasDisposeStarted);
            Assert.IsTrue(disposable.HasDisposeCompleted);
            Assert.IsFalse(disposable.HasDisposeFailed);
            Assert.AreEqual(3, disposable.DisposeCount);
        }

        [TestMethod]
        public void ThrowIfDisposed()
        {
            var disposable = new SampleDisposable();
            disposable.DoSomething();
            disposable.Dispose();
            var exception = ExceptionAssert.Throws<ObjectDisposedException>(() => disposable.DoSomething());
            Assert.AreEqual(nameof(SampleDisposable), exception.ObjectName);
        }

        private class SampleDisposable : Disposable
        {
            public int DisposeCount { get; private set; }
            public bool ThrowException { get; set; }
            
            protected override void Dispose(bool disposing)
            {
                Assert.IsTrue(disposing);
                Assert.IsTrue(HasDisposeStarted);
                Assert.IsFalse(HasDisposeCompleted);

                DisposeCount++;

                if (ThrowException)
                    throw new InvalidOperationException();
            }

            public void DoSomething()
            {
                ThrowIfDisposed();
            }
        }
    }
}