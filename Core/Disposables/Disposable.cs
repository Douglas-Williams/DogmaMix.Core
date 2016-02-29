using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Disposables
{
    /// <summary>
    /// Provides a reusable base implementation of the 
    /// <see href="https://msdn.microsoft.com/en-us/library/b1yfkh5e(v=vs.110).aspx#basic_pattern">Basic Dispose Pattern</see>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Classes that use managed and/or unmanaged resources that need to be released can derive from this base class 
    /// to inherit its implementation of the <see cref="IDisposable"/> interface, as well as the plumbing required
    /// for the Basic Dispose Pattern.
    /// Derived classes only need to override the <see cref="Dispose(bool)"/> method, relying on the base implementation
    /// to provide the public <see cref="Dispose()"/> method and to keep track of whether the current object 
    /// has already been disposed.
    /// </para>
    /// <para>
    /// If the <see cref="Dispose()"/> method is called more than once, this base class will only forward the call 
    /// to <see cref="Dispose(bool)"/> if the latter has not already been successfully executed.
    /// This follows the MSDN guidelines:
    /// </para>
    /// <blockquote>
    /// To help ensure that resources are always cleaned up appropriately, a <see cref="Dispose()"/> method 
    /// should be callable multiple times without throwing an exception.
    /// </blockquote>
    /// <para>
    /// The examples given in the MSDN documentation only set the <c>disposed</c> field to <see langword="true"/>
    /// <i>after</i> the managed and unmanaged resources have been successfully released. 
    /// In line with these semantics, this base class may forward multiple calls of <see cref="Dispose()"/> 
    /// to <see cref="Dispose(bool)"/> if all its former executions threw an unhandled exception.
    /// </para>
    /// <para>
    /// Classes should derive from this type only if they do not use any unmanaged resources directly, or if all their 
    /// unmanaged resources are wrapped in a safe handle (that is, in a class derived from <see cref="SafeHandle"/>).
    /// The Basic Dispose Pattern excludes the definition of a finalizer to release unmanaged resources.
    /// If a class uses unmanaged resources directly, it should derive from <see cref="FinalizableDisposable"/> instead.
    /// Note, however, that MSDN recommends against the implementation of such finalizer logic.
    /// </para>
    /// <blockquote>
    /// X AVOID making types finalizable.
    /// Carefully consider any case in which you think a finalizer is needed.
    /// There is a real cost associated with instances with finalizers, from both a performance and code complexity standpoint.
    /// Prefer using resource wrappers such as <see cref="SafeHandle"/> to encapsulate unmanaged resources where possible,
    /// in which case a finalizer becomes unnecessary because the wrapper is responsible for its own resource cleanup.
    /// </blockquote>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://msdn.microsoft.com/en-us/library/b1yfkh5e(v=vs.110).aspx">Dispose Pattern</see>, <i>MSDN Library</i></item>
    /// <item><see href="https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx">Implementing a Dispose Method</see>, <i>MSDN Library</i></item>
    /// <item><see href="https://coding.abel.nu/2012/01/disposable/">Disposable Base Class</see> by Anders Abel</item>
    /// </list>
    /// </remarks>
    public abstract class Disposable : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets whether the <see cref="Dispose()"/> method has been called on the current instance.
        /// Returns <see langword="true"/> even if <see cref="Dispose(bool)"/> has not yet completed executing, 
        /// or has thrown an unhandled exception.
        /// </summary>
        /// <remarks>
        /// Once this property becomes <see langword="true"/>, it will always remain <see langword="true"/>.
        /// </remarks>
        public bool HasDisposeStarted { get; private set; }

        /// <summary>
        /// Gets whether the <see cref="Dispose()"/> method has completed execution successfully 
        /// during its last invocation on the current instance.
        /// </summary>
        /// <remarks>
        /// Once this property becomes <see langword="true"/>, it will always remain <see langword="true"/>.
        /// Any further calls to <see cref="Dispose()"/> will be ignored and have no effect.
        /// </remarks>
        public bool HasDisposeCompleted { get; private set; }

        /// <summary>
        /// Gets whether the <see cref="Dispose()"/> method has thrown an unhandled exception
        /// during its last invocation on the current instance.
        /// </summary>
        /// <remarks>
        /// If this property becomes <see langword="true"/>, subsequent calls to <see cref="Dispose()"/> will still be forwarded 
        /// to <see cref="Dispose(bool)"/>. This property will revert to <see langword="false"/> as soon as such a call completes 
        /// execution successfully (with <see cref="HasDisposeCompleted"/> being set to <see langword="true"/> in its stead).
        /// </remarks>
        public bool HasDisposeFailed { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Releases all managed and unmanaged resources used by the current object.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            if (HasDisposeCompleted)
                return;

            HasDisposeStarted = true;

            try
            {
                // Dispose of managed and unmanaged resources.
                Dispose(true);

                // Suppress finalization.
                // This base class does not have a finalizer, but the call is left in place in case a derived class introduces one.
                // "If the type has no finalizer, the call to GC.SuppressFinalize has no effect."
                GC.SuppressFinalize(this);

                HasDisposeCompleted = true;
                HasDisposeFailed = false;
            }
            catch
            {
                HasDisposeFailed = true;
                throw;
            }
        }

        /// <summary>
        /// Should be overridden by derived classes to release the unmanaged resources used by the current object
        /// and optionally release the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; 
        /// <see langword="false"/> to release only unmanaged resources.
        /// From an execution perspective:
        /// <see langword="true"/> if the method call comes from the <see cref="Dispose()"/> method;
        /// <see langword="false"/> if the method call comes from a finalizer.
        /// </param>
        /// <remarks>
        /// The current base class does not override the finalizer. 
        /// Unless the finalizer is overridden by a derived class, the value of the <paramref name="disposing"/>
        /// parameter should always be <see langword="true"/>.
        /// </remarks>
        protected abstract void Dispose(bool disposing);

        #endregion
    }
}
