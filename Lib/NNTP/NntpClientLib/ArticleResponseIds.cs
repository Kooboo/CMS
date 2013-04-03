using System;

namespace NntpClientLib
{
    [Serializable]
    public sealed class ArticleResponseIds
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleResponseIds"/> class.
        /// </summary>
        private ArticleResponseIds()
        {
        }

        private int m_articleId;
        /// <summary>
        /// Gets the article id.
        /// </summary>
        /// <value>The article id.</value>
        public int ArticleId
        {
            get { return m_articleId; }
        }

        private string m_messageId;
        /// <summary>
        /// Gets the message id.
        /// </summary>
        /// <value>The message id.</value>
        public string MessageId
        {
            get { return m_messageId; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return m_articleId + " " + m_messageId;
        }

        /// <summary>
        /// Parses the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static ArticleResponseIds Parse(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                throw new ArgumentNullException("response");
            }
            ArticleResponseIds a = new ArticleResponseIds();
            string[] sa = response.Split(new char[] { ' ' });
            if (sa.Length == 2)
            {
                a.m_articleId = Rfc977NntpClient.ConvertToInt32(sa[0]);
                a.m_messageId = sa[1];
            }
            else if (sa.Length > 2)
            {
                a.m_articleId = Rfc977NntpClient.ConvertToInt32(sa[1]);
                a.m_messageId = sa[2];
            }
            else
            {
                throw new ArgumentException(Resource.ErrorMessage48, "response");
            }
            return a;
        }
    }
}

