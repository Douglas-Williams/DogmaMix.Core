using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DogmaMix.Core.Extensions;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Represents a generic equality comparison operation that compares sequences of elements.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the sequences to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// This equality comparer uses the same semantics as the 
    /// <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource})"/> 
    /// extension method, which it internally calls for performing the equality check.
    /// Two sequences are considered equal if they are of equal length and their corresponding elements
    /// are equal according to the default or specified equality comparer for their type.
    /// Namely, a sequence of elements { <i>a</i><sub>1</sub>, <i>a</i><sub>2</sub>, …, <i>a<sub>m</sub></i> }
    /// is equal to another sequence   { <i>b</i><sub>1</sub>, <i>b</i><sub>2</sub>, …, <i>b<sub>n</sub></i> }
    /// if and only if <i>m</i> is equal to <i>n</i>, 
    /// and <i>a<sub>i</sub></i> is equal to <i>b<sub>i</sub></i> for all values of <i>i</i> from 1 to <i>m</i>.
    /// For example, when comparing arrays of integers, it must be the case that
    /// <c>x[0] == y[0]</c>, <c>x[1] == y[1]</c>, …, <c>x[x.Length - 1] == y[y.Length - 1]</c>.
    /// </para>
    /// <para>
    /// Sequence equality comparers that use the default element equality comparer for type <typeparamref name="TElement"/> 
    /// may be retrieved through the <see cref="Default"/> static property.
    /// Sequence equality comparers that use a custom element equality comparer may be created by calling the 
    /// <see cref="SequenceEqualityComparer.Create{TElement}(IEqualityComparer{TElement})"/> factory method.
    /// </para>
    /// <para>
    /// This class serves a similar purpose to <see cref="StructuralComparisons.StructuralEqualityComparer"/>,
    /// which relies on implementations of the <see cref="IStructuralEquatable"/> interface.
    /// The current class offers the following advantages:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// In .NET Framework 4.6.1, <see cref="IStructuralEquatable"/> is only implemented by arrays and tuples,
    /// whereas the current class supports all <see cref="IEnumerable{T}"/> sequences.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralEqualityComparer"/> and <see cref="IStructuralEquatable"/> are non-generic, 
    /// meaning that boxing and unboxing would be required when comparing elements for value types.
    /// The current class provides a generic type parameter, <typeparamref name="TElement"/>, for the elements.
    /// </item>
    /// <item>
    /// <see cref="StructuralComparisons.StructuralEqualityComparer"/> does not accept a custom comparer for element-level comparisons,
    /// but just calls the <see cref="object.Equals(object)"/> method on the elements.
    /// The current class can accept a custom <see cref="IEqualityComparer{TElement}"/> comparer through 
    /// its <see cref="SequenceEqualityComparer.Create{TElement}(IEqualityComparer{TElement})"/> factory method.
    /// </item>
    /// </list>
    /// <para>
    /// The implementation of the <see cref="IEqualityComparer{T}.Equals"/> method 
    /// in this class includes an optimization for in-memory collections.
    /// Sequences are trivially unequal if they contain a different number of elements.
    /// This check would obviate the need to enumerate over the sequences,
    /// but should only be performed when the number of elements is trivial to compute upfront,
    /// as should be the case for <see cref="ICollection{T}"/> implementations.
    /// This class attempts to determine the number of elements of each sequence through the 
    /// <see cref="EnumerableExtensions.TryFastCount"/> extension method.
    /// Note that, as of .NET Framework 4.6.1,
    /// <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource})"/>
    /// does not perform this check; refer to its 
    /// <see href="https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,63644a881e976b52">source code</see>.
    /// </para>
    /// <para>
    /// The implementation of the <see cref="IEqualityComparer{T}.GetHashCode(T)"/> method
    /// in this class relies on the <see cref="HashCodeCombiner.Combine{T}"/> helper method.
    /// Refer to its documentation for more details.
    /// </para>
    /// <para>
    /// This class is defined separately from <see cref="SequenceComparer{TElement}"/> for reasons
    /// discussed under the remarks of the <see cref="KeyComparer{TSource, TKey}"/> class.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://stackoverflow.com/a/20802693/1149773">How to compare arrays in a list</see> (answer), <i>Stack Overflow</i></item>
    /// <item><see href="https://stackoverflow.com/a/14675741/1149773">IEqualityComparer for SequenceEqual</see> (answer), <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    [Serializable]
    public class SequenceEqualityComparer<TElement> : EqualityComparerBase<IEnumerable<TElement>>
    {
        private readonly IEqualityComparer<TElement> _elementEqualityComparer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEqualityComparer{TElement}"/> class,
        /// using the specified equality comparer for the sequence elements.
        /// </summary>
        /// <param name="elementEqualityComparer">
        /// The equality comparison operation to apply to the elements of the sequences.
        /// If the argument is omitted or specified as <see langword="null"/>, 
        /// the <see cref="EqualityComparer{T}.Default"/> comparer for type <typeparamref name="TElement"/> is used.
        /// </param>
        protected internal SequenceEqualityComparer(IEqualityComparer<TElement> elementEqualityComparer = null)
        {
            _elementEqualityComparer = elementEqualityComparer ?? EqualityComparer<TElement>.Default;
        }

        /// <summary>
        /// Gets the default sequence equality comparer for the element type specified by the generic argument.
        /// </summary>
        public static SequenceEqualityComparer<TElement> Default { get; } = new SequenceEqualityComparer<TElement>();

        /// <summary>
        /// Determines whether two sequences of elements are equal.
        /// </summary>
        /// <param name="x">The first sequence of elements to compare.</param>
        /// <param name="y">The second sequence of elements to compare.</param>
        /// <returns><see langword="true"/> if the sequences are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="https://stackoverflow.com/q/713341/1149773">Comparing arrays in C#</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        protected override bool EqualsNonNull(IEnumerable<TElement> x, IEnumerable<TElement> y)
        {
            // Optimization: Compare lengths of in-memory collections for inequality.
            // Refer to the remarks on the class.
            int xCount, yCount;
            if (x.TryFastCount(out xCount) &&
                y.TryFastCount(out yCount) &&
                xCount != yCount)
            {
                return false;
            }

            // A for loop might be significantly faster than the enumerators-based approach implemented in SequenceEqual.
            // If this is found to be the case, then perform a manual loop-based check here for IList<TElement> collections.

            return x.SequenceEqual(y, _elementEqualityComparer);
        }

        /// <summary>
        /// Returns a hash code for the specified sequence of elements.
        /// </summary>
        /// <param name="sequence">The sequence for which to get a hash code.</param>
        /// <returns>A hash code for the specified sequence.</returns>
        protected override int GetHashCodeNonNull(IEnumerable<TElement> sequence)
        {
            return HashCodeCombiner.Combine(sequence, _elementEqualityComparer);
        }
    }
}
