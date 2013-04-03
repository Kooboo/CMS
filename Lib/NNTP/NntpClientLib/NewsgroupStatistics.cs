using System;

namespace NntpClientLib
{
    [Serializable]
    public sealed class NewsgroupStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsgroupStatistics"/> class.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="estimateCount">The estimate article count.</param>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        public NewsgroupStatistics(string groupName, int estimateCount, int firstArticleId, int lastArticleId)
        {
            m_groupName = groupName;
            m_estimateCount = estimateCount;
            m_firstArticleId = firstArticleId;
            m_lastArticleId = lastArticleId;
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

        private int m_estimateCount;

        /// <summary>
        /// Gets the estimated article count.
        /// </summary>
        /// <value>The estimated count.</value>
        public int EstimatedCount
        {
            get { return m_estimateCount; }
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

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return m_groupName + " " + m_estimateCount + " " + m_firstArticleId + " " + m_lastArticleId; 
        }
    }
}

