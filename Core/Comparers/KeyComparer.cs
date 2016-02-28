using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DogmaMix.Core.Comparers
{
    /// <summary>
    /// Represents a generic sort-order comparison operation that compares keys extracted from source items.
    /// </summary>
    /// <typeparam name="TSource">The type of the source items whose keys are to be extracted.</typeparam>
    /// <typeparam name="TKey">The type of the keys on which the comparison is internally performed.</typeparam>
    /// <remarks>
    /// <para>
    /// This class is useful for creating data structures that store a collection of elements sorted by a custom key,
    /// but do not otherwise require the key for element access.
    /// The <see cref="SortedSet{T}"/> class can maintain a collection in sorted order, but has no inherent support for keys.
    /// By supplying an instance of this <see cref="KeyComparer{TSource, TKey}"/> class to a <see cref="SortedSet{T}"/> constructor
    /// that takes an <see cref="IComparer{T}"/> argument (such as <see cref="SortedSet{T}(IComparer{T})"/>),
    /// a structure can be created that automatically extracts and sorts by the custom key.
    /// By comparison, the <see cref="SortedList{TKey, TValue}"/> and <see cref="SortedDictionary{TKey, TValue}"/> structures
    /// have explicit support for keys, but require them to be manually provided by the callers –
    /// for example, through the <see cref="SortedList{TKey, TValue}.Add(TKey, TValue)"/> method –
    /// which is less convenient than automatic key extraction.
    /// </para>
    /// <para>
    /// This class is defined separately from <see cref="KeyEqualityComparer{TSource, TKey}"/> due to potential inconsistencies
    /// between the encapsulated <see cref="Comparer{T}"/> and <see cref="EqualityComparer{T}"/> comparers for type <typeparamref name="TKey"/>.
    /// These inconsistencies may arise whether default or custom comparers are used.
    /// The <see cref="Comparer{T}.Default"/> and <see cref="EqualityComparer{T}.Default"/> instances of the said comparers
    /// rely on the implementations of the <see cref="IComparable{T}"/> and <see cref="IEquatable{T}"/> interfaces
    /// by the <typeparamref name="TKey"/> type.
    /// Such inconsistencies occur even for types defined in the .NET Framework Class Library.
    /// For example, the <see cref="Comparer{T}.Default"/> sort-order comparer for <see cref="string"/> calls 
    /// <see cref="string.CompareTo(string)"/>, which performs a culture-sensitive comparison using the current culture. 
    /// On the other hand, its <see cref="EqualityComparer{T}.Default"/> equality comparer calls 
    /// <see cref="string.Equals(string)"/>, which performs an ordinal comparison.
    /// Using the two default comparers together can lead to anomalies:
    /// <c>Comparer&lt;string&gt;.Default.Compare("café", "cafe\u0301")</c> evaluates to <c>0</c>, indicating equality, whilst
    /// <c>EqualityComparer&lt;string&gt;.Default.Equals("café", "cafe\u0301")</c> evalutes to <see langword="false"/>, indicating inequality.
    /// It would be undesirable to encapsulate such inconsistent comparers within the same class.
    /// MSDN implicitly acknowledges the issue under the remarks for <see cref="Comparer{T}.Default"/>,
    /// recommending the <see cref="StringComparer"/> class instead:
    /// <blockquote>
    /// For string comparisons, the <see cref="StringComparer"/> class is recommended over <see cref="Comparer{String}"/>.
    /// Properties of the <see cref="StringComparer"/> class return predefined instances that perform string comparisons 
    /// with different combinations of culture-sensitivity and case-sensitivity. The case-sensitivity and culture-sensitivity 
    /// are consistent among the members of the same <see cref="StringComparer"/> instance.
    /// </blockquote>
    /// </para>
    /// <list type="bullet">
    /// <listheader>See Also</listheader>
    /// <item><see href="http://stackoverflow.com/a/21550837/1149773">Can I use Linq to create a comparer for a C# sorted dictionary</see> (answer), <i>Stack Overflow</i></item>
    /// </list>
    /// </remarks>
    public class KeyComparer<TSource, TKey> : Comparer<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IComparer<TKey> _innerComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyComparer{TSource, TKey}"/> class,
        /// using the specified key extraction function and the <see cref="EqualityComparer{T}.Default"/> sort-order comparer 
        /// for type <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public KeyComparer(Func<TSource, TKey> keySelector)
            : this(keySelector, Comparer<TKey>.Default)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyComparer{TSource, TKey}"/> class,
        /// using the specified key extraction function and the specified sort-order comparer for extracted keys.
        /// </summary>
        /// <param name="keySelector">A function to extract the key from a source item.</param>
        /// <param name="innerComparer">The sort-order comparison operation to apply to the extracted keys.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> or <paramref name="innerComparer"/> is <see langword="null"/>.</exception>
        public KeyComparer(Func<TSource, TKey> keySelector, IComparer<TKey> innerComparer)
        {
            ArgumentValidate.NotNull(keySelector, nameof(keySelector));
            ArgumentValidate.NotNull(innerComparer, nameof(innerComparer));

            _keySelector = keySelector;
            _innerComparer = innerComparer;
        }

        /// <summary>
        /// Compares the keys extracted from two source items and returns a value indicating 
        /// whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first source item to compare.</param>
        /// <param name="y">The second source item to compare.</param>
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
        /// The key for <paramref name="x"/> is less than <paramref name="y"/>. -or- 
        /// <paramref name="x"/> is <see langword="null"/> and <paramref name="y"/> is not <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// The key for <paramref name="x"/> is equal to <paramref name="y"/>. -or- 
        /// <paramref name="x"/> and <paramref name="y"/> are both <see langword="null"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// The key for <paramref name="x"/> is greater than <paramref name="y"/>. -or- 
        /// <paramref name="x"/> is not <see langword="null"/> and <paramref name="y"/> is <see langword="null"/>.
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        public override int Compare(TSource x, TSource y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            
            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);
            return _innerComparer.Compare(xKey, yKey);
        }
    }
}
