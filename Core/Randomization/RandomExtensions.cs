using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Random"/> class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns an array of bytes filled with random numbers.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance to generate the random numbers.</param>
        /// <param name="length">The length of the array of bytes to fill.</param>
        /// <returns>The array of bytes containing random numbers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="random"/> is <see langword="null"/>.</exception>
        public static byte[] NextBytes(this Random random, int length)
        {
            ArgumentValidate.NotNull(random, nameof(random));

            var buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }
    }
}
