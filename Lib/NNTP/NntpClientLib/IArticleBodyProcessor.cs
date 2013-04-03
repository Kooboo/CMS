using System;

namespace NntpClientLib
{
    public interface IArticleBodyProcessor
    {
        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="line">The line.</param>
        void AddText(string line);
    }
}

