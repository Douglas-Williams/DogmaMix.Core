using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides factory methods for the creating instances of the <see cref="SequenceEqualityComparer{TElement}"/> class.
    /// </summary>
    public static class SequenceEqualityComparer
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SequenceEqualityComparer{TElement}"/> class,
        /// using the specified equality comparer for the sequence elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of the sequences to compare.</typeparam>
        /// <param name="elementEqualityComparer">
        /// The equality comparison operation to apply to the elements of the sequences,
        /// or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer
        /// for type <typeparamref name="TElement"/>.
        /// </param>
        /// <remarks>
        /// To get a <see cref="SequenceEqualityComparer{TElement}"/> that uses the default comparer for the sequence elements,
        /// access the <see cref="SequenceEqualityComparer{TElement}.Default"/> static property rather than this factory method.
        /// </remarks>
        public static SequenceEqualityComparer<TElement> Create<TElement>(IEqualityComparer<TElement> elementEqualityComparer)
        {
            return new SequenceEqualityComparer<TElement>(elementEqualityComparer);
        }
    }
}
