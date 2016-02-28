﻿ 

//----------------------------------------------------------------------------------------------------
// <auto-generated> 
//     This code was generated by a T4 template: DisposableExtensionsTests.Auto.tt
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// </auto-generated> 
//----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogmaMix.Core.Disposables;
using DogmaMix.Core.Types;
using DogmaMix.Core.UnitTesting;

namespace DogmaMix.Core.Extensions.Tests
{
    public partial class DisposableExtensionsTests
    {
        [TestMethod]
        public void Using()
        {
            bool throwMain = false;
            bool throwDispose = false;
            foreach (var strategy in EnumUtility.GetValues<DisposeExceptionStrategy>())
                TestStrategyNoException(strategy, throwMain, throwDispose);

            throwMain = true;
            throwDispose = false;
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose);
            TestStrategy<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
                Assert.IsInstanceOfType(e.InnerExceptions.Single(), typeof(MainLogicException)));

            throwMain = false;
            throwDispose = true;
            TestStrategy<DisposeException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            TestStrategyNoException(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            TestStrategy<DisposeException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            TestStrategy<DisposeException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose);
            TestStrategy<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
                Assert.IsInstanceOfType(e.InnerExceptions.Single(), typeof(DisposeException)));

            throwMain = true;
            throwDispose = true;
            TestStrategy<DisposeException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            TestStrategy<MainLogicException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            TestStrategy<AggregateException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose, e =>
            {
                EnumerableAssert.HasCount(2, e.InnerExceptions);
                Assert.IsInstanceOfType(e.InnerExceptions[0], typeof(MainLogicException));
                Assert.IsInstanceOfType(e.InnerExceptions[1], typeof(DisposeException));
            });
            TestStrategy<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
            {
                EnumerableAssert.HasCount(2, e.InnerExceptions);
                Assert.IsInstanceOfType(e.InnerExceptions[0], typeof(MainLogicException));
                Assert.IsInstanceOfType(e.InnerExceptions[1], typeof(DisposeException));
            });
        }

        private static void TestStrategyNoException(DisposeExceptionStrategy strategy, bool throwMain, bool throwDispose)
        {
            // No return value:

            var disposable = new SampleDisposable(throwDispose);

            disposable.Using(strategy, _ => RunMainLogic(throwMain));

            Assert.IsTrue(disposable.WasDisposeCalled);

            // With return value:

            disposable = new SampleDisposable(throwDispose);

            var result = disposable.Using(strategy, _ => 
            {
                RunMainLogic(throwMain);
                return 42;
            });

            Assert.IsTrue(disposable.WasDisposeCalled);
            Assert.AreEqual(42, result);
        }

        private static void TestStrategy<TException>(DisposeExceptionStrategy strategy, bool throwMain, bool throwDispose, Action<TException> exceptionVerify = null)
            where TException : Exception
        {
            // No return value:

            var disposable = new SampleDisposable(throwDispose);

            var exception = ExceptionAssert.Throws<TException>(() =>
                disposable.Using(strategy, _ => RunMainLogic(throwMain)));

            Assert.IsTrue(disposable.WasDisposeCalled);
            exceptionVerify?.Invoke(exception);

            // With return value:

            disposable = new SampleDisposable(throwDispose);

            exception = ExceptionAssert.Throws<TException>(() =>
                disposable.Using(strategy, _ => 
                {
                    RunMainLogic(throwMain);
                    return 42;
                }));

            Assert.IsTrue(disposable.WasDisposeCalled);
            exceptionVerify?.Invoke(exception);
        }
        
#pragma warning disable 1998
        private static void RunMainLogic(bool throwMain = false)
        {
            if (throwMain)
                throw new MainLogicException();
        }
#pragma warning restore 1998

        [TestMethod]
        public async Task UsingAsync()
        {
            bool throwMain = false;
            bool throwDispose = false;
            foreach (var strategy in EnumUtility.GetValues<DisposeExceptionStrategy>())
                await TestStrategyNoExceptionAsync(strategy, throwMain, throwDispose);

            throwMain = true;
            throwDispose = false;
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose);
            await TestStrategyAsync<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
                Assert.IsInstanceOfType(e.InnerExceptions.Single(), typeof(MainLogicException)));

            throwMain = false;
            throwDispose = true;
            await TestStrategyAsync<DisposeException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            await TestStrategyNoExceptionAsync(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            await TestStrategyAsync<DisposeException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            await TestStrategyAsync<DisposeException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose);
            await TestStrategyAsync<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
                Assert.IsInstanceOfType(e.InnerExceptions.Single(), typeof(DisposeException)));

            throwMain = true;
            throwDispose = true;
            await TestStrategyAsync<DisposeException>(DisposeExceptionStrategy.Propagate, throwMain, throwDispose);
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.Swallow, throwMain, throwDispose);
            await TestStrategyAsync<MainLogicException>(DisposeExceptionStrategy.Subjugate, throwMain, throwDispose);
            await TestStrategyAsync<AggregateException>(DisposeExceptionStrategy.AggregateMultiple, throwMain, throwDispose, e =>
            {
                EnumerableAssert.HasCount(2, e.InnerExceptions);
                Assert.IsInstanceOfType(e.InnerExceptions[0], typeof(MainLogicException));
                Assert.IsInstanceOfType(e.InnerExceptions[1], typeof(DisposeException));
            });
            await TestStrategyAsync<AggregateException>(DisposeExceptionStrategy.AggregateAlways, throwMain, throwDispose, e =>
            {
                EnumerableAssert.HasCount(2, e.InnerExceptions);
                Assert.IsInstanceOfType(e.InnerExceptions[0], typeof(MainLogicException));
                Assert.IsInstanceOfType(e.InnerExceptions[1], typeof(DisposeException));
            });
        }

        private static async Task TestStrategyNoExceptionAsync(DisposeExceptionStrategy strategy, bool throwMain, bool throwDispose)
        {
            // No return value:

            var disposable = new SampleDisposable(throwDispose);

            await disposable.UsingAsync(strategy, async _ => await RunMainLogicAsync(throwMain));

            Assert.IsTrue(disposable.WasDisposeCalled);

            // With return value:

            disposable = new SampleDisposable(throwDispose);

            var result = await disposable.UsingAsync(strategy, async _ => 
            {
                await RunMainLogicAsync(throwMain);
                return 42;
            });

            Assert.IsTrue(disposable.WasDisposeCalled);
            Assert.AreEqual(42, result);
        }

        private static async Task TestStrategyAsync<TException>(DisposeExceptionStrategy strategy, bool throwMain, bool throwDispose, Action<TException> exceptionVerify = null)
            where TException : Exception
        {
            // No return value:

            var disposable = new SampleDisposable(throwDispose);

            var exception = await ExceptionAssert.ThrowsAsync<TException>(() =>
                disposable.UsingAsync(strategy, async _ => await RunMainLogicAsync(throwMain)));

            Assert.IsTrue(disposable.WasDisposeCalled);
            exceptionVerify?.Invoke(exception);

            // With return value:

            disposable = new SampleDisposable(throwDispose);

            exception = await ExceptionAssert.ThrowsAsync<TException>(() =>
                disposable.UsingAsync(strategy, async _ => 
                {
                    await RunMainLogicAsync(throwMain);
                    return 42;
                }));

            Assert.IsTrue(disposable.WasDisposeCalled);
            exceptionVerify?.Invoke(exception);
        }
        
#pragma warning disable 1998
        private static async Task RunMainLogicAsync(bool throwMain = false)
        {
            if (throwMain)
                throw new MainLogicException();
        }
#pragma warning restore 1998

    
    }
}