using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    [TestClass]
    public partial class DisposableExtensionsTests
    {
        private class SampleDisposable : IDisposable
        {
            public bool WasDisposeCalled { get; set; }
            public bool ThrowDisposeException { get; set; }
            
            public SampleDisposable(bool throwDispose = false)
            {
                ThrowDisposeException = throwDispose;
            }

            public void Dispose()
            {
                WasDisposeCalled = true;

                if (ThrowDisposeException)
                    throw new DisposeException();
            }
        }
        
        private class MainLogicException : Exception
        { }

        private class DisposeException : Exception
        { }
    }
}