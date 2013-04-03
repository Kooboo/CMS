using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NntpClientLib
{
    /// <summary>
    /// <para>
    /// This is a very lightweight NNTP client library. It is written a base class the implements
    /// the RFC977 and a subclass that implements common extensions. At some point one might implement
    /// the RFC3997 as a stand alone replacement for both of these classes. I've not tried to make that 
    /// kind of transition transparent.
    /// </para>
    /// <para>
    /// I'm using iterators to avoid building collections (either arrays or collection objects) to 
    /// hold the contents of the articles or headers that we receive from the server. Thus, one has
    /// this template for most of the protocol requests
    /// <code>
    ///     string server = "freenews.netfront.net";
    ///     using (Rfc977NntpClient client = new Rfc977NntpClient())
    ///     {
    ///         client.Connect(server);
    ///         int groupCount = 0;
    ///         foreach (NewsgroupHeader h in client.RetrieveNewsgroups())
    ///         {
    ///              groupCount++;
    ///         }
    ///    }
    /// </code>
    /// For more examples of how this library is used, please see the NUnit tests included in the
    /// distribution.
    /// </para>
    /// <para>
    /// One interesting aspect of this is the default text encoding for communication between the 
    /// library and the NNTP server. Most of the time it's undocumented and others, it's either 7 bit
    /// ASCII or UTF8. Here I'm using ISO-8859-1 because I want to correctly deal with 8 bit article
    /// bodies (possibly encoded in yEnc).
    /// </para>
    /// </summary>
    public class Rfc977NntpClient : IDisposable
    {
        internal static readonly Encoding DefaultEncoding = Encoding.GetEncoding("iso-8859-1");

        internal static IFormatProvider FormatProvider
        {
            get { return System.Globalization.CultureInfo.InvariantCulture; }
        }

        internal static System.Globalization.CultureInfo CultureInfo
        {
            get { return System.Globalization.CultureInfo.InvariantCulture; }
        }

        internal static int ConvertToInt32(string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            return Convert.ToInt32(argument, CultureInfo);
        }

        private NewsgroupStatistics m_currentGroup;

        /// <summary>
        /// Gets a value indicating whether a newgroup is selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if newsgroup selected; otherwise, <c>false</c>.
        /// </value>
        public bool CurrentGroupSelected
        {
            get { return m_currentGroup != null; }
        }

        /// <summary>
        /// Gets the current group.
        /// </summary>
        /// <value>The current group.</value>
        public NewsgroupStatistics CurrentGroup
        {
            get { return m_currentGroup; }
        }

        private bool m_postingIsAllowed;

        /// <summary>
        /// Gets or sets a value indicating whether posting is allowed.
        /// </summary>
        /// <value><c>true</c> if posting is allowed; otherwise, <c>false</c>.</value>
        public bool PostingAllowed
        {
            get { return m_postingIsAllowed; }
            protected set { m_postingIsAllowed = value; }
        }

        private int m_connectionTimeout = -1;

        /// <summary>
        /// Gets or sets the connection timeout.
        /// </summary>
        /// <value>The connection timeout.</value>
        public int ConnectionTimeout
        {
            get { return m_connectionTimeout; }
            set { m_connectionTimeout = value; }
        }

        private int m_port;

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return m_port; }
        }

        private string m_host;

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host
        {
            get { return m_host; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Rfc977NntpClient"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get { return (m_connection != null && m_connection.Connected); }
        }

        NntpProtocolReaderWriter m_nntpStream;
        internal NntpProtocolReaderWriter NntpReaderWriter
        {
            get { return m_nntpStream; }
            set { m_nntpStream = value; }
        }

        private TextWriter m_logger;
        public TextWriter ProtocolLogger
        {
            set
            {
                m_logger = value;
                if (NntpReaderWriter != null)
                {
                    NntpReaderWriter.LogWriter = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc977NntpClient"/> class.
        /// The most common usage pattern is: 
        /// <code>
        /// using (Rfc977NntpClient client = new Rfc977Client())
        /// {
        ///     client.Connect("nntp.server.net", 119);
        ///     foreach (NewsgroupHeader header in client.RetrieveNewsgroups())
        ///     {
        ///         Console.WriteLine(header);
        ///     }
        /// }
        /// </code>
        /// </summary>
        public Rfc977NntpClient()
        {
        }

        private TcpClient m_connection;

        /// <summary>
        /// Connects using the specified host name.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public void Connect(string hostName)
        {
            Connect(hostName, 119);
        }

        /// <summary>
        /// Connects using the specified host name and port number.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        public virtual void Connect(string hostName, int port)
        {
            Open(hostName, port);

            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.ServerReadyPostingAllowed)
            {
                m_postingIsAllowed = true;
            }
            else if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.ServerReadyNoPostingAllowed)
            {
                m_postingIsAllowed = false;
            }
            else
            {
                throw new NntpResponseException(Resource.ErrorMessage01, NntpReaderWriter.LastResponse);
            }
        }

        /// <summary>
        /// Opens the specified host name.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        protected virtual void Open(string hostName, int port)
        {
            m_host = hostName;
            m_port = port;
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            m_connection = new TcpClient(hostName, port);
            NntpReaderWriter = new NntpProtocolReaderWriter(m_connection);
            if (m_logger != null)
            {
                NntpReaderWriter.LogWriter = m_logger;
            }
            if (ConnectionTimeout != -1)
            {
                m_connection.SendTimeout = m_connection.ReceiveTimeout = ConnectionTimeout;
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            if (m_connection == null)
            {
                return;
            }
            try
            {
                if (m_connection.Connected)
                {
                    NntpReaderWriter.WriteCommand("QUIT");

                    NntpReaderWriter.Dispose();
                    m_nntpStream = null;
                    m_connection.Close();
                }
            }
            finally
            {
                try
                {
                    m_connection.Close();
                }
                catch
                {
                }
                m_connection = null;
            }
        }

        /// <summary>
        /// Gets the last NNTP command.
        /// </summary>
        /// <value>The last NNTP command.</value>
        public string LastNntpCommand
        {
            get { return (NntpReaderWriter == null ? null : NntpReaderWriter.LastCommand); }
        }

        /// <summary>
        /// Gets the last NNTP response.
        /// </summary>
        /// <value>The last NNTP response.</value>
        public string LastNntpResponse
        {
            get { return (NntpReaderWriter == null ? null : NntpReaderWriter.LastResponse); }
        }

        /// <summary>
        /// Retrieves the help content from the server. 
        /// </summary>
        /// <remarks>
        /// Below is an example of the output of this command.
        /// <code>
        /// authinfo user Name|pass Password
        /// article [MessageID|Number]
        /// body [MessageID|Number]
        /// check MessageID
        /// value
        /// group newsgroup
        /// head [MessageID|Number]
        /// help
        /// ihave
        /// last
        /// list [active|active.times|newsgroups|subscriptions]
        /// listgroup newsgroup
        /// mode stream
        /// mode reader
        /// newgroups yymmdd hhmmss [GMT] [&lt;distributions&gt;]
        /// newnews newsgroups yymmdd hhmmss [GMT] [&lt;distributions&gt;]
        /// next
        /// post
        /// slave
        /// stat [MessageID|Number]
        /// takethis MessageID
        /// xgtitle [group_pattern]
        /// xhdr header [range|MessageID]
        /// xover [range]
        /// xpat header range|MessageID pat [morepat...]	
        /// </code>
        /// </remarks>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveHelp()
        {
            return DoBasicCommand("HELP", Rfc977ResponseCodes.HelpTextFollows);
        }

        /// <summary>
        /// Retrieves the newsgroups.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<NewsgroupHeader> RetrieveNewsgroups()
        {
            foreach (string s in DoBasicCommand("LIST", Rfc977ResponseCodes.NewsgroupsFollow))
            {
                yield return NewsgroupHeader.Parse(s);
            }
        }

        /// <summary>
        /// Retrieves the new newsgroups.
        /// </summary>
        /// <param name="dateTime">The value time.</param>
        /// <returns></returns>
        public IEnumerable<NewsgroupHeader> RetrieveNewNewsgroups(DateTime dateTime)
        {
            return RetrieveNewNewsgroups(dateTime, TimeZoneOption.None, null);
        }

        /// <summary>
        /// Retrieves the new newsgroups.
        /// </summary>
        /// <param name="dateTime">The value time.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="distributions">The distributions.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsgroupHeader> RetrieveNewNewsgroups(DateTime dateTime, TimeZoneOption timeZone, string distributions)
        {
            string command = string.Format("NEWGROUPS {0:yyMMdd} {0:HHmmss}", dateTime);
            if (timeZone == TimeZoneOption.UseGreenwichMeanTime)
            {
                command += " GMT";
            }

            if (!string.IsNullOrEmpty(distributions))
            {
                command += " ";
                command += distributions;
            }

            foreach (string s in DoBasicCommand(command, Rfc977ResponseCodes.NewNewsgroupsFollow))
            {
                yield return NewsgroupHeader.Parse(s);
            }
        }

        /// <summary>
        /// Retrieves the new news for all of the groups.
        /// </summary>
        /// <param name="dateTime">The value time.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveNewNews(DateTime dateTime)
        {
            return RetrieveNewNews("*", dateTime, TimeZoneOption.None, null);
        }

        /// <summary>
        /// Retrieves the new news for the newsgroups that match the wildcard.
        /// </summary>
        /// <param name="newsgroupWildcardMatch">The newsgroup wildcard match.</param>
        /// <param name="dateTime">The value time.</param>
        /// <returns></returns>
        public IEnumerable<string> RetrieveNewNews(string newsgroupWildcardMatch, DateTime dateTime)
        {
            return RetrieveNewNews(newsgroupWildcardMatch, dateTime, TimeZoneOption.None, null);
        }

        /// <summary>
        /// Retrieves the new news.
        /// </summary>
        /// <param name="newsgroupWildcardMatch">The newsgroup wildcard match.</param>
        /// <param name="dateTime">The value time.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="distributions">The distributions.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveNewNews(string newsgroupWildcardMatch, DateTime dateTime, TimeZoneOption timeZone, string distributions)
        {
            string command = string.Format("NEWNEWS {0} {1:yyMMdd} {1:HHmmss}", newsgroupWildcardMatch, dateTime);
            if (timeZone == TimeZoneOption.UseGreenwichMeanTime)
            {
                command += " GMT";
            }

            if (!string.IsNullOrEmpty(distributions))
            {
                command += " ";
                command += distributions;
            }

            foreach (string s in DoBasicCommand(command, Rfc977ResponseCodes.NewArticlesFollow))
            {
                yield return s;
            }
        }

        /// <summary>
        /// Selects the newsgroup.
        /// </summary>
        /// <param name="group">The group.</param>
        public virtual void SelectNewsgroup(string group)
        {
            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException("group");
            }

            ValidateConnectionState();

            NntpReaderWriter.WriteCommand("GROUP " + group);

            string response = NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.NewsgroupSelected)
            {
                string[] parts = response.Split(' ');
                NewsgroupStatistics g = new NewsgroupStatistics(group, ConvertToInt32(parts[1]), ConvertToInt32(parts[2]), ConvertToInt32(parts[3]));
                m_currentGroup = g;
            }
            else
            {
                m_currentGroup = null;
                if (NntpReaderWriter.LastResponseCode == Rfc977ResponseCodes.NoSuchNewsgroup)
                {
                    throw new NntpGroupNotSelectedException();
                }
                else
                {
                    throw new NntpResponseException(Resource.ErrorMessage02, NntpReaderWriter.LastResponse);
                }
            }
        }

        /// <summary>
        /// Sets the previous article.
        /// </summary>
        /// <returns></returns>
        public virtual ArticleResponseIds SetPreviousArticle()
        {
            return SetArticleCursor("LAST");
        }

        /// <summary>
        /// Sets the next article.
        /// </summary>
        /// <returns></returns>
        public virtual ArticleResponseIds SetNextArticle()
        {
            return SetArticleCursor("NEXT");
        }

        /// <summary>
        /// Sets the article cursor.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        private ArticleResponseIds SetArticleCursor(string direction)
        {
            if (direction == null)
            {
                throw new ArgumentNullException("direction");
            }
            
            if (!(direction.Equals("LAST", StringComparison.InvariantCultureIgnoreCase) || direction.Equals("NEXT", StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException(Resource.ErrorMessage03, "direction");
            }

            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            ValidateConnectionState();

            NntpReaderWriter.WriteCommand(direction);
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode != Rfc977ResponseCodes.ArticleRetrievedTextSeparate)
            {
                throw new NntpResponseException(Resource.ErrorMessage04, NntpReaderWriter.LastResponse);
            }
            return ArticleResponseIds.Parse(NntpReaderWriter.LastResponse);
        }

        /// <summary>
        /// Retrieves the statistics for a current article.
        /// </summary>
        /// <returns></returns>
        public ArticleResponseIds RetrieveStatistics()
        {
            return RetrieveStatisticsCore("STAT");
        }

        /// <summary>
        /// Retrieves the statistics for the selected article.
        /// </summary>
        /// <param name="articleId">The article id.</param>
        /// <returns></returns>
        public ArticleResponseIds RetrieveStatistics(int articleId)
        {
            // TODO: validate id?
            return RetrieveStatisticsCore("STAT " + articleId);
        }

        /// <summary>
        /// Retrieves the statistics for the selected article.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public ArticleResponseIds RetrieveStatistics(string messageId)
        {
            ValidateMessageIdArgument(messageId);
            return RetrieveStatisticsCore("STAT " + messageId);
        }

        /// <summary>
        /// Retrieves the statistics for an article based on the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected virtual ArticleResponseIds RetrieveStatisticsCore(string command)
        {
            ValidateConnectionState();

            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            NntpReaderWriter.WriteCommand(command);
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode != Rfc977ResponseCodes.ArticleRetrievedTextSeparate)
            {
                throw new NntpResponseException(Resource.ErrorMessage05, NntpReaderWriter.LastResponse);
            }
            return ArticleResponseIds.Parse(NntpReaderWriter.LastResponse);
        }

        /// <summary>
        /// Retrieves the article headers for the current article.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ArticleHeadersDictionary> RetrieveArticleHeaders()
        {
            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            return RetrieveArticleHeaders(CurrentGroup.FirstArticleId, CurrentGroup.LastArticleId);
        }

        /// <summary>
        /// Retrieves the article headers for the specified range. Since this iterates over the article identifiers and
        /// that range may contain identifiers that are not legal (i.e. they don't exist on the server), this method will
        /// catch NNTP RFC 997 423 response codes and therefore skip the article identifier.
        /// </summary>
        /// <param name="firstArticleId">The first article id.</param>
        /// <param name="lastArticleId">The last article id.</param>
        /// <returns></returns>
        public virtual IEnumerable<ArticleHeadersDictionary> RetrieveArticleHeaders(int firstArticleId, int lastArticleId)
        {
            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            for (; firstArticleId < lastArticleId; firstArticleId++)
            {
                ArticleHeadersDictionary d = null;
                try
                {
                    d = RetrieveArticleHeader(firstArticleId);
                }
                catch (NntpResponseException error)
                {
                    if (error.LastResponseCode == 423)
                    {
                        continue;
                    }
                    throw error;
                }
                yield return d;
            }
        }

        /// <summary>
        /// Retrieves the article header the current article.
        /// </summary>
        /// <returns></returns>
        public virtual ArticleHeadersDictionary RetrieveArticleHeader()
        {
            return RetrieveArticleHeaderCore("HEAD");
        }

        /// <summary>
        /// Retrieves the article header for the specified article.
        /// </summary>
        /// <param name="articleId">The article id.</param>
        /// <returns></returns>
        public virtual ArticleHeadersDictionary RetrieveArticleHeader(int articleId)
        {
            return RetrieveArticleHeaderCore("HEAD " + articleId);
        }

        /// <summary>
        /// Retrieves the article header for the specified article.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public virtual ArticleHeadersDictionary RetrieveArticleHeader(string messageId)
        {
            ValidateMessageIdArgument(messageId);
            return RetrieveArticleHeaderCore("HEAD " + messageId);
        }

        /// <summary>
        /// Retrieves the article header common functionality. The command argument
        /// should be in the form "HEAD [article-id|message-id]."
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected ArticleHeadersDictionary RetrieveArticleHeaderCore(string command)
        {
            ArticleHeadersDictionary headers = new ArticleHeadersDictionary();
            foreach (string s in DoArticleCommand(command, Rfc977ResponseCodes.ArticleRetrievedHeadFollows))
            {
                if (s.Length == 0)
                {
                    break;
                }
                else
                {
                    headers.AddHeader(s);
                }
            }
            return headers;
        }

        public virtual IEnumerable<string> RetrieveArticleBody()
        {
            return DoArticleCommand("BODY", Rfc977ResponseCodes.ArticleRetrievedBodyFollows);
        }

        /// <summary>
        /// Retrieves the article body for the specified article.
        /// </summary>
        /// <param name="articleId">The article id.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveArticleBody(int articleId)
        {
            return DoArticleCommand("BODY " + articleId, Rfc977ResponseCodes.ArticleRetrievedBodyFollows);
        }

        /// <summary>
        /// Retrieves the article body for the specified message id.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveArticleBody(string messageId)
        {
            ValidateMessageIdArgument(messageId);
            return DoArticleCommand("BODY " + messageId, Rfc977ResponseCodes.ArticleRetrievedBodyFollows);
        }

        /// <summary>
        /// Retrieves the article that is currently selected.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="body">The body.</param>
        public virtual void RetrieveArticle(IArticleHeadersProcessor header, IArticleBodyProcessor body)
        {
            RetrieveArticleCore("ARTICLE", header, body);
        }

        /// <summary>
        /// Retrieves the article for the specified article id.
        /// </summary>
        /// <param name="articleId">The article id.</param>
        /// <param name="header">The header.</param>
        /// <param name="body">The body.</param>
        public virtual void RetrieveArticle(int articleId, IArticleHeadersProcessor header, IArticleBodyProcessor body)
        {
            RetrieveArticleCore("ARTICLE " + articleId, header, body);
        }

        /// <summary>
        /// Retrieves the article for the specified message id.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="header">The header.</param>
        /// <param name="body">The body.</param>
        public virtual void RetrieveArticle(string messageId, IArticleHeadersProcessor header, IArticleBodyProcessor body)
        {
            ValidateMessageIdArgument(messageId);
            RetrieveArticleCore("ARTICLE " + messageId, header, body);
        }

        private void RetrieveArticleCore(string command, IArticleHeadersProcessor headers, IArticleBodyProcessor body)
        {
            bool readingHeader = true;
            foreach (string s in DoArticleCommand(command, Rfc977ResponseCodes.ArticleRetrieved))
            {
                if (readingHeader)
                {
                    if (s.Length == 0)
                    {
                        readingHeader = false;
                    }
                    else
                    {
                        headers.AddHeader(s);
                    }
                }
                else
                {
                    body.AddText(s);
                }
            }
        }

        /// <summary>
        /// Posts the article.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="body">The body.</param>
        public virtual void PostArticle(IArticleHeaderEnumerator header, IEnumerable<string> body)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            ValidateConnectionState();

            NntpReaderWriter.WriteCommand("POST");
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode != Rfc977ResponseCodes.SendArticleToPost)
            {
                throw new NntpResponseException(Resource.ErrorMessage06, NntpReaderWriter.LastResponse);
            }

            foreach (string key in header.HeaderKeys)
            {
                int count = 0;
                foreach (string v in header[key])
                {
                    if (count > 0)
                    {
                        NntpReaderWriter.Write("\t");
                    }
                    else
                    {
                        NntpReaderWriter.Write(key);
                        NntpReaderWriter.Write(": ");
                    }
                    NntpReaderWriter.WriteLine(v);
                    count++;
                }
            }

            NntpReaderWriter.WriteLine("");
            foreach (string s in body)
            {
                if (s.StartsWith("."))
                {
                    NntpReaderWriter.Write(".");
                }
                NntpReaderWriter.WriteLine(s);
            }
            NntpReaderWriter.WriteLine(".");
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode != Rfc977ResponseCodes.ArticlePostedOk)
            {
                throw new NntpResponseException(Resource.ErrorMessage07, NntpReaderWriter.LastResponse);
            }
        }

        /// <summary>
        /// Sends the slave command.
        /// </summary>
        public virtual void SendSlave()
        {
            ValidateConnectionState();

            NntpReaderWriter.WriteLine("SLAVE");
            NntpReaderWriter.ReadResponse();
            if (NntpReaderWriter.LastResponseCode != Rfc977ResponseCodes.SlaveStatusNoted)
            {
                throw new NntpResponseException(Resource.ErrorMessage08, NntpReaderWriter.LastResponse);
            }
        }

        /// <summary>
        /// Validates the message id argument.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        protected static void ValidateMessageIdArgument(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException("messageId");
            }
            if (!(messageId.StartsWith("<") && messageId.EndsWith(">")))
            {
                throw new ArgumentException(Resource.ErrorMessage09, "messageId");
            }
            if (messageId.Length < 3)
            {
                throw new ArgumentException(Resource.ErrorMessage10, "messageId");
            }
        }

        /// <summary>
        /// Validates the state of the connection.
        /// </summary>
        protected void ValidateConnectionState()
        {
            if (m_connection == null || !m_connection.Connected)
            {
                throw new InvalidOperationException(Resource.ErrorMessage11);
            }
        }

        /// <summary>
        /// Does the article command but checks that a group is currently selected.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="expectedResponseCode">The expected response code.</param>
        /// <returns></returns>
        protected IEnumerable<string> DoArticleCommand(string command, int expectedResponseCode)
        {
            if (!CurrentGroupSelected)
            {
                throw new NntpGroupNotSelectedException();
            }

            return DoBasicCommand(command, expectedResponseCode);
        }

        /// <summary>
        /// Does the basic command. In the NNTP protocol, a command is sent and the server
        /// possibly returns some text and finally is returns a response code. If a server
        /// returned line equal a single "." we are done and nothing more is returned. If
        /// the server returns a ".." (double period) the leading period is removed and the
        /// remaining string is returned.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="expectedResponseCode">The expected response code.</param>
        /// <returns></returns>
        protected IEnumerable<string> DoBasicCommand(string command, int expectedResponseCode)
        {
            ValidateConnectionState();
            NntpReaderWriter.WriteCommand(command);
            NntpReaderWriter.ReadResponse();

            if (NntpReaderWriter.LastResponseCode != expectedResponseCode)
            {
                throw new NntpResponseException(Resource.ErrorMessage12, NntpReaderWriter.LastResponse);
            }

            do
            {
                string line = NntpReaderWriter.ReadLine();
                if (line.Equals("."))
                {
                    break;
                }
                else if (line.StartsWith(".."))
                {
                    line = line.Substring(1);
                }
                yield return line;
            } while (true);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected virtual void Dispose(bool disposing)
        {
            if (m_connection == null) return;
            if (disposing)
            {
                Close();
            }
        }
    }
}

