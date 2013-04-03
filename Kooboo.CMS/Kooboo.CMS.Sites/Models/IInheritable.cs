using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInheritable
    {
        /// <summary>
        /// Gets the site.
        /// </summary>
        Site Site { get; }
        /// <summary>
        /// Determines whether [has parent version].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has parent version]; otherwise, <c>false</c>.
        /// </returns>
        bool HasParentVersion();

        /// <summary>
        /// Determines whether the specified site is localized.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns>
        ///   <c>true</c> if the specified site is localized; otherwise, <c>false</c>.
        /// </returns>
        bool IsLocalized(Site site);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInheritable<T> : IInheritable
    {
        /// <summary>
        /// Lasts the version.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        T LastVersion();
        /// <summary>
        /// Lasts the version.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        T LastVersion(Site site);
    }

}
