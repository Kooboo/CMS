using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;

namespace NntpClientLib
{
    internal class NntpStreamReader : TextReader
    {
        const int DefaultBufferSize = 1024;
        const int DefaultFileBufferSize = 4096;
        const int MinimumBufferSize = 512;

        //
        // The input array
        //
        byte[] m_inputBuffer;

        //
        // The decoded array from the above input array
        //
        char[] m_decodedBuffer;

        //
        // Decoded bytes in m_decodedBuffer.
        //
        int m_decodedCount;

        //
        // Current position in the m_decodedBuffer
        //
        int m_currentDecodePosition;

        //
        // The array size that we are using
        //
        int m_bufferSize;

        Encoding m_encoding;
        Decoder m_decoder;

        Stream m_baseStream;
        bool m_mayBlock;
        StringBuilder m_lineBuilder;

        private NntpStreamReader() { }

        public NntpStreamReader(Stream stream)
            : this(stream, Rfc977NntpClient.DefaultEncoding, DefaultBufferSize) { }

        public NntpStreamReader(Stream stream, Encoding encoding, int bufferSize)
        {
            Initialize(stream, encoding, bufferSize);
        }

        internal void Initialize(Stream stream, Encoding encoding, int bufferSize)
        {
            if (null == stream)
            {
                throw new ArgumentNullException("stream");
            }
            if (null == encoding)
            {
                throw new ArgumentNullException("encoding");
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException(Resource.ErrorMessage44);
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentException(Resource.ErrorMessage43, "bufferSize");
            }

            if (bufferSize < MinimumBufferSize)
            {
                bufferSize = MinimumBufferSize;
            }

            m_baseStream = stream;
            m_inputBuffer = new byte[bufferSize];
            this.m_bufferSize = bufferSize;
            this.m_encoding = encoding;
            m_decoder = encoding.GetDecoder();

            m_decodedBuffer = new char[encoding.GetMaxCharCount(bufferSize)];
            m_decodedCount = 0;
            m_currentDecodePosition = 0;
        }

        public virtual Stream BaseStream
        {
            get { return m_baseStream; }
        }

        public virtual Encoding CurrentEncoding
        {
            get
            {
                if (m_encoding == null)
                {
                    throw new InvalidOperationException();
                }
                return m_encoding;
            }
        }

        public bool EndOfStream
        {
            get { return Peek() < 0; }
        }

        public override void Close()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && m_baseStream != null)
                {
                    m_baseStream.Close();
                }

                m_inputBuffer = null;
                m_decodedBuffer = null;
                m_encoding = null;
                m_decoder = null;
                m_baseStream = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public void DiscardBufferedData()
        {
            m_decoder = m_encoding.GetDecoder();
            m_currentDecodePosition = 0;
            m_decodedCount = 0;
            m_mayBlock = false;
            
        }

        private int ReadBuffer()
        {
            m_currentDecodePosition = 0;
            int cbEncoded = 0;

            m_decodedCount = 0;
            int parse_start = 0;
            do
            {
                cbEncoded = m_baseStream.Read(m_inputBuffer, 0, m_bufferSize);

                if (cbEncoded <= 0)
                {
                    return 0;
                }

                m_mayBlock = (cbEncoded < m_bufferSize);

                m_decodedCount += m_decoder.GetChars(m_inputBuffer, parse_start, cbEncoded, m_decodedBuffer, 0);
                parse_start = 0;
            } while (m_decodedCount == 0);

            return m_decodedCount;
        }

        public override int Peek()
        {
            CheckObjectState();

            if (m_currentDecodePosition >= m_decodedCount && (m_mayBlock || ReadBuffer() == 0))
            {
                return -1;
            }

            return m_decodedBuffer[m_currentDecodePosition];
        }

        public override int Read()
        {
            CheckObjectState();

            if (m_currentDecodePosition >= m_decodedCount && ReadBuffer() == 0)
            {
                return -1;
            }

            return m_decodedBuffer[m_currentDecodePosition++];
        }

        public override int Read([In, Out] char[] destinationBuffer, int index, int count)
        {
            CheckObjectState();

            if (destinationBuffer == null)
            {
                throw new ArgumentNullException("destinationBuffer");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (index > (destinationBuffer.Length - count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int charsRead = 0;
            while (count > 0)
            {
                if (m_currentDecodePosition >= m_decodedCount && ReadBuffer() == 0)
                {
                    return charsRead > 0 ? charsRead : 0;
                }

                int cch = Math.Min(m_decodedCount - m_currentDecodePosition, count);
                Array.Copy(m_decodedBuffer, m_currentDecodePosition, destinationBuffer, index, cch);
                m_currentDecodePosition += cch;
                index += cch;
                count -= cch;
                charsRead += cch;
            }
            return charsRead;
        }

        bool foundCR;

        int FindNextEOL()
        {
            char c = '\0';
            for (; m_currentDecodePosition < m_decodedCount; m_currentDecodePosition++)
            {
                c = m_decodedBuffer[m_currentDecodePosition];
                if (c == '\n' && foundCR)
                {
                    m_currentDecodePosition++;
                    int res = (m_currentDecodePosition - 2);
                    if (res < 0)
                    {
                        res = 0; // if a new array starts with a \n and there was a \r at the end of the previous one, we get here.
                    }
                    foundCR = false;
                    return res;
                }

                foundCR = (c == '\r');
            }

            return -1;
        }

        public override string ReadLine()
        {
            CheckObjectState();

            if (m_currentDecodePosition >= m_decodedCount && ReadBuffer() == 0)
            {
                return null;
            }

            int begin = m_currentDecodePosition;
            int end = FindNextEOL();
            if (end < m_decodedCount && end >= begin)
            {
                return new string(m_decodedBuffer, begin, end - begin);
            }

            if (m_lineBuilder == null)
            {
                m_lineBuilder = new StringBuilder();
            }
            else
            {
                m_lineBuilder.Length = 0;
            }

            while (true)
            {
                if (foundCR) // don't include the trailing CR if present
                {
                    m_decodedCount--;
                }

                m_lineBuilder.Append(m_decodedBuffer, begin, m_decodedCount - begin);
                if (ReadBuffer() == 0)
                {
                    if (m_lineBuilder.Capacity > 32768)
                    {
                        StringBuilder sb = m_lineBuilder;
                        m_lineBuilder = null;
                        return sb.ToString(0, sb.Length);
                    }
                    return m_lineBuilder.ToString(0, m_lineBuilder.Length);
                }

                begin = m_currentDecodePosition;
                end = FindNextEOL();
                if (end < m_decodedCount && end >= begin)
                {
                    m_lineBuilder.Append(m_decodedBuffer, begin, end - begin);
                    if (m_lineBuilder.Capacity > 32768)
                    {
                        StringBuilder sb = m_lineBuilder;
                        m_lineBuilder = null;
                        return sb.ToString(0, sb.Length);
                    }
                    return m_lineBuilder.ToString(0, m_lineBuilder.Length);
                }
            }
        }

        public override string ReadToEnd()
        {
            CheckObjectState();

            StringBuilder text = new StringBuilder();

            int size = m_decodedBuffer.Length;
            char[] buffer = new char[size];
            int len;

            while ((len = Read(buffer, 0, size)) > 0)
            {
                text.Append(buffer, 0, len);
            }

            return text.ToString();
        }

        private void CheckObjectState()
        {
            if (m_baseStream == null)
            {
                throw new InvalidOperationException(Resource.ErrorMessage45);
            }
        }
    }
}

