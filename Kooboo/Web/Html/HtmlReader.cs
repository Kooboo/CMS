using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Web.Html
{
    public class HtmlReader
    {
        public const string CDATA = "<![CDATA[";

        public static readonly string[] AbbreviatableTags = new string[] 
        {
            "area",
            "br",
            "col",
            "embed",
            "hr",
            "img",
            "input",
            "link",
            "base",
            "basefont",
            "meta",
            "param",
            "frame"
        };

        private string _html;
        private int _p;
        private ReadStatus _status;

        public HtmlReader(string html)
        {
            _html = html;
            _status = ReadStatus.Text;
            MoveToNextElement();
        }

        public int Position
        {
            get
            {
                return _p;
            }
        }

        public bool EndOfReading
        {
            get
            {
                return _p >= _html.Length;
            }
        }

        public bool MoveToAttribute(string attributeName)
        {
            MoveToTagStart();
            MoveToFirstAttribute();

            while (!IsTagEnd())
            {
                if (String.Compare(ReadAttributeName(), attributeName, true) == 0)
                {
                    return true;
                }

                MoveToNextAttribute();
            }

            return false;
        }

        public bool MoveToFirstAttribute()
        {
            MoveToTagStart();
            return MoveToNextAttribute();
        }

        public bool MoveToNextAttribute()
        {
            while (!IsAttributeBreak())
            {
                if (IsTagEnd())
                    return false;

                _p++;
            }

            SkipSpace();

            if (IsTagEnd())
                return false;

            return true;
        }

        public bool IsStartTag()
        {
            MoveToTagStart();
            return !IsEndTagStart();
        }

        public bool IsTag()
        {
            return _status == ReadStatus.Tag;
        }

        public string ReadTagName()
        {
            MoveToTagStart();
            SkipTagStart();

            int start = _p;
            while (!IsAttributeBreak())
                _p++;

            return _html.Substring(start, _p - start);
        }

        public string ReadAttributeName()
        {
            int start = _p;
            while (!IsAttributeNameValueSeparator() && !IsAttributeBreak())
                _p++;

            return _html.Substring(start, _p - start);
        }

        public string ReadAttributeValue()
        {
            if (IsAttributeNameValueSeparator())
            {
                // Skip "="
                _p++;
            }

            while (IsSpace())
                _p++;

            char quote = Char.MinValue;
            if (IsAttributeQuote())
            {
                // Skip quote " or '
                quote = _html[_p];
                _p++;
            }
            else
            {
                return null;
            }

            int start = _p;
            while (_html[_p] != quote)
                _p++;

            string result = _html.Substring(start, _p - start);
            return result;

        }

        public bool MoveToNextElement()
        {
            if (EndOfReading)
                return false;

            if (_status == ReadStatus.Tag)
            {
                MoveToTagEnd();
                SkipTagEnd();

                if (EndOfReading)
                    return false;

                _status = IsTagStart() ? ReadStatus.Tag : ReadStatus.Text;
            }
            else
            {
                while (!EndOfReading && !(IsTagStart() && !IsCDATAStart()))
                    _p++;

                if (EndOfReading)
                    return false;

                _status = ReadStatus.Tag;
            }

            return true;
        }

        public string ReadNode()
        {
            MoveToTagStart();
            int start = _p;
            MoveToNextNode();

            return _html.Substring(start, _p - start);
        }

        public string ReadTag()
        {
            MoveToTagStart();

            int start = _p;
            MoveToNextElement();

            return _html.Substring(start, _p - start);
        }

        public string ReadText()
        {
            int start = _p;
            MoveToNextElement();

            return _html.Substring(start, _p - start);
        }

        public void MoveToNextNode()
        { 
            if (!IsTag() || IsEndTagStart())
                return;

            var context = RecordContext();
            string startTagName = ReadTagName();
            if (AbbreviatableTags.Contains(startTagName))
            {
                MoveToNextElement();
                return;
            }
                
            MoveToTagEnd();
            if (IsStartTagEnd())
            {
                MoveToNextElement();
                return;
            }

            MoveToNextElement();
            int stack = 1;
            while (!EndOfReading && stack > 0)
            {
                if (IsTag())
                {
                    if (IsEndTagStart())
                    {
                        stack--;
                        if (stack == 0)
                        {
                            string endTagName = ReadTagName();
                            if (String.Compare(startTagName, endTagName, true) != 0)
                                throw new InvalidHtmlException(String.Format("Html is invalid, start tag {0} ({1}) and end tag {2} are not matched", startTagName, context, endTagName));
                        }
                    }
                    else
                    {
                        string tagName = ReadTagName();
                        MoveToTagEnd();
                        if (!IsStartTagEnd())
                        {
                            if (!AbbreviatableTags.Contains(tagName))
                            {
                                stack++;
                            }
                        }
                    }
                }

                MoveToNextElement();
            }
        }

        public void MoveToTagStart()
        {
            while (!IsTagStart())
                _p--;
        }

        private void MoveToTagEnd()
        {
            while (!IsTagEnd())
                _p++;
        }

        private bool IsSpace()
        {
            return Char.IsWhiteSpace(_html[_p]);
        }

        private bool IsAttributeBreak()
        {
            return Char.IsWhiteSpace(_html[_p]) || IsTagEnd();
        }

        private bool IsTagEnd()
        {
            return _html[_p] == '>' || IsStartTagEnd();
        }

        private bool IsStartTagEnd()
        {
            if (_p == _html.Length - 1)
                return false;

            return _html.Substring(_p, 2) == "/>";
        }

        private bool IsTagStart()
        {
            return _html[_p] == '<';
        }

        private bool IsCDATAStart()
        {
            if (_p + CDATA.Length > _html.Length)
            {
                // overflow the html length
                return false;
            }
            else
            {
                return _html.Substring(_p, CDATA.Length) == CDATA;
            }
        }

        private bool IsEndTagStart()
        {
            return _html.Substring(_p, 2) == "</";
        }

        private bool IsAttributeNameValueSeparator()
        {
            return _html[_p] == '=';
        }

        private bool IsAttributeQuote()
        {
            return _html[_p] == '"' || _html[_p] == '\'';
        }

        private void SkipTagEnd()
        {
            if (IsStartTagEnd())
            {
                _p += 2;
            }
            else
            {
                _p++;
            }
        }

        private void SkipTagStart()
        {
            if (IsEndTagStart())
            {
                _p += 2;
            }
            else
            {
                _p++;
            }
        }

        private void SkipSpace()
        {
            while (IsSpace()) 
                _p++;
        }

        private string RecordContext()
        {
            var result = _html.Substring(_p, Math.Min(_html.Length - _p, 50));
            result = result.Replace(Environment.NewLine, String.Empty);
            result = Regex.Replace(result, @"\s+", " ");
            return "..." + result + "...";
        }

        internal enum ReadStatus
        {
            Text,

            Tag
        }
    }


}
