using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Disposables;

namespace DogmaMix.Core.Collections
{
    /// <summary>
    /// Encapsulates a pair of enumerators that iterate over a specified pair of sequences in sync,
    /// up to the end of the shorter sequence,
    /// thereafter indicating the relative length of the two sequences.
    /// </summary>
    /// <typeparam name="T">The type of objects in each sequence to enumerate.</typeparam>    
    /// <remarks>
    /// <para>
    /// This class accepts a pair of <see cref="IEnumerable{T}"/> sequences,
    /// and creates their <see cref="IEnumerator{T}"/> enumerators by calling their
    /// <see cref="IEnumerable{T}.GetEnumerator()"/> method.
    /// This class follows the standard semantics as for enumerators in general
    /// with respect to access patterns for its <see cref="MoveNext"/> and <see cref="Current"/> members.
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item><see href="https://dogmamix.com/cms/blog/SequenceComparer#Advancing-enumerators-in-sync">Advancing enumerators in sync</see>, <i>DogmaMix.com</i></item>
    /// </list>
    /// </remarks>
    public class EnumeratorPair<T> : Disposable, IEnumerator<EnumeratorPair<T>.ElementPair>
    {
        private readonly IEnumerator<T> _enumerator1;
        private readonly IEnumerator<T> _enumerator2;
        
        private int _lengthComparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumeratorPair{T}"/> class
        /// that iterates over the specified pair of sequences in sync,
        /// up to the end of the shorter sequence.
        /// </summary>
        /// <param name="sequence1">The first sequence over which to iterate.</param>
        /// <param name="sequence2">The second sequence over which to iterate.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence1"/> or <paramref name="sequence2"/> is <see langword="null"/>.
        /// </exception>
        public EnumeratorPair(IEnumerable<T> sequence1, IEnumerable<T> sequence2)
        {
            ArgumentValidate.NotNull(sequence1, nameof(sequence1));
            ArgumentValidate.NotNull(sequence2, nameof(sequence2));

            _enumerator1 = sequence1.GetEnumerator();
            _enumerator2 = sequence2.GetEnumerator();
        }

        /// <summary>
        /// Gets the element in the first sequence at the current position of the enumerators.
        /// </summary>
        public T Current1 => _enumerator1.Current;

        /// <summary>
        /// Gets the element in the second sequence at the current position of the enumerators.
        /// </summary>
        public T Current2 => _enumerator2.Current;

        /// <summary>
        /// Gets the element pair from the two sequences at the current position of the enumerators.
        /// </summary>
        public ElementPair Current => new ElementPair(Current1, Current2);

        object IEnumerator.Current => Current;

        /// <summary>
        /// Whether the first enumerator has passed the end of its sequence.
        /// </summary>
        public bool PassedEnd1 { get; private set; }

        /// <summary>
        /// Whether the second enumerator has passed the end of its sequence.
        /// </summary>
        public bool PassedEnd2 { get; private set; }

        /// <summary>
        /// Whether one or both enumerators have passed the end of their sequence.
        /// </summary>
        public bool PassedEnd { get; private set; }
        
        /// <summary>
        /// Gets the relative length of the enumerated sequences,
        /// provided that one or both enumerators have passed their end.
        /// This property should only be accessed after the <see cref="MoveNext"/> method 
        /// returns <see langword="false"/>.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative length of the enumerated sequences, 
        /// as shown in the following table:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>
        /// The first enumerator passed the end of its sequence before the second enumerator.
        /// Therefore, the first sequence is shorter than the second.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// Both enumerators passed the end of their sequence during the same iteration.
        /// Therefore, the two sequences have the same length.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// The second enumerator passed the end of its sequence before the first enumerator.
        /// Therefore, the first sequence is longer than the second.
        /// </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The relative length of the two sequences is still unknown.
        /// Neither enumerator has passed the end of its sequence yet.
        /// This occurs when this property is accessed before <see cref="MoveNext"/> 
        /// returns <see langword="false"/>.
        /// </exception>
        public int LengthComparison
        {
            get
            {
                if (PassedEnd)
                    return _lengthComparison;

                throw new InvalidOperationException(
                    "The relative length of the two sequences is still unknown. " +
                    "Neither enumerator has passed the end of its sequence yet.");
            }
        }

        /// <summary>
        /// Advances both enumerators to the next element of their sequence.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if both enumerators were successfully advanced to the next element; 
        /// <see langword="false"/> if one or both enumerators have passed the end of their sequence.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// A sequence was modified after the enumerator was created.
        /// </exception>
        public bool MoveNext()
        {
            ThrowIfDisposed();

            if (!PassedEnd1)
                PassedEnd1 = !_enumerator1.MoveNext();

            if (!PassedEnd2)
                PassedEnd2 = !_enumerator2.MoveNext();

            if (!PassedEnd && (PassedEnd1 || PassedEnd2))
            {
                PassedEnd = true;

                // The boolean value "true" is greater than "false",
                // but a sequence that has passed its end is less than one that has not
                // (in terms of relative length), so the comparison result must be reversed.
                _lengthComparison = PassedEnd2.CompareTo(PassedEnd1);
            }

            return !PassedEnd;
        }

        /// <summary>
        /// Sets the two enumerators to their initial position, 
        /// which is before the first element in their sequence.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// A sequence was modified after the enumerator was created.
        /// </exception>
        public void Reset()
        {
            ThrowIfDisposed();

            _lengthComparison = 0;
            PassedEnd1 = false;
            PassedEnd2 = false;
            PassedEnd = false;

            _enumerator1.Reset();
            _enumerator2.Reset();
        }

        /// <summary>
        /// Releases all managed and unmanaged resources used by the two enumerators.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/>, since the method call always comes from 
        /// the <see cref="Disposable.Dispose()"/> method.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _enumerator1.Dispose();
                _enumerator2.Dispose();
            }
        }
        
        /// <summary>
        /// Represents a pair of elements from the two sequences at a given position of the enumerators.
        /// </summary>
        public struct ElementPair
        {
            internal ElementPair(T element1, T element2)
            {
                Element1 = element1;
                Element2 = element2;
            }

            /// <summary>
            /// The element from the first sequence.
            /// </summary>
            public T Element1 { get; }

            /// <summary>
            /// The element from the second sequence.
            /// </summary>
            public T Element2 { get; }
        }
    }
}
