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
    partial class Combiner<T>
    {
        public class NodeStyle
        {
            private Dictionary<string, Tuple<Property, int>> _properties = new Dictionary<string, Tuple<Property, int>>();

            public void Add(int specifility, Property property)
            {
                int s;
                if (property.Important)
                {
                    s = Selector.ImportantSpecificity;
                    property.Important = false;
                }
                else
                {
                    s = specifility;
                }

                if (property.IsShorthand)
                {
                    AddShorthandProperty(s, property);
                }
                else
                {
                    AddProperty(s, property);
                }
            }

            public override string ToString()
            {
                return ToDeclaration().ToString();
            }

            public Declaration ToDeclaration()
            {
                var result = new Declaration();
                result.AddRange(Property.Combine(_properties.Select(o => o.Value.Item1)));
                return result;
            }

            private void AddProperty(int specifility, Property property)
            {
                property.Standarlize();
                string name = property.Name.ToLower();
                if (_properties.ContainsKey(name))
                {
                    if (_properties[property.Name].Item2 <= specifility)
                    {
                        _properties[property.Name] = new Tuple<Property, int>(property, specifility);
                    }
                }
                else
                {
                    _properties.Add(name, new Tuple<Property, int>(property, specifility));
                }
            }

            private void AddShorthandProperty(int specifility, Property property)
            {
                foreach (var each in property.Split())
                {
                    AddProperty(specifility, each);
                }
            }
        }
    }
}
