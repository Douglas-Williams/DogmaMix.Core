using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Provides factory methods for the creating instances of the <see cref="SequenceComparer{TElement}"/> class.
    /// </summary>
    public static class SequenceComparer
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SequenceComparer{TElement}"/> class,
        /// using the specified or default sequence comparison rules for sequences of different lengths, 
        /// and the specified or default sort-order comparer for the sequence elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of the sequences to compare.</typeparam>
        /// <param name="comparisonType">
        /// Indicates the sequence comparison rules to be used when comparing sequences of different lengths.
        /// If the argument is omitted, <see cref="SequenceComparison.Lexicographical"/> is used.
        /// </param>
        /// <param name="elementComparer">
        /// The sort-order comparison operation to apply to the elements of the sequences.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="TElement"/> is used.
        /// </param>
        /// <remarks>
        /// To get a <see cref="SequenceComparer{TElement}"/> that uses the default comparer
        /// for the sequence elements, access the 
        /// <see cref="SequenceComparer{TElement}.Lexicographical"/>,
        /// <see cref="SequenceComparer{TElement}.Shortlex"/>, or
        /// <see cref="SequenceComparer{TElement}.SameLength"/> static property rather than this factory method.
        /// </remarks>
        public static SequenceComparer<TElement> Create<TElement>(
            SequenceComparison comparisonType = SequenceComparison.Lexicographical,
            IComparer<TElement> elementComparer = null)
        {
            return new SequenceComparer<TElement>(comparisonType, elementComparer);
        }
    }
}
