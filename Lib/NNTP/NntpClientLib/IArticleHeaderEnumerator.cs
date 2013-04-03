using System;
using System.Collections.Generic;
using System.Text;

namespace NntpClientLib
{
    public interface IArticleHeaderEnumerator
    {
        /// <summary>
        /// Gets the header keys.
        /// </summary>
        /// <value>The header keys.</value>
        IEnumerable<string> HeaderKeys { get; }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IList&lt;System.String&gt;"/> with the specified header key.
        /// Each named header can potentially have multiple values, so we manage this with a list.
        /// </summary>
        /// <value></value>
        IList<string> this[string headerKey] { get; }
    }
}
