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
    public class Selector
    {
        public const int ImportantSpecificity = 10000;
        public const int InlineSpecificity = 1000;
        public const int IDSpecificity = 100;
        public const int AttributeSpecificity = 10;
        public const int TypeSpecificity = 1;

        public int Specificity
        {
            get
            {
                return Chain.Sum(o => o.Item2.Specificity);
            }
        }

        public SimpleSelector PseudoElementSelector { get; private set; }

        private List<Tuple<SelectorCombinator, SimpleSelectorChain>> _chain;
        protected IList<Tuple<SelectorCombinator, SimpleSelectorChain>> Chain
        {
            get
            {
                if (_chain == null)
                {
                    _chain = new List<Tuple<SelectorCombinator, SimpleSelectorChain>>();
                }
                return _chain;
            }
        }

        public void Add(SelectorCombinator combinator, SimpleSelectorChain selector)
        {
            if (Chain.Count > 0 && combinator == null)
                throw new ArgumentNullException("Combinator could not be null if selector is not in first node of chain.");

            Chain.Add(new Tuple<SelectorCombinator, SimpleSelectorChain>(combinator, selector));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var each in Chain)
            {
                if (each.Item1 != null)
                {
                    builder.Append(each.Item1);
                }
                builder.Append(each.Item2.ToString());
            }

            return builder.ToString();
        }

        public string ToStringWithoutPseudoElementSelector()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var each in Chain)
            {
                if (each.Item1 != null)
                {
                    builder.Append(each.Item1);
                }
                builder.Append(each.Item2.ToStringWithoutPseudoElementSelector());
            }

            return builder.ToString();
        }

        public static Selector Parse(string str)
        {
            Selector result = new Selector();

            SelectorReader reader = new SelectorReader(str);
            SelectorCombinator combinator = null;
            SimpleSelectorChain simpleSelectors = new SimpleSelectorChain();

            while (!reader.EndOfStream)
            {
                switch (reader.Status)
                {
                    case SelectorReader.ReadStatus.Combinator:
                        result.Add(combinator, simpleSelectors);                      
                        combinator = reader.ReadCombinator();
                        simpleSelectors = new SimpleSelectorChain();
                        break;
                    case SelectorReader.ReadStatus.SimpleSelector:
                        SimpleSelector simpleSelector = reader.ReadSimpleSelector();
                        if (simpleSelector.Type == SelectorType.PseudoElement)
                        {
                            result.PseudoElementSelector = simpleSelector;
                        }
                        simpleSelectors.Add(simpleSelector);
                        break;
                }
            }
            result.Add(combinator, simpleSelectors);

            return result;
        }

        public static implicit operator string(Selector combinator)
        {
            return combinator.ToString();
        }

        public static implicit operator Selector(string str)
        {
            return Selector.Parse(str);
        }
    }
}
