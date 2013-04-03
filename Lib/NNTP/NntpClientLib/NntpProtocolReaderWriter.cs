using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;

namespace NntpClientLib
{
    internal class NntpProtocolReaderWriter : IDisposable
    {
        private TcpClient m_connection;
        private NetworkStream m_network;
        private StreamWriter m_writer;
        private NntpStreamReader m_reader;
        private TextWriter m_log;

        public TextWriter LogWriter
        {
            get { return m_log; }
            set { m_log = value; }
        }

        private System.Text.Encoding m_enc = Rfc977NntpClient.DefaultEncoding;
        internal Encoding DefaultTextEncoding
        {
            get { return m_enc; }
        }

        internal NntpProtocolReaderWriter(TcpClient connection)
        {
            m_connection = connection;
            m_network = m_connection.GetStream();
            m_writer = new StreamWriter(m_network, DefaultTextEncoding);
            m_writer.AutoFlush = true;
            m_reader = new NntpStreamReader(m_network);
        }

        internal string ReadLine()
        {
            string s = m_reader.ReadLine();
            if (m_log != null)
            {
                m_log.Write(">> ");
                m_log.WriteLine(s);
            }
            return s;
        }

        internal string ReadResponse()
        {
            m_lastResponse = m_reader.ReadLine();
            if (m_log != null)
            {
                m_log.WriteLine("< " + m_lastResponse);
            }
            return m_lastResponse;
        }

        private string m_lastResponse;
        internal string LastResponse
        {
            get { return m_lastResponse; }
        }

        internal int LastResponseCode
        {
            get
            {
                if (string.IsNullOrEmpty(m_lastResponse))
                {
                    throw new InvalidOperationException(Resource.ErrorMessage41);
                }
                if (m_lastResponse.Length > 2)
                {
                    return Convert.ToInt32(m_lastResponse.Substring(0, 3), System.Globalization.CultureInfo.InvariantCulture);
                }
                throw new InvalidOperationException(Resource.ErrorMessage42);
            }
        }

        private string m_lastCommand;
        internal string LastCommand
        {
            get { return m_lastCommand; }
        }

        internal void WriteCommand(string line)
        {
            if (m_log != null)
            {
                m_log.WriteLine("> " + line);
            }
            m_lastCommand = line;
            m_writer.WriteLine(line);
        }

        internal void WriteLine(string line)
        {
            if (m_log != null)
            {
                m_log.WriteLine("> " + line);
            }
            m_writer.WriteLine(line);
        }

        internal void Write(string line)
        {
            if (m_log != null)
            {
                m_log.Write("> " + line);
            }
            m_writer.Write(line);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_connection == null)
            {
                return;
            }
            try
            {
                m_writer.Close();
            }
            catch { }
            m_writer = null;

            try
            {
                m_reader.Close();
            }
            catch { }
            m_reader = null;

            if (m_connection != null)
            {
                try
                {
                    m_connection.GetStream().Close();
                }
                catch { }
            }
        }

        #endregion
    }
}

