using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides factory methods for the creating instances of the <see cref="KeyComparer{TSource, TKey}"/> class.
    /// </summary>
    public static class KeyComparer
    {
        /// <summary>
        /// Creates a new instance of the <see cref="KeyComparer{TSource, TKey}"/> class,
        /// using the specified key extraction function and the specified or default 
        /// sort-order comparer for extracted keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
        /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerComparer">
        /// The sort-order comparison operation to apply to the extracted keys.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="TKey"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public static KeyComparer<TSource, TKey> Create<TSource, TKey>(
            Func<TSource, TKey> keySelector, 
            IComparer<TKey> innerComparer = null)
        {
            return new KeyComparer<TSource, TKey>(keySelector, innerComparer);
        }
    }
}
