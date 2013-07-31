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
    public partial class Combiner<T> : IEnumerable<KeyValuePair<T, Combiner<T>.NodeStyle>>
    {
        private IDom<T> _dom;
        private Dictionary<T, NodeStyle> _nodeStyles = new Dictionary<T, NodeStyle>();

        public Combiner(IDom<T> nodeSelector)
        {
            _dom = nodeSelector;
        }

        public NodeStyle this[T node]
        {
            get
            {
                if (_nodeStyles.ContainsKey(node))
                {
                    return _nodeStyles[node];
                }

                return null;
            }
        }

        public void AddStyleSheet(string styleSheet)
        {
            AddStyleSheet(StyleSheet.Load(styleSheet));
        }

        public void AddStyleSheet(StyleSheet sheet)
        {
            foreach (var each in sheet.Statements.OfType<RuleSet>())
            {
                AddRuleSet(each);
            }
        }

        public void AddRuleSet(string ruleSet)
        {
            AddRuleSet(RuleSet.Parse(ruleSet));
        }

        public void AddRuleSet(RuleSet set)
        {
            foreach (var selector in set.Selectors)
            {
                if (selector.PseudoElementSelector != null)
                {
                    var pseudoSelector = selector.PseudoElementSelector;
                    if (pseudoSelector.Name == "before")
                    {
                        AddPseudoElement(selector.ToStringWithoutPseudoElementSelector(), set.Declaration, (n, h) => _dom.InsertBefore(n, h));
                    }
                    else if (pseudoSelector.Name == "after")
                    {
                        AddPseudoElement(selector.ToStringWithoutPseudoElementSelector(), set.Declaration, (n, h) => _dom.InsertAfter(n, h));
                    }
                }
                else
                {
                    AddStyle(selector, set.Declaration);
                }
            }
        }

        private void AddPseudoElement(Selector selector, Declaration declaration, Action<T, string> action)
        {

            IEnumerable<T> selectedNodes;
            try
            {
                selectedNodes = _dom.Select(selector).ToArray();
            }
            catch
            {
                return;
            }

            var contentProperty = declaration.FirstOrDefault(o => o.Name == "content");
            string content = null;
            if (contentProperty == null)
            {
                content = String.Empty;
            }
            else
            {
                content = contentProperty.Value.Trim().Trim('\"');
                declaration.Remove("content");
            }
            string html = String.Format("<div style=\"{0}\">{1}</div>", declaration.ToString(), content);
            foreach (var node in selectedNodes)
            {
                action(node, html);
            }
        }

        private void AddStyle(Selector selector, Declaration declaration)
        {
            IEnumerable<T> selectedNodes;
            try
            {
                selectedNodes = _dom.Select(selector);
            }
            catch
            {
                return;
            }

            foreach (var node in selectedNodes)
            {
                foreach (var property in declaration)
                {
                    AddStyle(node, selector.Specificity, property);
                }
            }
        }

        public void AddInlineStyle(T node, Declaration declaration)
        {
            foreach (var each in declaration)
            {
                AddInlineStyle(node, each);
            }
        }

        public void AddInlineStyle(T node, string style)
        {
            AddInlineStyle(node, Declaration.Parse(style));
        }

        public void AddInlineStyle(T node, Property property)
        {
            AddStyle(node, Selector.InlineSpecificity, property);
        }

        public void AddStyle(T node, int specifility, Property property)
        {
            NodeStyle nodeStyle;
            if (_nodeStyles.ContainsKey(node))
            {
                nodeStyle = _nodeStyles[node];
            }
            else
            {
                nodeStyle = new NodeStyle();
                _nodeStyles.Add(node, nodeStyle);
            }
            nodeStyle.Add(specifility, property);
        }

        #region IEnumerable<KeyValuePair<T,NodeStyle>> Members

        public IEnumerator<KeyValuePair<T, NodeStyle>> GetEnumerator()
        {
            return _nodeStyles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
