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

namespace Kooboo.Web.Css.Meta
{
    public class PositionDescriminationRule : ShorthandRule
    {
        private Dictionary<int, string[]> _mappings = new Dictionary<int, string[]>();
        private string[] _subProperties = null;

        public PositionDescriminationRule(params string[] grammars)
        {
            foreach (var each in grammars)
            {
                AddMapping(each);
            }

            _subProperties = _mappings.OrderByDescending(o => o.Key).First().Value;
        }

        public override IEnumerable<string> SubProperties(PropertyMeta meta)
        {
            return _subProperties.Select(o => CombineName(meta, o));
        }

        public void AddMapping(string grammar)
        {
            string[] splitted = grammar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            _mappings.Add(splitted.Length, splitted);
        }

        public override bool TryCombine(IEnumerable<Property> properties, PropertyMeta meta, out Property property)
        {
            if (SubProperties(meta).Except(properties.Select(o => o.Name)).Count() > 0)
            {
                property = null;
                return false;
            }

            string value = String.Join(" ", Filter(properties, meta).OrderBy(o => o.Item2).Select(o => o.Item1.Value));
            property = new Property(meta.Name, value);
            return true;
        }

        protected override IEnumerable<Property> Split(List<string> splitted, PropertyMeta meta)
        {
            for (int i = 0; i < splitted.Count; i++)
            {
                if (_mappings.ContainsKey(splitted.Count))
                {
                    foreach (var each in _mappings[splitted.Count][i].Split(','))
                    {
                        yield return new Property(CombineName(meta, each), splitted[i]);
                    }
                }
            }        
        }

        protected virtual string CombineName(PropertyMeta meta, string name)
        {
            return meta.Name + "-" + name;
        }

        private IEnumerable<Tuple<Property, int>> Filter(IEnumerable<Property> properties, PropertyMeta meta)
        {
            string[] highestMapping = _mappings.OrderByDescending(o => o.Key).First().Value;

            foreach (var each in properties)
            {
                string name = each.Name.Substring(meta.Name.Length + 1);
                int i = 0;
                for (; i < highestMapping.Length && highestMapping[i] != name; i++) ;
                if (i < highestMapping.Length)
                {
                    yield return new Tuple<Property, int>(each, i);
                }
            }
        }

    }
}
