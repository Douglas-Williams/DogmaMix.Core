using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogmaMix.Core.Randomization
{
    /// <summary>
    /// Provides thread-local cached instances of the <see cref="Random"/> class.
    /// </summary>
    public static class CachedRandom
    {
        private static int counter = Environment.TickCount % (16 * 1024 * 1024);

        /// <remarks>
        /// "Do not specify initial values for fields marked with ThreadStaticAttribute."
        /// </remarks>
        [ThreadStatic]
        private static Random _current;

        /// <summary>
        /// Gets a thread-local cached instance of the <see cref="Random"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property initializes a new instance of the <see cref="Random"/> class on the first time that it is called from each thread.
        /// The instance is cached in thread-local storage, and returned again on subsequent calls from the same thread.
        /// Each new <see cref="Random"/> instance is initialized with an atomically-incremented seed value,
        /// so that concurrent threads would not get the same random sequences even if they access this property simultaneously.
        /// </para>
        /// <para>
        /// Since this property accesses thread-local storage, it incurs an overhead that may be significant in performance-critical situations.
        /// In such cases, the returned instance should be stored in a local variable or field by the caller and reused.
        /// Whilst this property is thread-safe, the <see cref="Random"/> instance that it returns is not.
        /// The instance must not be used by any thread other than the one on which it was retrieved.
        /// </para>
        /// <para>
        /// This implementation is adapted from the <see href="https://csharpindepth.com/Articles/Random">Random numbers</see> article
        /// in <i>C# in Depth</i> by Jon Skeet. 
        /// It has been improved to take a modulus of the <see cref="Environment.TickCount"/> value, reducing the risk of overflow if it is close 
        /// to <see cref="int.MaxValue"/>, which is reached when the system has been running continuously for approximately 24.9 days.
        /// Whilst <see cref="Interlocked.Increment(ref int)"/> wraps around from <see cref="int.MaxValue"/> to <see cref="int.MinValue"/>,
        /// <see cref="Random(int)"/> takes the <i>absolute</i> value of the specified seed, meaning that identical random sequences would 
        /// result for pairs of seeds corresponding to <see cref="int.MaxValue"/> - <i>i</i> and <see cref="int.MinValue"/> + <i>i</i> + 1.
        /// This implementation also substitutes <see cref="ThreadStaticAttribute"/> for <see cref="ThreadLocal{T}"/>, 
        /// since the former can have better performance; see the Stack Overflow answer for 
        /// <see href="https://stackoverflow.com/a/7635342/1149773">ThreadStatic vs. ThreadLocal&lt;T&gt; Performance: speedups or alternatives?</see>
        /// </para>
        /// </remarks>
        public static Random Current
        {
            get
            {
                if (_current == null)
                    _current = new Random(Interlocked.Increment(ref counter));

                return _current;
            }
        }
    }
}
