using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides factory methods for the creating instances of the <see cref="KeyEqualityComparer{TSource, TKey}"/> class.
    /// </summary>
    public static class KeyEqualityComparer
    {
        /// <summary>
        /// Creates a new instance of the <see cref="KeyEqualityComparer{TSource, TKey}"/> class,
        /// using the specified key extraction function and the <see cref="EqualityComparer{T}.Default"/> equality comparer 
        /// for type <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
        /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public static KeyEqualityComparer<TSource, TKey> Create<TSource, TKey>(Func<TSource, TKey> keySelector)
        {
            return new KeyEqualityComparer<TSource, TKey>(keySelector);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="KeyComparer{TSource,TKey}"/> class,
        /// using the specified key extraction function and the specified equality comparer for extracted keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
        /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerEqualityComparer">
        /// The equality comparison operation to apply to the extracted keys,
        /// or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer
        /// for type <typeparamref name="TKey"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public static KeyEqualityComparer<TSource, TKey> Create<TSource, TKey>(
            Func<TSource, TKey> keySelector, 
            IEqualityComparer<TKey> innerEqualityComparer)
        {
            return new KeyEqualityComparer<TSource, TKey>(keySelector, innerEqualityComparer);
        }
    }    
}
