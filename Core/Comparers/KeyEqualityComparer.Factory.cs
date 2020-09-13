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
        /// Creates a new instance of the <see cref="KeyEqualityComparer{TSource,TKey}"/> class,
        /// using the specified key extraction function and the specified or default equality comparer
        /// for extracted keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
        /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerEqualityComparer">
        /// The equality comparison operation to apply to the extracted keys.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="EqualityComparer{T}.Default"/> comparer for type <typeparamref name="TKey"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public static KeyEqualityComparer<TSource, TKey> Create<TSource, TKey>(
            Func<TSource, TKey> keySelector, 
            IEqualityComparer<TKey> innerEqualityComparer = null)
        {
            return new KeyEqualityComparer<TSource, TKey>(keySelector, innerEqualityComparer);
        }
    }    
}
