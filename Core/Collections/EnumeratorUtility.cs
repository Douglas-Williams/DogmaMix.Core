using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Collections
{
    /// <summary>
    /// Provides utility methods for enumerators. 
    /// Enumerators are objects that implement the <see cref="IEnumerator{T}"/> interface.
    /// </summary>
    public static class EnumeratorUtility
    {
        /// <summary>
        /// Advances both of the specified enumerators to the next element of their sequence.
        /// </summary>
        /// <param name="xEnumerator">The enumerator for the first sequence.</param>
        /// <param name="yEnumerator">The enumerator for the second sequence.</param>
        /// <param name="lengthComparison">
        /// When this method returns <see langword="true"/>, this argument always contains zero, 
        /// indicating that the relative length of the sequences being enumerated is still unknown.
        /// When this method returns <see langword="false"/>, this argument contains a signed integer 
        /// that indicates the relative length of the sequences being enumerated, 
        /// as shown in the following table:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>
        /// <paramref name="xEnumerator"/> has passed the end of its sequence, 
        /// but <paramref name="yEnumerator"/> has not. 
        /// Therefore, the sequence enumerated by <paramref name="xEnumerator"/> is shorter
        /// than the sequence enumerated by <paramref name="yEnumerator"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// Both <paramref name="xEnumerator"/> and <paramref name="yEnumerator"/> have passed the end of their sequence. 
        /// Therefore, the sequences enumerated by the two enumerators have the same length.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// <paramref name="yEnumerator"/> has passed the end of its sequence, 
        /// but <paramref name="xEnumerator"/> has not. 
        /// Therefore, the sequence enumerated by <paramref name="xEnumerator"/> is longer
        /// than the sequence enumerated by <paramref name="yEnumerator"/>.
        /// </term>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>
        /// <see langword="true"/> if both enumerators were successfully advanced to the next element; 
        /// <see langword="false"/> if one or both enumerators have passed the end of their sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="xEnumerator"/> or <paramref name="yEnumerator"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A sequence was modified after the enumerator was created.
        /// </exception>
        public static bool MoveNextBoth<T>(
            IEnumerator<T> xEnumerator, 
            IEnumerator<T> yEnumerator, 
            out int lengthComparison)
        {
            ArgumentValidate.NotNull(xEnumerator, nameof(xEnumerator));
            ArgumentValidate.NotNull(yEnumerator, nameof(yEnumerator));

            bool xMove = xEnumerator.MoveNext();
            bool yMove = yEnumerator.MoveNext();

            lengthComparison = xMove.CompareTo(yMove);
            return xMove && yMove;
        }
    }
}
