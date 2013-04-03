using System;
using System.Collections.Generic;
using System.Text;

namespace NntpClientLib
{
    public class ArticleHeadersDictionaryEnumerator : IArticleHeaderEnumerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleHeadersDictionaryEnumerator"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public ArticleHeadersDictionaryEnumerator(Dictionary<string, List<string>> dictionary)
        {
            m_dict = dictionary;
        }

        Dictionary<string, List<string>> m_dict;

        #region IArticleHeaderEnumerator Members
        /// <summary>
        /// Gets the header keys.
        /// </summary>
        /// <value>The header keys.</value>
        public IEnumerable<string> HeaderKeys
        {
            get { return m_dict.Keys; }
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IList&lt;System.String&gt;"/> with the specified header key.
        /// Each named header can potentially have multiple values, so we manage this with a list.
        /// </summary>
        /// <value></value>
        public IList<string> this[string headerKey]
        {
            get { return m_dict[headerKey]; }
        }
        #endregion
    }
}
