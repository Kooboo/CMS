using System;
using System.Collections.Generic;
using System.IO;

namespace NntpClientLib
{
    public delegate void ReceiveLine(string line);

    public class Rfc977NntpClientWithExtensions : Rfc977NntpClient
    {
        private List<string> m_supportedCommands = new List<string>();

        private bool m_supportsXover;
        /// <summary>
        /// Gets a value indicating whether [supports xover].
        /// </summary>
        /// <value><c>true</c> if [supports xover]; otherwise, <c>false</c>.</value>
        public bool SupportsXover
        {
            get { return m_supportsXover; }
        }

        private bool m_supportsListActive;
        /// <summary>
        /// Gets a value indicating whether [supports list active].
        /// </summary>
        /// <value><c>true</c> if [supports list active]; otherwise, <c>false</c>.</value>
        public bool SupportsListActive
        {
            get { return m_supportsListActive; }
        }

        private bool m_supportsXhdr;
        /// <summary>
        /// Gets a value indicating whether [supports XHDR].
        /// </summary>
        /// <value><c>true</c> if [supports XHDR]; otherwise, <c>false</c>.</value>
        public bool SupportsXhdr
        {
            get { return m_supportsXhdr; }
        }

        private bool m_supportsXgtitle;
        /// <summary>
        /// Gets a value indicating whether [supports xgtitle].
        /// </summary>
        /// <value><c>true</c> if [supports xgtitle]; otherwise, <c>false</c>.</value>
        public bool SupportsXgtitle
        {
            get { return m_supportsXgtitle; }
        }

        private bool m_supportsXpat;

        public bool SupportsXpat
        {
            get { return m_supportsXpat; }
        }

        /// <summary>
        /// Connects using the specified host name and port number.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        public override void Connect(string hostName, int port)
        {
            base.Connect(hostName, port);
            CheckToSupportedExtensions();
        }

        /// <summary>
        /// Connects the specified host name.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public virtual void Connect(string hostName, int port, string userName, string password)
        {
            Open(hostName, port);
            AuthenticateUser(userName, password);

            CheckToSupportedExtensions();
        }

        /// <summary>
        /// Checks to supported extensions.
        /// </summary>
        private void CheckToSupportedExtensions()
        {
            foreach (string hs in RetrieveHelp())
            {
                string s = hs.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                if (s.IndexOf("xover") != -1)
                {
                    m_supportsXover = true;
                }
                else if (s.IndexOf("list") != -1)
                {
                    if (s.IndexOf("active") != -1)
                    {
                        m_supportsListActive = true;
                    }
                }
                else if (s.IndexOf("xhdr") != -1)
                {
                    m_supportsXhdr = true;
                }
                else if (s.IndexOf("xgtitle") != -1)
                {
                    m_supportsXgtitle = true;
                }
                m_supportedCommands.Add(s);
            }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public virtual void AuthenticateUser(string userName, string password)
        {
            NntpReaderWriter.WriteCommand("AUTHINFO USER " + userName);
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode == Rfc4643ResponseCodes.PasswordRequired)
            {
                NntpReaderWriter.WriteCommand("AUTHINFO PASS " + password);
                NntpReaderWriter.ReadResponse();
                if (NntpReaderWriter.LastResponseCode != Rfc4643ResponseCodes.AuthenticationAccepted)
                {
                    throw new NntpNotAuthorizedException(Resource.ErrorMessage30);
                }
            }
            else
            {
                throw new NntpNotAuthorizedException(Resource.ErrorMessage31);
            }
        }

