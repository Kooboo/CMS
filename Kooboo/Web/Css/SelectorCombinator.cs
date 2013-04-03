using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class SelectorCombinator
    {
        public const char DescendantCombinator = ' ';
        public const char ChildCombinator = '>';
        public const char SiblingCombinator = '+';
        public const char GeneralSiblingCombinator = '~';

        public static readonly char[] ValidCombinators = new char[] 
        {
            DescendantCombinator,
            ChildCombinator,
            SiblingCombinator,
            GeneralSiblingCombinator
        };

        public SelectorCombinator(char ch)
        {
            Value = ch;
        }

        public override string ToString()
        {
            return new String(new char[] { Value });
        }

        public char Value { get; private set; }

        public static implicit operator char(SelectorCombinator combinator)
        {
            return combinator.Value;
        }

        public static implicit operator SelectorCombinator(char ch)
        {
            return new SelectorCombinator(ch);
        }
    }
}
