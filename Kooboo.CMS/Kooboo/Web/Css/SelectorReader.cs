#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class SelectorReader
    {
        private static readonly char[] SimpleSelectorSeparators = new char[] 
        {
            SimpleSelector.IDSelectorPrefix[0],
            SimpleSelector.ClassSelectorPrefix[0],
            AttributeSelector.StartQuote,
            SimpleSelector.PseudoClassSelectorPrefix[0],
            SimpleSelector.PseudoElementSelectorPrefix[0]
        };

        private string _str;
        private int _p;
        private ReadStatus _status;

        public SelectorReader(string str)
        {
            _str = str.Trim();
            _status = ReadStatus.SimpleSelector;
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

        public char ReadCombinator()
        {
            if (_status != ReadStatus.Combinator)
                throw new InvalidReadingCallException("The function can only be called when reading status in Combinator.");

            int start = _p;
            while (!EndOfStream)
            {
                char ch = _str[_p];
                if (!IsCombinator(ch) && !IsWhiteSpace(ch))
                {
                    _status = ReadStatus.SimpleSelector;
                    break;
                }

                _p++;
            }

            if (EndOfStream)
                return Char.MinValue;

            string s = _str.Substring(start, _p - start);
            if (s.Length > 0)
            {
                s = s.Trim();
                if (s.Length == 0)
                {
                    s = " ";
                }
            }
            if (s.Length != 1)
                throw new InvalidStructureException(String.Format("Invalid combinator reading in sub string {0}.", _str.Substring(start)));

            return s[0];
        }

        public string ReadSimpleSelector()
        {
            if (_status != ReadStatus.SimpleSelector)
                throw new InvalidReadingCallException("The function can only be called when reading status in SimpleSelector.");

            int start = _p;
            while (!EndOfStream)
            {
                char ch = _str[_p];
                if (_p > start && IsSimpleSelectorSeparator(ch))
                    break;
                
                if (IsCombinator(ch))
                {
                    _status = ReadStatus.Combinator;
                    break;
                }

                _p++;
            }

            return _str.Substring(start, _p - start);
        }

        private bool IsCombinator(char ch)
        {
            return SelectorCombinator.ValidCombinators.Contains(ch);
        }

        private bool IsWhiteSpace(char ch)
        {
            return Char.IsWhiteSpace(ch);
        }

        private bool IsSimpleSelectorSeparator(char ch)
        {
            return SimpleSelectorSeparators.Contains(ch);
        }

        public enum ReadStatus
        {
            SimpleSelector,

            Combinator, 

            EndOfStream
        }
    }
}