        /// <summary>
        /// Sends the mode reader.
        /// </summary>
        public void SendModeReader()
        {
            NntpReaderWriter.WriteCommand("MODE READER");
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.ServerReadyPostingAllowed)
            {
                PostingAllowed = true;
            }
            else if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.ServerReadyNoPostingAllowed)
            {
                PostingAllowed = false;
            }
            else if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.CommandUnavailable)
            {
                throw new NntpException();
            }
        }

        /// <summary>
        /// Retrieves the newsgroups.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<NewsgroupHeader> RetrieveNewsgroups()
        {
            if (m_supportsListActive)
            {
                foreach (string s in DoBasicCommand("LIST ACTIVE", Rfc977ResponseCodes.NewsgroupsFollow))
                {
                    yield return NewsgroupHeader.Parse(s);
                }
            }
            else
            {
                // I can't use the base class to do this.
                foreach (string s in DoBasicCommand("LIST", Rfc977ResponseCodes.NewsgroupsFollow))
                {
                    yield return NewsgroupHeader.Parse(s);
                }
            }
        }

        /// <summary>
        /// Retrieves the newsgroups.
        /// </summary>
        /// <param name="wildcardMatch">The wildcard match.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsgroupHeader> RetrieveNewsgroups(string wildcardMatch)
        {
            if (m_supportsListActive)
            {
                foreach (string s in DoBasicCommand("LIST ACTIVE " + wildcardMatch, Rfc977ResponseCodes.NewsgroupsFollow))
                {
                    yield return NewsgroupHeader.Parse(s);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieves the article headers for the specified range.
        /// </summary>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        /// <returns></returns>
        public override IEnumerable<ArticleHeadersDictionary> RetrieveArticleHeaders(int firstArticleId, int lastArticleId)
        {
            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            if (m_supportsXover)
            {
                string[] headerNames = new string[] { "Article-ID", "Subject", "From", "Date", "Message-ID", "Xref", "Bytes", "Lines"};
                foreach (string s in DoArticleCommand("XOVER " + firstArticleId + "-" + lastArticleId, 224))
                {
                    ArticleHeadersDictionary headers = new ArticleHeadersDictionary();
                    string[] fields = s.Split('\t');

                    for (int i = 0; i < headerNames.Length; i++)
                    {
                        headers.AddHeader(headerNames[i], fields[i]);
                    }
                    yield return headers;
                }
            }
            else
            {
                // I can't use the base class to do this.
                for (; firstArticleId < lastArticleId; firstArticleId++)
                {
                    yield return RetrieveArticleHeader(firstArticleId);
                }
            }
        }

        /// <summary>
        /// Gets the abbreviated article headers.
        /// </summary>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        /// <param name="receiveLine">The receive line.</param>
        public void GetAbbreviatedArticleHeaders(int firstArticleId, int lastArticleId, ReceiveLine receiveLine)
        {
            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            if (m_supportsXover)
            {
                foreach (string s in DoArticleCommand("XOVER " + firstArticleId + "-" + lastArticleId, 224))
                {
                    receiveLine(s);
                }
            }
        }

        /// <summary>
        /// Retrieves the specific header.
        /// </summary>
        /// <param name="headerLine">The header line.</param>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeader(string headerLine, string messageId)
        {
            return RetrieveSpecificArticleHeaderCore("XHDR " + headerLine + " " + messageId);
        }

        /// <summary>
        /// Retrieves the specific header.
        /// </summary>
        /// <param name="headerLine">The header line.</param>
        /// <param name="articleId">The article id.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeader(string headerLine, int articleId)
        {
            return RetrieveSpecificArticleHeaderCore("XHDR " + headerLine + " " + articleId);
        }

        /// <summary>
        /// Retrieves the specific header.
        /// </summary>
        /// <param name="headerLine">The header line.</param>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeaders(string headerLine, int firstArticleId, int lastArticleId)
        {
            return RetrieveSpecificArticleHeaderCore("XHDR " + headerLine + " " + firstArticleId + "-" + lastArticleId);
        }

        /// <summary>
        /// Retrieves the specific header. This core method implements the iteration of the XHDR command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected virtual IEnumerable<string> RetrieveSpecificArticleHeaderCore(string command)
        {
            if (!m_supportsXhdr)
            {
                throw new NotImplementedException();
            }

            foreach (string s in DoArticleCommand(command, Rfc977ResponseCodes.ArticleRetrievedHeadFollows))
            {
                if (!s.StartsWith("(none)"))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Retrieves the specific article header using pattern. This method implements the XPAT NNTP extension
        /// command.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeaderUsingPattern(string header, string messageId, string[] patterns)
        {
            if (!SupportsXpat)
            {
                throw new NotImplementedException();
            }

            ValidateMessageIdArgument(messageId);

            return DoBasicCommand("XPAT " + header + " " + messageId + " " + string.Join(" ", patterns), 221);
        }

        /// <summary>
        /// Retrieves the specific article header using pattern. This method implements the XPAT NNTP extension
        /// command.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="articleId">The article id.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeaderUsingPattern(string header, int articleId, string[] patterns)
        {
            if (!SupportsXpat)
            {
                throw new NotImplementedException();
            }
            return DoBasicCommand("XPAT " + header + " " + articleId + " " + string.Join(" ", patterns), 221);
        }

        /// <summary>
        /// Retrieves the specific article header using pattern. This method implements the XPAT NNTP extension
        /// command.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveSpecificArticleHeadersUsingPattern(string header, int firstArticleId, int lastArticleId, string[] patterns)
        {
            if (!SupportsXpat)
            {
                throw new NotImplementedException();
            }
            if (lastArticleId == -1)
            {
                return DoBasicCommand("XPAT " + header + " " + firstArticleId + "- " + string.Join(" ", patterns), 221);
            }
            else
            {
                return DoBasicCommand("XPAT " + header + " " + firstArticleId + "-" + lastArticleId + " " + string.Join(" ", patterns), 221);
            }
        }

        /// <summary>
        /// Retrieves the group descriptions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> RetrieveGroupDescriptions()
        {
            if (!SupportsXgtitle)
            {
                throw new NotImplementedException();
            }
            return DoBasicCommand("XGTITLE", 282);
        }

        /// <summary>
        /// Retrieves the group descriptions.
        /// </summary>
        /// <param name="wildcardMatch">The wildcard match.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveGroupDescriptions(string wildcardMatch)
        {
            if (!SupportsXgtitle)
            {
                throw new NotImplementedException();
            }
            return DoBasicCommand("XGTITLE " + wildcardMatch, 282);
        }
    }
}
