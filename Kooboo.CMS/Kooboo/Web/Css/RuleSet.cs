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
using System.Xml.Linq;

namespace Kooboo.Web.Css
{
    public class RuleSet : Statement
    {
        public const char BlockStartQuote = '{';
        public const char BlockEndQuote = '}';
        public const char PropertyNameValueSeparator = ':';
        public const char PropertyEndToken = ';';

        private SelectorGroup _selectors;

        public IList<Selector> Selectors
        {
            get
            {
                if (_selectors == null)
                {
                    _selectors = new SelectorGroup();
                }
                return _selectors;
            }
        }

        public Declaration Declaration { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Selectors.ToString());

            builder.Append(BlockStartQuote);
            builder.Append(Declaration.ToString());
            builder.Append(BlockEndQuote);

            return builder.ToString();
        }

        public static RuleSet Parse(string str)
        {
            string s = str.Trim().TrimEnd('}');
            string[] splitted = s.Split(new char[] { '{' });
            if (splitted.Length != 2)
                throw new InvalidStructureException(String.Format("Ruleset must contains selector and declaration which quoted by {}, the exception string is {0}", str));

            RuleSet result = new RuleSet();

            foreach (var each in splitted[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                result.Selectors.Add(Selector.Parse(each));
            }

            result.Declaration = Declaration.Parse(splitted[1]);

            return result;
        }
    }
}
