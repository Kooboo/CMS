using System;

namespace NntpClientLib
{
    [Serializable]
    public sealed class NewsgroupHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsgroupHeader"/> class.
        /// </summary>
        private NewsgroupHeader()
        {
        }

        private string m_groupName;
        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName
        {
            get { return m_groupName; }
        }

        private int m_firstArticleId;
        /// <summary>
        /// Gets the first article id.
        /// </summary>
        /// <value>The first article id.</value>
        public int FirstArticleId
        {
            get { return m_firstArticleId; }
        }

        private int m_lastArticleId;
        /// <summary>
        /// Gets the last article id.
        /// </summary>
        /// <value>The last article id.</value>
        public int LastArticleId
        {
            get { return m_lastArticleId; }
        }

        private char m_statusCode;
        /// <summary>
        /// Gets the status code. This will be indicate whether posting is allowed to this group ("y") or not ("n")
        /// or postings will be forwarded to the newsgroup moderator ("m").
        /// </summary>
        /// <value>The status code.</value>
        public char StatusCode
        {
            get { return m_statusCode; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return GroupName + " " + FirstArticleId + " " + LastArticleId + " " + StatusCode;
        }

        /// <summary>
        /// Parses the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static NewsgroupHeader Parse(string response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            string[] parts = response.Split(new char[] { ' ' });
            if (parts.Length < 3)
            {
                throw new ArgumentException(Resource.ErrorMessage13);
            }

            NewsgroupHeader h = new NewsgroupHeader();
            h.m_groupName = parts[0];
            h.m_lastArticleId = Rfc977NntpClient.ConvertToInt32(parts[1]);
            h.m_firstArticleId = Rfc977NntpClient.ConvertToInt32(parts[2]);
            h.m_statusCode = ((parts.Length > 3 && parts[3].Length > 0) ? h.m_statusCode = parts[3][0] : 'y');
            return h;
        }
    }
}

