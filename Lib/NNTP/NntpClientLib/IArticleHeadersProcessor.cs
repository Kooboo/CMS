using System;

namespace NntpClientLib
{
    public interface IArticleHeadersProcessor
    {
        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="headerAndValue">The header and value.</param>
        void AddHeader(string headerAndValue);

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="value">The value.</param>
        void AddHeader(string header, string value);
    }
}

