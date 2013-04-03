using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class AttributeSelector : SimpleSelector
    {
        public const char StartQuote = '[';
        public const char EndQuote = ']';
        public static readonly string[] ValidOperators = new string[] 
        {
            "~=",
            "|=",
            "^=",
            "$=",
            "*=",
            "="
        };

        public AttributeSelector(string name, string op, string value)
            : this(name)
        {
            Operator = op;
            Value = value.Trim('"', '\'');
        }

        public AttributeSelector(string name)
            : base(SelectorType.Attribute, name)
        {
        }

        public string Operator { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StartQuote);
            builder.Append(Name);
            if (!String.IsNullOrEmpty(Operator))
            {
                builder.Append(Operator);
            }
            if (!String.IsNullOrEmpty(Value))
            {
                builder.Append('"').Append(Value).Append('"');
            }
            builder.Append(EndQuote);

            return builder.ToString();
        }

        public static new AttributeSelector Parse(string str)
        {
            string s = str.Trim().TrimStart(StartQuote).TrimEnd(EndQuote).Trim();

            for (int i = 0; i < ValidOperators.Length; i++)
            {
                string op = ValidOperators[i];
                int index = s.IndexOf(op);
                if (index >= 0)
                {
                    string[] splitted = s.Split(new string[] { op }, StringSplitOptions.None);
                    if (splitted.Length != 2)
                        throw new InvalidStructureException(String.Format("Invalid attribute selector structure, exception string: \"{0}\".", str));

                    if (splitted[0].Length == 0)
                        throw new InvalidStructureException(String.Format("Attribute selector must contains attribute name, execption string: \"{0}\".", str));

                    return new AttributeSelector(splitted[0], op, splitted[1].Length == 0 ? null : splitted[1]); 
                }
            }

            return new AttributeSelector(s);
        }

        public static implicit operator string(AttributeSelector selector)
        {
            return selector.ToString();
        }

        public static implicit operator AttributeSelector(string str)
        {
            return AttributeSelector.Parse(str);
        }
    }
}
