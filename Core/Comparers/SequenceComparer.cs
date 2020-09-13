using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DogmaMix.Core.Collections;
using DogmaMix.Core.Extensions;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Represents a generic sort-order comparison operation that compares sequences of elements,
    /// using the lexicographical order or the shortlex order. 
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the sequences to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// This sort-order comparer implements a 
    /// <see href="https://en.wikipedia.org/wiki/Lexicographical_order">lexicographical order</see> by default.
    /// Namely, a sequence of elements { <i>a</i><sub>1</sub>, <i>a</i><sub>2</sub>, …, <i>a<sub>m</sub></i> }
    /// is less than another sequence  { <i>b</i><sub>1</sub>, <i>b</i><sub>2</sub>, …, <i>b<sub>n</sub></i> }
    /// if and only if, at the first index <i>i</i> where <i>a<sub>i</sub></i> and <i>b<sub>i</sub></i> differ, 
    /// <i>a<sub>i</sub></i> is less than <i>b<sub>i</sub></i>.
    /// If all elements are equal, then the two sequences are considered to be equal,
    /// provided that they are of the same length.
    /// When comparing sequences of different lengths, the aforementioned element-by-element check still applies.
    /// However, if all elements of the shorter sequence are equal to their counterparts in the longer sequence
    /// (meaning that the shorter sequence is a <i>prefix</i> of the longer sequence),
    /// then the shorter sequence is considered to be less than the longer sequence.
    /// Nominally, the empty sequence is considered to be less than any other sequence.
    /// </para>
    /// <para>
    /// Lexicographical ordering is closely related to ordinal comparisons on strings as sequences of characters.
    /// In fact, if a <see cref="SequenceComparer{TElement}"/> of type <see cref="char"/> were used to compare strings,
    /// it would give the same results as the <see cref="StringComparer.Ordinal"/> string comparer
    /// (except for strings containing a mix of surrogate pairs and <abbr title="Basic Multilingual Plane">BMP</abbr>
    /// characters with higher code points; refer to the explanation on
    /// <see href="https://dogmamix.com/cms/blog/SequenceComparer#CodePointOrder">code point order</see>).
    /// </para>
    /// <para>
    /// Apart from the lexicographical order, 
    /// this class can also provide a <see href="https://en.wikipedia.org/wiki/Shortlex_order">shortlex order</see>,
    /// as well as throw <see cref="ArgumentException"/> for sequences of different lengths.
    /// Sequence comparers for these three variants that use the default element comparer for
    /// type <typeparamref name="TElement"/> may be retrieved through the <see cref="Lexicographical"/>, 
    /// <see cref="Shortlex"/>, and <see cref="SameLength"/> static properties.
    /// Sequence comparers that use a custom element comparer may be created by calling the 
    /// <see cref="SequenceComparer.Create{TElement}(SequenceComparison, IComparer{TElement})"/> factory method
    /// with the appropriate <see cref="SequenceComparison"/> enumeration value.
    /// </para>
    /// <para>
    /// This class serves a similar purpose to <see cref="StructuralComparisons.StructuralComparer"/>,
    /// which relies on implementations of the <see cref="IStructuralComparable"/> interface.
    /// The current class offers the following advantages:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// In .NET Framework 4.6.1, <see cref="IStructuralComparable"/> is only implemented by arrays and tuples,
    /// whereas the current class supports all <see cref="IEnumerable{T}"/> sequences.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralComparer"/> and <see cref="IStructuralComparable"/> are non-generic, 
    /// meaning that boxing and unboxing would be required when comparing elements of value types.
    /// The current class provides a generic type parameter, <typeparamref name="TElement"/>, for the elements.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralComparer"/> does not accept a custom comparer for element-level comparisons,
    /// but just calls the <see cref="Comparer.Compare(object, object)"/> method on the <see cref="Comparer.Default"/> instance
    /// of the <see cref="Comparer"/> class.
    /// The current class can accept a custom <see cref="IComparer{TElement}"/> comparer through 
    /// its <see cref="SequenceComparer.Create{TElement}(SequenceComparison, IComparer{TElement})"/> factory method.
    /// </item>
    /// <item>
    /// The <see cref="IStructuralComparable.CompareTo"/> implementation for arrays throws an 
    /// an <see cref="ArgumentException"/> for arrays of different lengths. 
    /// The current class can support <see cref="SequenceComparison.Lexicographical"/>
    /// and <see cref="SequenceComparison.Shortlex"/> orders, which permit sequences of different lengths.
    /// </item>
    /// </list>
    /// <para>
    /// The implementation of the <see cref="IComparer{T}.Compare(T, T)"/> method 
    /// in this class includes an optimization for in-memory collections.
    /// If the sequence comparison type is <see cref="SequenceComparison.Shortlex"/> 
    /// or <see cref="SequenceComparison.SameLength"/>, then this class would first attempt to 
    /// perform a length comparison of the two sequences without enumerating them.
    /// The lengths are determined through the <see cref="EnumerableExtensions.TryFastCount"/> extension method.
    /// </para>
    /// <para>
    /// This class is defined separately from <see cref="SequenceEqualityComparer{TElement}"/> for reasons
    /// discussed under the remarks of the <see cref="KeyComparer{TSource, TKey}"/> class.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://dogmamix.com/cms/blog/SequenceComparer">SequenceComparer: Sorting sequences of sequences</see>, <i>DogmaMix.com</i></item>
    /// <item><see href="https://stackoverflow.com/q/2811725/1149773">Is there a built-in way to compare IEnumerable&lt;T&gt; (by their elements)?</see>, <i>Stack Overflow</i></item>
    /// <item><see href="https://stackoverflow.com/q/20552503/1149773">Order parent collection by minimum values in child collection in Linq</see>, <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// For demonstrations and examples, see our referenced
    /// <see href="https://dogmamix.com/cms/blog/SequenceComparer">SequenceComparer</see> article,
    /// an excerpt of which is provided below:
    /// <code>
    /// var sequences = new string[][]
    /// {
    ///     new string[] { "Paris", "Añasco", "Athens", "New York" },
    ///     new string[] { "Madrid", "Paris", "Añasco" },
    ///     new string[] { },
    ///     new string[] { "Añasco", "Madrid" },
    ///     new string[] { "Paris", "Añasco", "Athens" },
    ///     // ...
    /// };
    /// 
    /// // Lexicographical order, with element-level string comparisons being linguistic under the current culture:
    /// var a = sequences.OrderBy(s =&gt; s, SequenceComparer&lt;string&gt;.Lexicographical);
    /// 
    /// // Lexicographical order, with element-level string comparisons being ordinal:
    /// var b = sequences.OrderBy(s &gt; s, SequenceComparer.Create(SequenceComparison.Lexicographical, StringComparer.Ordinal));
    /// 
    /// // Shortlex order, with element-level string comparisons being linguistic under the current culture:
    /// var c = sequences.OrderBy(s =&gt; s, SequenceComparer&lt;string&gt;.Shortlex);
    /// 
    /// // Shortlex order, with element-level string comparisons being ordinal:
    /// var d = sequences.OrderBy(s =&gt; s, SequenceComparer.Create(SequenceComparison.Shortlex, StringComparer.Ordinal));
    /// </code>
    /// </example>
    [Serializable]
    public class SequenceComparer<TElement> : ComparerBase<IEnumerable<TElement>>
    {
        private readonly SequenceComparison _comparisonType;
        private readonly IComparer<TElement> _elementComparer;

        /// <summary>
        /// Whether the comparison of the lengths of the sequences dominates 
        /// over the element-by-element comparisons.
        /// </summary>
        /// <remarks>
        /// <see langword="true"/> for <see cref="SequenceComparison.Shortlex"/> 
        /// and <see cref="SequenceComparison.SameLength"/>;
        /// <see langword="false"/> for <see cref="SequenceComparison.Lexicographical"/>.
        /// </remarks>
        private readonly bool _lengthDominated;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceComparer{TElement}"/> class,
        /// using the specified or default sequence comparison rules for sequences of different lengths, 
        /// and the specified or default sort-order comparer for the sequence elements.
        /// </summary>
        /// <param name="comparisonType">
        /// Indicates the sequence comparison rules to be used when comparing sequences of different lengths.
        /// If the argument is omitted, <see cref="SequenceComparison.Lexicographical"/> is used.
        /// </param>
        /// <param name="elementComparer">
        /// The sort-order comparison operation to apply to the elements of the sequences.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="Comparer{T}.Default"/> comparer for type <typeparamref name="TElement"/> is used.
        /// </param>
        protected internal SequenceComparer(
            SequenceComparison comparisonType = SequenceComparison.Lexicographical,
            IComparer<TElement> elementComparer = null)
        {
            _comparisonType = comparisonType;
            _elementComparer = elementComparer ?? Comparer<TElement>.Default;

            _lengthDominated =
                comparisonType == SequenceComparison.Shortlex ||
                comparisonType == SequenceComparison.SameLength;
        }

        /// <summary>
        /// Gets the default lexicographical order comparer for the element type 
        /// specified by the generic argument.
        /// </summary>
        public static SequenceComparer<TElement> Lexicographical { get; } =
            new SequenceComparer<TElement>(SequenceComparison.Lexicographical);

        /// <summary>
        /// Gets the default shortlex order comparer for the element type 
        /// specified by the generic argument.
        /// </summary>
        public static SequenceComparer<TElement> Shortlex { get; } =
            new SequenceComparer<TElement>(SequenceComparison.Shortlex);

        /// <summary>
        /// Gets the default same-length order comparer for the element type 
        /// specified by the generic argument.
        /// </summary>
        public static SequenceComparer<TElement> SameLength { get; } =
            new SequenceComparer<TElement>(SequenceComparison.SameLength);

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
        /// <term><paramref name="x"/> is less than <paramref name="y"/>.</term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term><paramref name="x"/> is equal to <paramref name="y"/>.</term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term><paramref name="x"/> is greater than <paramref name="y"/>.</term>
        /// </item>
        /// </list>
        /// </returns>
        protected override int CompareNonNull(IEnumerable<TElement> x, IEnumerable<TElement> y)
        {
            // Optimization for in-memory collections:
            // If the current sequence comparison is Shortlex or SameLength, 
            // then attempt to perform a length comparison without enumerating the sequences.
            if (_lengthDominated)
            {
                if (x.TryFastCount(out int xCount) &&
                    y.TryFastCount(out int yCount))
                {
                    int countComparison = xCount.CompareTo(yCount);
                    if (countComparison != 0)
                    {
                        // Shortlex returns immediately for sequences of different lengths,
                        // whilst SameLength throws immediately for such sequences.
                        if (_comparisonType == SequenceComparison.Shortlex)
                            return countComparison;
                        if (_comparisonType == SequenceComparison.SameLength)
                            throw new ArgumentException("The sequences to be compared do not have the same number of elements.");
                    }
                }
            }

            // For an example of how to use enumerators, refer to the source code for the Enumerable.SequenceEqual method.
            // https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,9bdd6ef7ba6a5615

            // Create an enumerator pair to iterate over both sequences in sync.
            using (var enumeratorPair = new EnumeratorPair<TElement>(x, y))
            {
                // Advance both enumerators to their next element, 
                // until at least one passes the end of its sequence.
                while (enumeratorPair.MoveNext())
                {
                    // Compare the current pair of elements across the two sequences,
                    // seeking element inequality.
                    int elementComparison = _elementComparer.Compare(enumeratorPair.Current1, enumeratorPair.Current2);
                    if (elementComparison != 0)
                    {
                        // If the current sequence comparison is Shortlex or SameLength, 
                        // then length inequality needs to be given precedence over element inequality.
                        if (_lengthDominated)
                        {
                            // Advance both enumerators, ignoring the elements,
                            // until at least one passes the end of its sequence. 
                            while (enumeratorPair.MoveNext())
                                ;

                            // If one sequence was shorter than the other, then use the length comparison result.
                            // Such logic is implemented beyond the outer while loop.
                            if (enumeratorPair.LengthComparison != 0)
                                break;
                        }

                        // If the current sequence comparison is Lexicographical, 
                        // or the sequences have been found to share the same length,
                        // then return the non-zero element comparison result.
                        return elementComparison;
                    }
                }

                // This point is only reached if at least one sequence ended without finding any unequal pair of elements;
                // or an unequal pair of elements was found, but the unequal length of the sequences dominates the result.

                // If the current sequence comparison is SameLength,
                // then throw exception for sequences of different lengths.
                if (_comparisonType == SequenceComparison.SameLength &&
                    enumeratorPair.LengthComparison != 0)
                    throw new ArgumentException("The sequences to be compared do not have the same number of elements.");

                // Return the length comparison result.
                return enumeratorPair.LengthComparison;
            }
        }
    }
}