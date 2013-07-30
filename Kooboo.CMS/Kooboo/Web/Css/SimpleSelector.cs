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
    public class SimpleSelector
    {
        public const string UniversalSelectorToken = "*";
        public const string IDSelectorPrefix = "#";
        public const string ClassSelectorPrefix = ".";
        public const string PseudoClassSelectorPrefix = ":";
        public const string PseudoElementSelectorPrefix = "::";
        public static readonly string[] PseudoElementSelectorNames = new string[] 
        {
            "first-line", "first-letter", "before", "after"
        };

        public SimpleSelector(SelectorType type, string name)
        {
            Type = type;
            Name = name;
        }

        public int Specificity
        {
            get
            {
                switch (Type)
                {
                    case SelectorType.ID:
                        return Selector.IDSpecificity;
                    case SelectorType.Class:
                    case SelectorType.PseudoClass:
                    case SelectorType.Attribute:
                        return Selector.AttributeSpecificity;
                    case SelectorType.Type:
                    case SelectorType.PseudoElement:
                        return Selector.TypeSpecificity;
                    default:
                        return 0;
                }
            }
        }

        public string Name { get; set; }

        public SelectorType Type { get; private set; }

        public override string ToString()
        {
            switch(Type)
            {
                case SelectorType.Universal:
                    return UniversalSelectorToken;
                case SelectorType.Type:
                    return Name;
                case SelectorType.ID:
                    return IDSelectorPrefix + Name;
                case SelectorType.Class:
                    return ClassSelectorPrefix + Name;
                case SelectorType.PseudoClass:
                    return PseudoClassSelectorPrefix + Name;
                case SelectorType.PseudoElement:
                    return PseudoElementSelectorPrefix + Name;
                default:
                    throw new NotSupportedException();
            }
        }

        public static SimpleSelector Parse(string str)
        {
            string s = str.Trim();
            if (s == UniversalSelectorToken)
            {
                return new SimpleSelector(SelectorType.Universal, "*");
            }

            if (s.StartsWith(IDSelectorPrefix))
            {
                return new SimpleSelector(SelectorType.ID, s.Substring(IDSelectorPrefix.Length));
            }

            if (s.StartsWith(ClassSelectorPrefix))
            {
                return new SimpleSelector(SelectorType.Class, s.Substring(ClassSelectorPrefix.Length));
            }

            if (s.StartsWith(PseudoElementSelectorPrefix))
            {
                return new SimpleSelector(SelectorType.PseudoElement, s.Substring(PseudoElementSelectorPrefix.Length));
            }

            if (s.StartsWith(PseudoClassSelectorPrefix))
            {
                string name = s.Substring(PseudoClassSelectorPrefix.Length);
                if (PseudoElementSelectorNames.Contains(name))
                {
                    return new SimpleSelector(SelectorType.PseudoElement, name);
                }
                else
                {
                    return new SimpleSelector(SelectorType.PseudoClass, name);
                }
            }

            if (s[0] == AttributeSelector.StartQuote)
            {
                return AttributeSelector.Parse(s);
            }

            return new SimpleSelector(SelectorType.Type, s);
        }

        public static implicit operator string(SimpleSelector selector)
        {
            return selector.ToString();
        }

        public static implicit operator SimpleSelector(string str)
        {
            return SimpleSelector.Parse(str);
        }
    }
}
