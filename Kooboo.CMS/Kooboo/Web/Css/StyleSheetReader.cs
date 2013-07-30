#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class StyleSheetReader
    {
        private string _str;
        private int _p;
        private ReadStatus _status = ReadStatus.RuleSet;

        public StyleSheetReader(TextReader reader)
        {
            Initialize(reader.ReadToEnd());
        }

        public StyleSheetReader(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Initialize(reader.ReadToEnd());
            }
        }

        public StyleSheetReader(string str)
        {
            Initialize(str);
        }

        public ReadStatus Status
        {
            get
            {
                return _status;
            }
        }

        public bool EndOfStream
        {
            get
            {
                if (_status == ReadStatus.EndOfStream)
                    return true;

                if (_p < _str.Length)
                    return false;

                _status = ReadStatus.EndOfStream;
                return true;
            }
        }

        public string ReadAtRule()
        {
            if (_status != ReadStatus.AtRule)
                throw new InvalidReadingCallException("The function can only be called when reading status in AtRule.");

            int start = _p;
            int inBlock = 0;
            while (!EndOfStream)
            {
                char ch = _str[_p];
                if (ch == RuleSet.BlockStartQuote)
                {
                    inBlock++;
                }
                else if (ch == RuleSet.BlockEndQuote && inBlock > 1)
                {
                    inBlock--;
                }
                else if ((ch == RuleSet.BlockEndQuote && inBlock == 1) || (ch == AtRule.EndToken && inBlock == 0))
                {
                    _p++;
                    string result = _str.Substring(start, _p - start);
                    SkipToNextNode();
                    return result;
                }

                _p++;
            }

            return _str.Substring(start);
        }

        public string ReadRuleSet()
        {
            if (_status != ReadStatus.RuleSet)
                throw new InvalidReadingCallException("The function can only be called when reading status in RuleSet.");

            int start = _p;
            while (!EndOfStream)
            {
                char ch = _str[_p];
                if (ch == RuleSet.BlockEndQuote)
                {
                    _p++;
                    string result = _str.Substring(start, _p - start);
                    SkipToNextNode();
                    return result;
                }

                _p++;
            }

            return _str.Substring(start);
        }

        public string ReadComment()
        {
            if (_status != ReadStatus.Comment)
                throw new InvalidReadingCallException("The function can only be called when reading status in Comment.");

            int start = _p;
            while (!EndOfStream)
            {
                if (_p > 0 && _str.Substring(_p - 1, 2) == Comment.EndToken)
                {
                    _p++;
                    string result = _str.Substring(start, _p - start);
                    SkipToNextNode();
                    return result;
                }

                _p++;
            }

            return _str.Substring(start);
        }

        private void Initialize(string str)
        {
            _str = str;
            SkipToNextNode();
        }

        private void SkipToNextNode()
        {
            SkipWhiteSpace();
            if (EndOfStream)
                return;

            if (_str[_p] == AtRule.StartToken)
            {
                _status = ReadStatus.AtRule;
            }
            else if (_str.Substring(_p, 2) == Comment.StartToken)
            {
                _status = ReadStatus.Comment;
            }
            else
            {
                _status = ReadStatus.RuleSet;
            }
        }

        private void SkipWhiteSpace()
        {
            while (!EndOfStream)
            {
                char ch = _str[_p];
                if (!IsWhiteSpace(ch))
                    break;

                _p++;
            }
        }

        private bool IsWhiteSpace(char ch)
        {
            return Char.IsWhiteSpace(ch);
        }

        public enum ReadStatus
        {
            AtRule,
            RuleSet,
            Comment,
            EndOfStream
        }
    }
}
