#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public class ValueDiscriminationRule : ShorthandRule
    {
        private bool _groupable = false;
        private List<SubList> _list = new List<SubList>();

        public ValueDiscriminationRule(string grammar)
        {
            _list = CreateList(grammar);
            _groupable = _list.Any(o => o.Type == SubListType.Sequencial);
        }

        public override IEnumerable<string> SubProperties(PropertyMeta meta)
        {
            return _list.SelectMany(o => o);
        }

        protected IList<SubList> Grammar
        {
            get
            {
                return _list;
            }
        }

        protected override IEnumerable<Property> Split(List<string> splitted, PropertyMeta meta)
        {
            return Split(splitted, meta, CreateMatchList(_list));
        }

        protected virtual List<SubList> CreateMatchList(List<SubList> list)
        {
            List<SubList> matchList = new List<SubList>();
            foreach (var each in list)
            {
                if (each.Type == SubListType.Alternative)
                {
                    matchList.Add(new SubList(each, each.Type));
                }
                else
                {
                    matchList.Add(each);
                }
            }
            return matchList;
        }

        protected virtual IEnumerable<Property> Split(List<string> splitted, PropertyMeta meta, List<SubList> matchList)
        {
            foreach (var value in splitted)
            {
                if (_groupable && value.Contains('/'))
                {
                    // In case like "font-size/line-height"
                    string[] subValues = value.Split('/');
                    for (int i = 0; i < matchList.Count; i++)
                    {
                        var propertyNames = matchList[i];

                        if (matchList[i].Type != SubListType.Sequencial || matchList[i].Count() < subValues.Length)
                            continue;

                        var subMeta = PropertyMeta.GetMeta(propertyNames.First());
                        if (subMeta != null && subMeta.ValueType.IsValid(subValues[0]))
                        {
                            var propertyName = propertyNames.GetEnumerator();
                            var subValue = subValues.GetEnumerator();
                            while (subValue.MoveNext() && propertyName.MoveNext())
                            {
                                yield return new Property(propertyName.Current, subValue.Current.ToString());
                            }

                            matchList.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < matchList.Count; i++)
                    {
                        var propertyNames = matchList[i];

                        if (propertyNames.Type == SubListType.Alternative)
                        {
                            Property property = null;
                            for (int j = 0; j < propertyNames.Count; j++)
                            {
                                var subMeta = PropertyMeta.GetMeta(propertyNames[j]);
                                if (subMeta != null && subMeta.ValueType.IsValid(value))
                                {
                                    property = new Property(propertyNames[j], value);;
                                    propertyNames.RemoveAt(j);
                                    break;
                                }
                            }
                            if (property != null)
                            {
                                yield return property;
                                break;
                            }
                        }
                        else
                        {
                            var subMeta = PropertyMeta.GetMeta(propertyNames.First());
                            if (subMeta != null && subMeta.ValueType.IsValid(value))
                            {
                                yield return new Property(propertyNames.First(), value);

                                matchList.RemoveAt(i);
                                break;
                            }    
                        }
                    }
                }
            }
        }

        public override bool TryCombine(IEnumerable<Property> properties, PropertyMeta meta, out Property property)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var propertyNames in _list)
            {
                if ((propertyNames.Type == SubListType.Sequencial || propertyNames.Type == SubListType.MustContains)
                    && !properties.Any(o => o.Name == propertyNames.First()))
                {
                    property = null;
                    return false;
                }

                var propertyName = propertyNames.GetEnumerator();
                int i = 0;
                while (propertyName.MoveNext())
                {
                    var p = properties.FirstOrDefault(o => o.Name == propertyName.Current);
                    if (p == null)
                        continue;

                    if (i == 0)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(' ');
                        }
                    }
                    else if (propertyNames.Type == SubListType.Sequencial)
                    {
                        builder.Append('/');
                    }
                    else
                    {
                        builder.Append(' ');
                    }

                    builder.Append(p.Value);

                    i++;
                }
            }

            property = new Property(meta.Name, builder.ToString());
            return true;
        }

        protected List<SubList> CreateList(string grammar)
        {
            List<SubList> list = new List<SubList>();

            string[] splitted1 = grammar.Split(new char[] { ']' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var each1 in splitted1)
            {
                if (each1.Contains('['))
                {
                    list.Add(new SubList(each1.TrimStart('[').Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SubListType.Alternative));
                }
                else
                {
                    foreach (var each2 in each1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (each2.Contains('/'))
                        {
                            list.Add(new SubList(each2.TrimStart(' ').Split('/'), SubListType.Sequencial));
                        }
                        else
                        {
                            list.Add(new SubList(each2));
                        }
                    }
                }
            }

            return list;
        }

        protected class SubList : List<string>
        {
            public SubList(IEnumerable<string> list, SubListType type)
            {
                AddRange(list);
                Type = type;
            }

            public SubList(string value)
                : this(new string[] { value }, SubListType.MustContains)
            {
            }

            public SubListType Type { get; private set; }
        }

        protected enum SubListType
        {
            Alternative,
            Sequencial,
            MustContains
        }
    }
}
