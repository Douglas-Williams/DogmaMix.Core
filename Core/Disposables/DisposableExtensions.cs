using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Disposables;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IDisposable"/> interface.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Executes the specified action delegate using the disposable resource,
        /// then disposes of the said resource by calling its <see cref="IDisposable.Dispose()"/> method.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable resource to use.</typeparam>
        /// <param name="disposable">The disposable resource to use.</param>
        /// <param name="strategy">
        /// The strategy for propagating or swallowing exceptions thrown by the <see cref="IDisposable.Dispose"/> method.
        /// </param>
        /// <param name="action">The action delegate to execute using the disposable resource.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This extension method enhances the functionality of the <see langword="using"/> keyword by providing alternative strategies
        /// for propagating or swallowing exceptions thrown by the <see cref="IDisposable.Dispose"/> method,
        /// in conjunction with exceptions thrown by the main logic of the <paramref name="action"/> delegate.
        /// </para>
        /// <para>
        /// The standard behavior of the <see langword="using"/> keyword causes any exceptions from the main logic
        /// to be hidden and lost if an exception is also thrown from <see cref="IDisposable.Dispose"/>.
        /// According to Sections 8.9.5 and 8.10 of the C# Language Specification (version 5.0):
        /// </para>
        /// <blockquote>
        /// If the <see langword="finally"/> block throws another exception, processing of the current exception 
        /// [that was thrown from the <see langword="try"/> block or a <see langword="catch"/> block] is terminated.
        /// </blockquote>
        /// <blockquote>
        /// If an exception is thrown during execution of a <see langword="finally"/> block,
        /// and is not caught within the same <see langword="finally"/> block, 
        /// the exception is propagated to the next enclosing <see langword="try"/> statement. 
        /// If another exception was in the process of being propagated, that exception is lost. 
        /// </blockquote>
        /// <para>
        /// MSDN cautions against the throwing of exceptions from the <see cref="IDisposable.Dispose"/> method.
        /// </para>
        /// <blockquote>
        /// X AVOID throwing an exception from within <see cref="IDisposable.Dispose"/> except under critical situations 
        /// where the containing process has been corrupted (leaks, inconsistent shared state, etc.).
        /// Users expect that a call to <see cref="IDisposable.Dispose"/> will not raise an exception.
        /// </blockquote>
        /// <para>
        /// However, this guideline is broken repeatedly throughout the .NET Framework, including in the <see cref="FileStream"/> class,
        /// which can throw <see cref="IOException"/> whilst flushing its buffered data to the underlying device,
        /// and notoriously from the <see cref="ClientBase{TChannel}"/> base class for WCF clients, 
        /// which recommends ditching the <see langword="using"/> keyword to avoid this issue 
        /// in <see href="https://msdn.microsoft.com/en-us/library/aa355056(v=vs.110).aspx">Avoiding Problems with the Using Statement</see>.
        /// </para>
        /// <para>
        /// The problematic implications of such exception swallowing are discussed 
        /// in this <see href="http://stackoverflow.com/q/577607/1149773">Stack Overflow question</see>:
        /// </para>
        /// <blockquote>
        /// <list type="number">
        /// <item>A first exception is thrown</item>
        /// <item>A finally block is executed as a result of the first exception</item>
        /// <item>The finally block calls a Dispose() method</item>
        /// <item>The Dispose() method throws a second exception</item>
        /// </list>
        /// […] You lose information because .NET unceremoneously replaces the first exception with the second one.
        /// A catch block somewhere up the call stack will therefore never see the first exception.
        /// However, one is usually more interested in the first exception because that normally gives better clues 
        /// as to why things started to go wrong.
        /// </blockquote>
        /// <para>
        /// This extension method aims to provide a conclusive resolution to this issue by implementing the various plausible strategies,
        /// including the standard behavior, and leaving it up to the caller to decide which to use.
        /// </para>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/q/577607/1149773">Should you implement IDisposable.Dispose() so that it never throws?</see>, <i>Stack Overflow</i></item>
        /// <item><see href="http://stackoverflow.com/q/35602760/1149773">Delegate-parameterized method vs IDisposable implementation"</see>, <i>Stack Overflow</i></item>
        /// <item><see href="http://stackoverflow.com/q/1654487/1149773">Swallowing exception thrown in catch/finally block"</see>, <i>Stack Overflow</i></item>
        /// <item><see href="https://msdn.microsoft.com/en-us/library/b1yfkh5e(v=vs.110).aspx">Dispose Pattern</see>, <i>MSDN Library</i></item>
        /// <item><see href="https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx">Implementing a Dispose Method</see>, <i>MSDN Library</i></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// new FileStream(Path.GetTempFileName(), FileMode.Create)
        ///     .Using(strategy: DisposeExceptionStrategy.Subjugate, action: fileStream =>
        ///     {
        ///         // Access fileStream here
        ///         fileStream.WriteByte(42);
        ///         throw new InvalidOperationException();
        ///     });   
        ///     // Any Dispose() exceptions will be swallowed due to the above InvalidOperationException
        /// </code>
        /// </example>
        public static void Using<TDisposable>(this TDisposable disposable, DisposeExceptionStrategy strategy, Action<TDisposable> action)
            where TDisposable : IDisposable
        {
            ArgumentValidate.NotNull(disposable, nameof(disposable));
            ArgumentValidate.NotNull(action, nameof(action));
            ArgumentValidate.EnumDefined(strategy, nameof(strategy));

            var func = action.Return(true);
            disposable.Using(strategy, func);
        }

        /// <summary>
        /// Executes the specified function delegate using the disposable resource,
        /// then disposes of the said resource by calling its <see cref="IDisposable.Dispose()"/> method.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable resource to use.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate.</typeparam>
        /// <param name="disposable">The disposable resource to use.</param>
        /// <param name="strategy">
        /// The strategy for propagating or swallowing exceptions thrown by the <see cref="IDisposable.Dispose"/> method.
        /// </param>
        /// <param name="func">The function delegate to execute using the disposable resource.</param>
        /// <returns>The return value of the function delegate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Using{TDisposable}(TDisposable, DisposeExceptionStrategy, Action{TDisposable})"/> overload.
        /// </remarks>
        public static TResult Using<TDisposable, TResult>(this TDisposable disposable, DisposeExceptionStrategy strategy, Func<TDisposable, TResult> func)
            where TDisposable : IDisposable
        {
            ArgumentValidate.NotNull(disposable, nameof(disposable));
            ArgumentValidate.NotNull(func, nameof(func));
            ArgumentValidate.EnumDefined(strategy, nameof(strategy));

            var asyncFunc = func.WrapAsync();
            var completedTask = disposable.UsingAsync(strategy, asyncFunc);
            return completedTask.GetResult();   // task is always completed; returns immediately
        }

        /// <summary>
        /// Executes the specified asynchronous action delegate using the disposable resource,
        /// then disposes of the said resource by calling its <see cref="IDisposable.Dispose()"/> method.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable resource to use.</typeparam>
        /// <param name="disposable">The disposable resource to use.</param>
        /// <param name="strategy">
        /// The strategy for propagating or swallowing exceptions thrown by the <see cref="IDisposable.Dispose"/> method.
        /// </param>
        /// <param name="asyncAction">The asynchronous action delegate to execute using the disposable resource.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> or <paramref name="asyncAction"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Using{TDisposable}(TDisposable, DisposeExceptionStrategy, Action{TDisposable})"/> overload.
        /// </remarks>
        public static Task UsingAsync<TDisposable>(this TDisposable disposable, DisposeExceptionStrategy strategy, Func<TDisposable, Task> asyncAction)
            where TDisposable : IDisposable
        {
            ArgumentValidate.NotNull(disposable, nameof(disposable));
            ArgumentValidate.NotNull(asyncAction, nameof(asyncAction));
            ArgumentValidate.EnumDefined(strategy, nameof(strategy));

            var asyncFunc = asyncAction.ReturnAsync(true);
            return disposable.UsingAsync(strategy, asyncFunc);
        }

        /// <summary>
        /// Executes the specified asynchronous function delegate using the disposable resource,
        /// then disposes of the said resource by calling its <see cref="IDisposable.Dispose()"/> method.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable resource to use.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the asynchronous function delegate.</typeparam>
        /// <param name="disposable">The disposable resource to use.</param>
        /// <param name="strategy">
        /// The strategy for propagating or swallowing exceptions thrown by the <see cref="IDisposable.Dispose"/> method.
        /// </param>
        /// <param name="asyncFunc">The asynchronous function delegate to execute using the disposable resource.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the return value of the asynchronous function delegate.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> or <paramref name="asyncFunc"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// Refer to the remarks on the <see cref="Using{TDisposable}(TDisposable, DisposeExceptionStrategy, Action{TDisposable})"/> overload.
        /// </remarks>
        public static Task<TResult> UsingAsync<TDisposable, TResult>(this TDisposable disposable, DisposeExceptionStrategy strategy, Func<TDisposable, Task<TResult>> asyncFunc)
            where TDisposable : IDisposable
        {
            ArgumentValidate.NotNull(disposable, nameof(disposable));
            ArgumentValidate.NotNull(asyncFunc, nameof(asyncFunc));
            ArgumentValidate.EnumDefined(strategy, nameof(strategy));

            return disposable.UsingAsyncInner(strategy, asyncFunc);
        }

        private static async Task<TResult> UsingAsyncInner<TDisposable, TResult>(this TDisposable disposable, DisposeExceptionStrategy strategy, Func<TDisposable, Task<TResult>> asyncFunc)
            where TDisposable : IDisposable
        {
            Exception mainException = null;

            try
            {
                return await asyncFunc(disposable).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                mainException = exception;
                throw;
            }
            finally
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception disposeException)
                {
                    switch (strategy)
                    {
                        case DisposeExceptionStrategy.Propagate:
                            throw;

                        case DisposeExceptionStrategy.Swallow:
                            break;   // swallow exception

                        case DisposeExceptionStrategy.Subjugate:
                            if (mainException == null)
                                throw;
                            break;    // otherwise swallow exception
                            
                        case DisposeExceptionStrategy.AggregateMultiple:
                            if (mainException != null)
                                throw new AggregateException(mainException, disposeException);
                            throw;

                        case DisposeExceptionStrategy.AggregateAlways:
                            if (mainException != null)
                                throw new AggregateException(mainException, disposeException);
                            throw new AggregateException(disposeException);
                    }
                }

                if (mainException != null && strategy == DisposeExceptionStrategy.AggregateAlways)
                    throw new AggregateException(mainException);
            }
        }
    }
}
