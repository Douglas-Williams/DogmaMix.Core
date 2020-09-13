using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Disposables
{
    /// <summary>
    /// Extends the <see cref="Disposable"/> base class by overriding the finalizer to release unmanaged resources
    /// if the <see cref="Disposable.Dispose()"/> method is not called by a consumer of the class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Classes should derive from <see cref="FinalizableDisposable"/> if they are responsible 
    /// for releasing unmanaged resources that do not have their own finalizer.
    /// Derived classes should encapsulate the logic for releasing unmanaged resources within their
    /// overridden implementation of the <see cref="Disposable.Dispose(bool)"/> method.
    /// Unmanaged resources should always be released by <see cref="Disposable.Dispose(bool)"/>,
    /// regardless of the value of the <c>disposing</c> parameter.
    /// </para>
    /// <blockquote>
    /// ✓ DO make a type finalizable if the type is responsible for releasing an unmanaged resource that does not have its own finalizer.
    /// When implementing the finalizer, simply call <c>Dispose</c> and place all resource cleanup logic inside the <c>Dispose(bool)</c> method.
    /// </blockquote>
    /// <para>
    /// However, note that MSDN recommends against the definition of finalizable types:
    /// </para>
    /// <blockquote>
    /// Finalizers are notoriously difficult to implement correctly, primarily because you cannot make certain
    /// (normally valid) assumptions about the state of the system during their execution.
    /// <blockquote>
    /// </blockquote>
    /// Writing code for an object's finalizer is a complex task that can cause problems if not done correctly. 
    /// Therefore, we recommend that you construct <see cref="SafeHandle"/> objects instead of implementing a finalizer.
    /// </blockquote>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose">Implementing a Dispose Method</see>, <i>MSDN Library</i></item>
    /// </list>
    /// </remarks>
    public abstract class FinalizableDisposable : Disposable
    {
        /// <summary>
        /// Destructs the current object, performing cleanup operations on unmanaged resources it holds.
        /// </summary>
        /// <remarks>
        /// This destructor overrides the <see cref="object.Finalize"/> method.
        /// </remarks>
        ~FinalizableDisposable()
        {
            Dispose(false);
        }
    }
}
