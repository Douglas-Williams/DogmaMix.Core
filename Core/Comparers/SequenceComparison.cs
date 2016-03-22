using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Comparers
{    
    /// <summary>
    /// Specifies the sequence comparison rules to be used by the <see cref="SequenceComparer{TElement}"/> class,
    /// particularly when comparing sequences of different lengths.
    /// </summary>
    public enum SequenceComparison
    {
        /// <summary>
        /// Compares sequences using the
        /// <see href="https://en.wikipedia.org/wiki/Lexicographical_order">lexicographical order</see>.
        /// An element-by-element comparison is performed, and the sequence lengths are 
        /// only compared if one sequence is found to be a prefix of the other.
        /// </summary>
        Lexicographical,

        /// <summary>
        /// Compares sequences using the
        /// <see href="https://en.wikipedia.org/wiki/Shortlex_order">shortlex order</see>.
        /// The sequence lengths are compared first, 
        /// with a shorter sequence always considered to be less than a longer sequence. 
        /// If the sequence lengths are equal, an element-by-element comparison is performed.
        /// </summary>
        Shortlex,

        /// <summary>
        /// Compares sequences that are required to have the same length.
        /// If two sequences have different lengths, an <see cref="ArgumentException"/> is thrown.
        /// If the sequences have the same length, an element-by-element comparison is performed,
        /// like for <see cref="Lexicographical"/> or <see cref="Shortlex"/>.
        /// </summary>
        SameLength,
    }
}
