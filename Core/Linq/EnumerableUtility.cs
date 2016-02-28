using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Linq
{
    /// <summary>
    /// Provides utility methods for the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableUtility
    {
        /// <summary>
        /// Creates a single-element sequence consisting of the specified value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="element">The element from which to create the sequence.</param>
        /// <returns>A sequence that contains <paramref name="element"/>.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>See Also</listheader>
        /// <item><see href="http://stackoverflow.com/q/1577822/1149773">Passing a single item as IEnumerable&lt;T&gt;</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static IEnumerable<TSource> Yield<TSource>(TSource element)
        {
            yield return element;
        }

        /// <summary>
        /// Creates a sequence consisting of the specified elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="elements">The elements from which to create the sequence.</param>
        /// <returns>A sequence that contains the items in <paramref name="elements"/>.</returns>
        /// <remarks>
        /// <see href="http://stackoverflow.com/a/1577868/1149773">Jon Skeet recommends</see> against returning a list or an array,
        /// since an unscrupulous consumer could cast it back and change its contents, breaking the expected immutable behaviour
        /// for other consumers of the same sequence.
        /// </remarks>
        public static IEnumerable<TSource> Yield<TSource>(params TSource[] elements)
        {
            foreach (var element in elements)
                yield return element;
        }
    }
}
