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
    /// Instances of this class may be created through the 
    /// <see cref="KeyEqualityComparer.Create{TSource, TKey}(Func{TSource, TKey}, IEqualityComparer{TKey})"/> factory method.
    /// </para>
    /// <para>
    /// This class is defined separately from <see cref="KeyComparer{TSource, TKey}"/> for reasons
    /// discussed under the remarks of the latter class.
    /// </para>
    /// </remarks>
    public class KeyEqualityComparer<TSource, TKey> : EqualityComparerBase<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _innerEqualityComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{TSource,TKey}"/> class,
        /// using the specified key extraction function and the specified or default 
        /// equality comparer for extracted keys.
        /// </summary>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerEqualityComparer">
        /// The equality comparison operation to apply to the extracted keys.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="EqualityComparer{T}.Default"/> comparer for type <typeparamref name="TKey"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        protected internal KeyEqualityComparer(
            Func<TSource, TKey> keySelector, 
            IEqualityComparer<TKey> innerEqualityComparer = null)
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
        protected override bool EqualsNonNull(TSource x, TSource y)
        {
            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);
            return _innerEqualityComparer.Equals(xKey, yKey);
        }

        /// <summary>
        /// Returns a hash code for the key extracted from the specified source item.
        /// </summary>
        /// <param name="item">The source item for which to get a hash code.</param>
        /// <returns>A hash code for the extracted key.</returns>
        protected override int GetHashCodeNonNull(TSource item)
        {
            TKey itemKey = _keySelector(item);
            return _innerEqualityComparer.GetHashCode(itemKey);
        }        
    }
}
