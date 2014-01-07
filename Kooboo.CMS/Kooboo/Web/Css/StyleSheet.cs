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
using System.Text.RegularExpressions;

namespace Kooboo.Web.Css
{
    public class StyleSheet
    {
        private List<Node> _nodes;
        public IList<Node> Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new List<Node>();
                }
                return _nodes;
            }
        }

        public IEnumerable<Statement> Statements
        {
            get
            {
                return Nodes.OfType<Statement>();
            }
        }

        public static StyleSheet Load(string str, bool removeComment = true)
        {
            var s = str;
            if (removeComment)
            {
                s = Regex.Replace(s, @"(/\*[\w\W]*?\*/)|(<--)|(-->)", String.Empty);
            }
            return Load(new StyleSheetReader(s));
        }

        public static StyleSheet Load(StyleSheetReader reader)
        {
            StyleSheet result = new StyleSheet();

            while (!reader.EndOfStream)
            {
                switch (reader.Status)
                {
                    case StyleSheetReader.ReadStatus.AtRule:
                        result.Nodes.Add(AtRule.Parse(reader.ReadAtRule()));
                        break;
                    case StyleSheetReader.ReadStatus.RuleSet:
                        result.Nodes.Add(RuleSet.Parse(reader.ReadRuleSet()));
                        break;
                    case StyleSheetReader.ReadStatus.Comment:
                        result.Nodes.Add(Comment.Parse(reader.ReadComment()));
                        break;
                    default:
                        break;
                }                
            }

            return result;
        }

        public static StyleSheet Load(TextReader reader)
        {
            return Load(new StyleSheetReader(reader));
        }

        public static StyleSheet Load(Stream stream)
        {
            return Load(new StyleSheetReader(stream));
        }
    }
}
