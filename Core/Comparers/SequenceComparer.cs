using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Represents a generic sort-order comparison operation that compares sequences of elements.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the sequences to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// This sort-order comparer implements a <see href="https://en.wikipedia.org/wiki/Lexicographical_order">lexicographical order</see>.
    /// Namely, a sequence of elements { <i>a</i><sub>1</sub>, <i>a</i><sub>2</sub>, …, <i>a</i><sub>m</sub> }
    /// is less than another sequence  { <i>b</i><sub>1</sub>, <i>b</i><sub>2</sub>, …, <i>b</i><sub>n</sub> }
    /// if and only if, at the first index <i>i</i> where <i>a<sub>i</sub></i> and <i>b<sub>i</sub></i> differ, 
    /// <i>a<sub>i</sub></i> is less than <i>b<sub>i</sub></i>.
    /// If all elements are equal, then the two sequences are considered to be equal, provided that they are of the same length.
    /// When comparing sequences of different lengths, the aforementioned element-by-element check still applies.
    /// However, if all elements of the shorter sequence are equal to their counterparts in the longer sequence
    /// (meaning that the shorter sequence is a <i>prefix</i> of the longer sequence),
    /// then the shorter sequence is considered to be less than the longer sequence.
    /// Nominally, the empty sequence is considered to be less than any other sequence.
    /// </para>
    /// <para>
    /// Lexicographical ordering is closely related to ordinal comparisons on strings as sequences of characters.
    /// In fact, if a <see cref="SequenceComparer{TElement}"/> of type <see cref="char"/> was used to compare strings,
    /// it should give the same results as the <see cref="StringComparer.Ordinal"/> string comparer.
    /// </para>
    /// <para>
    /// This class serves a similar purpose to <see cref="StructuralComparisons.StructuralComparer"/>,
    /// which relies on implementations of the <see cref="IStructuralComparable"/> interface.
    /// The current class offers the following advantages:
    /// <list type="bullet">
    /// <item>
    /// In .NET Framework 4.6.1, <see cref="IStructuralComparable"/> is only implemented by arrays and tuples,
    /// whereas the current class supports all <see cref="IEnumerable{T}"/> sequences.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralComparer"/> and <see cref="IStructuralComparable"/> are non-generic, 
    /// meaning that boxing and unboxing would be required when comparing elements for value types.
    /// The current class provides a generic type parameter, <typeparamref name="TElement"/>, for the elements.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralEqualityComparer"/> does not accept a custom comparer for element-level comparisons,
    /// but just calls the <see cref="Comparer.Compare(object, object)"/> method on the <see cref="Comparer.Default"/> instance
    /// of the <see cref="Comparer"/> class.
    /// The current class can accept a custom <see cref="IComparer{TElement}"/> comparer through 
    /// its <see cref="SequenceComparer(IComparer{TElement})"/> constructor.
    /// </item>
    /// <item>
    /// The <see cref="IStructuralComparable.CompareTo"/> implementation for arrays throws an <see cref="ArgumentException"/>
    /// for arrays of different lengths. The current class can correctly handle such arrays.
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// This class is defined separately from <see cref="SequenceEqualityComparer{TElement}"/> for reasons
    /// discussed under the remarks of the <see cref="KeyComparer{TSource, TKey}"/> class.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="http://stackoverflow.com/a/20802693/1149773">How to compare arrays in a list</see> (answer), <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    [Serializable]
    public class SequenceComparer<TElement> : Comparer<IEnumerable<TElement>>
    {
        private readonly IComparer<TElement> _elementComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer{TElement}"/> class,
        /// using the <see cref="EqualityComparer{T}.Default"/> sort-order comparer for type <typeparamref name="TElement"/>.
        /// </summary>
        public SequenceComparer()
            : this(Comparer<TElement>.Default)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer{TElement}"/> class,
        /// using the specified sort-order comparer for the sequence elements.
        /// </summary>
        /// <param name="elementComparer">The sort-order comparison operation to apply to the elements of the sequences.</param>
        /// <exception cref="ArgumentNullException"><paramref name="elementComparer"/> is <see langword="null"/>.</exception>
        public SequenceComparer(IComparer<TElement> elementComparer)
        {
            ArgumentValidate.NotNull(elementComparer, nameof(elementComparer));

            _elementComparer = elementComparer;
        }

        /// <summary>
        /// Gets the default sequence sort-order comparer for the element type specified by the generic argument.
        /// </summary>
        public new static SequenceComparer<TElement> Default { get; } = new SequenceComparer<TElement>();

        /// <summary>
        /// Compares two sequences of elements and returns a value indicating whether one is less than, equal to, 
        /// or greater than the other.
        /// </summary>
        /// <param name="x">The first sequence of elements to compare.</param>
        /// <param name="y">The second sequence of elements to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, 
        /// as shown in the following table:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>
        /// <paramref name="x"/> is less than <paramref name="y"/>. -or- 
        /// <paramref name="x"/> is <see langword="null"/> and <paramref name="y"/> is not <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// <paramref name="x"/> is equal to <paramref name="y"/>. -or- 
        /// <paramref name="x"/> and <paramref name="y"/> are both <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// <paramref name="x"/> is greater than <paramref name="y"/>. -or- 
        /// <paramref name="x"/> is not <see langword="null"/> and <paramref name="y"/> is <see langword="null"/>.
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        public override int Compare(IEnumerable<TElement> x, IEnumerable<TElement> y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            // For an example of how to use enumerators, refer to the source code for the Enumerable.SequenceEqual method.
            // http://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,9bdd6ef7ba6a5615

            using (IEnumerator<TElement> xEnumerator = x.GetEnumerator())
            using (IEnumerator<TElement> yEnumerator = y.GetEnumerator())
            {
                while (xEnumerator.MoveNext())
                {
                    if (!yEnumerator.MoveNext())
                        return 1;   // y ended first, so x is greater

                    int elementComparison = _elementComparer.Compare(xEnumerator.Current, yEnumerator.Current);
                    if (elementComparison != 0)
                        return Math.Sign(elementComparison);
                }

                if (yEnumerator.MoveNext())
                    return -1;   // x ended first, so y is greater
            }

            return 0;
        }
    }
}
