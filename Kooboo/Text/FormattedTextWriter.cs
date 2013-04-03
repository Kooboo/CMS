using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Text
{
    public class FormattedTextWriter : TextWriter
    {
        public const int AverageWordLength = 5;

        private TextWriter _writer;
        private char[] _word;
        private int _wordLength;
        private int _lineLength;
        private int _averageLineLength;
        private int _maxLineLength;

        public FormattedTextWriter(TextWriter writer, int lineLength, int overflow = AverageWordLength)
        {
            if (overflow < 0)
                throw new ArgumentException("Overflow must greater than zero.", "overflow");

            _writer = writer;

            _averageLineLength = lineLength;
            _maxLineLength = _averageLineLength + overflow;
            _word = new char[_averageLineLength];
        }

        public override void Write(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (IsSpace(ch))
                {
                    if (IsAppendingWord())
                    {
                        WriteWord();
                    }
                    WriteSpace(ch);
                }
                else
                {
                    AppendWord(ch);
                }
            }
        }

        public override void WriteLine()
        {
            WriteWord();
            DoWriteLine();
        }

        public override void Flush()
        {
            WriteWord();
            _writer.Flush();
        }

        protected override void Dispose(bool disposing)
        {
            Flush();
            _writer.Dispose();
        }

        private void DoWriteLine()
        {
            _lineLength = 0;
            _writer.WriteLine();
        }

        private bool IsSpace(char ch)
        {
            return Char.IsWhiteSpace(ch);
        }

        private bool IsAppendingWord()
        {
            return _wordLength > 0;
        }

        private void AppendWord(char ch)
        {
            if (_wordLength >= _word.Length)
            {
                ExpandWord();
            }

            _word[_wordLength] = ch;
            _wordLength++;
        }

        private void ExpandWord()
        {
            char[] newWord = new char[_wordLength * 2];
            _word.CopyTo(newWord, 0);
            _word = newWord;
        }

        private void WriteSpace(char ch)
        {
            _writer.Write(ch);
            _lineLength++;
        }

        private void WriteWord()
        {
            if (_wordLength == 0)
                return;

            if (_lineLength >= _averageLineLength)
            {
                DoWriteLine();
            }

            if (_lineLength > 0 && _lineLength + _wordLength > _maxLineLength)
            {
                DoWriteLine();
            }
            _writer.Write(_word, 0, _wordLength);

            _lineLength += _wordLength;
            _wordLength = 0;
        }

        #region Override

        public override string NewLine
        {
            get
            {
                return _writer.NewLine;
            }
            set
            {
                base.NewLine = value;
            }
        }

        public override IFormatProvider FormatProvider
        {
            get
            {
                return _writer.FormatProvider;
            }
        }

        public override Encoding Encoding
        {
            get { return _writer.Encoding; }
        }

        public override void Close()
        {
            _writer.Close();
        }

        public override void Write(bool value)
        {
            Write(value.ToString());
        }

        public override void Write(char value)
        {
            Write(value.ToString());
        }

        public override void Write(char[] buffer)
        {
            Write(new String(buffer));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new String(buffer, index, count));
        }

        public override void Write(decimal value)
        {
            Write(value.ToString());
        }

        public override void Write(double value)
        {
            Write(value.ToString());
        }

        public override void Write(float value)
        {
            Write(value.ToString());
        }

        public override void Write(int value)
        {
            Write(value.ToString());
        }

        public override void Write(long value)
        {
            Write(value.ToString());
        }

        public override void Write(ulong value)
        {
            Write(value.ToString());
        }

        public override void Write(uint value)
        {
            Write(value.ToString());
        }

        public override void Write(object value)
        {
            Write(value.ToString());
        }

        public override void Write(string format, object arg0)
        {
            Write(String.Format(format, arg0));
        }

        public override void Write(string format, object arg0, object arg1)
        {
            Write(String.Format(format, arg0, arg1));
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(String.Format(format, arg0, arg1, arg2));
        }

        public override void Write(string format, params object[] arg)
        {
            Write(String.Format(format, arg));
        }

        public override void WriteLine(bool value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(char value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(char[] buffer)
        {
            Write(new String(buffer));
            WriteLine();
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            Write(new String(buffer, index, count));
            WriteLine();
        }

        public override void WriteLine(decimal value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(double value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(float value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(int value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(long value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(uint value)
        {
            Write(value.ToString());
            WriteLine();
        }

        public override void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }

        public override void WriteLine(string format, object arg0)
        {
            Write(String.Format(format, arg0));
            WriteLine();
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            Write(String.Format(format, arg0, arg1));
            WriteLine();
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(String.Format(format, arg0, arg1, arg2));
            WriteLine();
        }

        public override void WriteLine(string format, params object[] arg)
        {
            Write(String.Format(format, arg));
            WriteLine();
        }

        #endregion
    }
}
