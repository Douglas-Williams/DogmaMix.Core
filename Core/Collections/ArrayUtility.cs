using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Collections
{
    /// <summary>
    /// Provides utility methods for arrays.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    public static class ArrayUtility<T>
    {
        /// <summary>
        /// Gets an empty array of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This readonly property caches an empty array of type <typeparamref name="T"/>.
        /// Since an empty array is immutable, the same instance can be returned to all callers,
        /// avoiding unnecessary memory allocation (and the performance penalty that this carries).
        /// </para>
        /// <para>
        /// This method serves a similar purpose to the <see cref="Enumerable.Empty{TResult}"/> method
        /// in the .NET Framework Class Library.
        /// </para>
        /// </remarks>
        public static T[] Empty { get; } = new T[0];
    }
}
