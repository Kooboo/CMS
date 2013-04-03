using System;
using System.Collections.Generic;

namespace NntpClientLib
{
    public class ArticleBodyTextCollection : List<string>, IArticleBodyProcessor
    {
        #region IArticleBodyProcessor Members

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="line">The line.</param>
        public void AddText(string line)
        {
            Add(line);
        }

        #endregion
    }
}

