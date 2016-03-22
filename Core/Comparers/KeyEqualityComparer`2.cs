using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Represents a generic equality comparison operation that compares keys extracted from source items.
    /// </summary>
    /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
    /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
    /// <remarks>
    /// <para>
    /// This class is defined separately from <see cref="KeyComparer{TSource, TKey}"/> for reasons
    /// discussed under the remarks of the latter class.
    /// </para>
    /// </remarks>
    public class KeyEqualityComparer<TSource, TKey> : EqualityComparer<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _innerEqualityComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{TSource, TKey}"/> class,
        /// using the specified key extraction function and the <see cref="EqualityComparer{T}.Default"/> equality comparer 
        /// for type <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        protected internal KeyEqualityComparer(Func<TSource, TKey> keySelector)
            : this(keySelector, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyComparer{TSource,TKey}"/> class,
        /// using the specified key extraction function and the specified equality comparer for extracted keys.
        /// </summary>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerEqualityComparer">
        /// The equality comparison operation to apply to the extracted keys,
        /// or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer
        /// for type <typeparamref name="TKey"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        protected internal KeyEqualityComparer(Func<TSource, TKey> keySelector, IEqualityComparer<TKey> innerEqualityComparer)
        {
            ArgumentValidate.NotNull(keySelector, nameof(keySelector));

            _keySelector = keySelector;
            _innerEqualityComparer = innerEqualityComparer ?? EqualityComparer<TKey>.Default;
        }

        /// <summary>
        /// Determines whether the keys extracted from the specified source items are equal.
        /// </summary>
        /// <param name="x">The first source item to compare.</param>
        /// <param name="y">The second source item to compare.</param>
        /// <returns><see langword="true"/> if the keys are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(TSource x, TSource y)
        {
            if (object.ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;

            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);
            return _innerEqualityComparer.Equals(xKey, yKey);
        }
        
        /// <summary>
        /// Returns a hash code for the key extracted from the specified source item.
        /// </summary>
        /// <param name="item">The source item for which to get a hash code.</param>
        /// <returns>A hash code for the extracted key.</returns>
        public override int GetHashCode(TSource item)
        {
            if (item == null)
                return 0;

            TKey itemKey = _keySelector(item);
            return _innerEqualityComparer.GetHashCode(itemKey);
        }        
    }
}
