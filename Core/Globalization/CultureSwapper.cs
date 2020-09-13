using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogmaMix.Core.Disposables;

namespace DogmaMix.Core.Globalization
{
    /// <summary>
    /// Substitutes the specified culture on the current thread for the lifetime of the current instance,
    /// restoring the former culture when disposed.
    /// </summary>
    public class CultureSwapper : Disposable
    {
        private readonly CultureInfo formerCulture;

        /// <summary>
        /// Sets the culture used by the current thread to the <paramref name="culture"/> instance,
        /// and creates an <see cref="IDisposable"/> that restores the former culture when 
        /// its <see cref="IDisposable.Dispose()"/> method is called.
        /// </summary>
        /// <param name="culture">The culture to set for the lifetime of this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="culture"/> is <see langword="null"/>.</exception>
        public CultureSwapper(CultureInfo culture)
        {
            ArgumentValidate.NotNull(culture, nameof(culture));

            formerCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = culture;
        }

        /// <summary>
        /// Restores the former culture that was active when this instance was created.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/>, since the method call always comes from 
        /// the <see cref="Disposable.Dispose()"/> method.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                CultureInfo.CurrentCulture = formerCulture;
        }
    }
}
