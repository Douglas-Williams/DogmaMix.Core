using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogmaMix.Core.Extensions
{
    /// <summary>
    /// Provides extension methods of the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all bytes from the current position to the end of the stream,
        /// and advances the position of the stream to its end.
        /// </summary>
        /// <param name="stream">The stream to read.</param>The size of the buffer.
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>
        /// The rest of the stream as a byte array, from the current position to the end. 
        /// If the current position is at the end of the stream, returns an empty byte array.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This extension method is modeled after the <see cref="StreamReader.ReadToEnd"/> method of the <see cref="StreamReader"/> class,
        /// but reads byte sequences instead of strings.
        /// </para>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/q/221925/1149773">How to convert an Stream into a byte[] in C#?</see>, <i>Stack Overflow</i></item>
        /// <item><see href="http://stackoverflow.com/q/1080442/1149773">Creating a byte array from a stream</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static byte[] ReadToEnd(this Stream stream, int bufferSize = 81920)
        {
            ArgumentValidate.NotNull(stream, nameof(stream));

            // Can be optimized to avoid copying twice in some cases
            // (from input stream to memory stream, and from memory stream to byte array).

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream, bufferSize);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Asynchronously reads all bytes from the current position to the end of the stream,
        /// and advances the position of the stream to its end.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>
        /// A task that represents the asynchronous read operation.
        /// The task result contains the rest of the stream as a byte array, from the current position to the end. 
        /// If the current position is at the end of the stream, returns an empty byte array.
        /// </returns>
        public static async Task<byte[]> ReadToEndAsync(this Stream stream, int bufferSize = 81920, CancellationToken cancellationToken = default(CancellationToken))
        {
            ArgumentValidate.NotNull(stream, nameof(stream));

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream, bufferSize, cancellationToken);
                return memoryStream.ToArray();
            }
        }        
    }
}
