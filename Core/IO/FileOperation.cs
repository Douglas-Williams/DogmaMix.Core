using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogmaMix.Core.IO
{
    /// <summary>
    /// Provides utility methods for performing operations on files,
    /// similar to the <see cref="File"/> class.
    /// </summary>
    public static class FileOperation
    {
        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the empty file to create.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>References</listheader>
        /// <item><see href="http://stackoverflow.com/q/802541/1149773">Creating an empty file in C#</see>, <i>Stack Overflow</i></item>
        /// </list>
        /// </remarks>
        public static void CreateEmpty(string path)
        {
            ArgumentValidate.NotNull(path, nameof(path));

            using (File.Create(path))
            {
                // Do nothing; just need "using" statement to dispose the file stream.
            }
        }
    }
}
