using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Kooboo.Web.Css.Meta;

namespace Kooboo.Web.Css
{
    public class Property
    {
        public const char NameValueSeparator = ':';
        public const string ImportantToken = "!important";
        public const string InitialRegexPattern = @"^initial(\sinitial)*$";

        public Property(string name, string value)
        {
            Name = name.Trim().ToLower();
            Value = value.Trim();
            Important = false;
            IsInitial = Regex.IsMatch(Value, InitialRegexPattern);
            IsBrowserGenerated = Name.StartsWith("-");
        }

        public bool IsBrowserGenerated { get; set; }

        public bool IsInitial { get; set; }

        public string Name { get; private set; }

        public virtual string Value { get; protected set; }

        public bool Important { get; set; }

        public bool IsShorthand 
        {
            get
            {
                if (Meta == null)
                    return false;

                return Meta.ValueType is ShorthandType;
            }
        }

        internal PropertyMeta Meta
        {
            get
            {
                return PropertyMeta.GetMeta(Name);
            }
        }

        public override string ToString()
        {
            return Name + ":" + Value ?? String.Empty + (Important ? " " + ImportantToken : String.Empty);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Property;
            if (other == null)
                return false;

            return String.Compare(other.Name, Name, true) == 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public IEnumerable<Property> Split(SplitOptions options = SplitOptions.Recursive)
        {
            if (!IsShorthand)
                throw new InvalidOperationException("Only short hand property can be splitted.");

            if (options == SplitOptions.NonRecursive)
            {
                return NonRecursiveSplit();
            }
            else
            {
                return RecursiveSplit();
            }
        }

        public void Standarlize()
        {
            if (Meta != null)
            {
                Value = Meta.ValueType.Standardlize(Value);
            }
        }

        private IEnumerable<Property> RecursiveSplit()
        {
            List<Property> result = new List<Property>();
            result.Add(this);

            for (int i = 0; i < result.Count; )
            {
                if (result[i].IsShorthand)
                {
                    result.AddRange(result[i].NonRecursiveSplit());
                    result.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return result;
        }

        private IEnumerable<Property> NonRecursiveSplit()
        {
            return (Meta.ValueType as ShorthandType).ShorthandRule.Split(this, Meta);
        }

        public static IEnumerable<Property> Combine(IEnumerable<Property> properties)
        {
            List<Property> result = new List<Property>();
            var uncombinable = properties.Where(o => o.Meta == null || String.IsNullOrEmpty(o.Meta.ShorthandName));
            var combinable = properties.Except(uncombinable);

            // Add uncombinable part
            result.AddRange(uncombinable);

            // Combine combinable part
            foreach (var each in combinable.GroupBy(o => o.Meta.ShorthandName))
            {
                var meta = PropertyMeta.GetMeta(each.Key);
                Property shorthand;
                if ((meta.ValueType as ShorthandType).ShorthandRule.TryCombine(each, meta, out shorthand))
                {
                    result.Add(shorthand);
                }
                else
                {
                    result.AddRange(each);
                }
            }

            return result;
        }

        public static Property Parse(string str)
        {
            string s = str.Trim().TrimEnd(';').Trim();

            int index = s.IndexOf(NameValueSeparator);
            if (index <= 0)
                throw new InvalidStructureException(String.Format("Property must contains name and value part, exception str: {0}", str));

            string name = s.Substring(0, index).Trim();
            if (name.Length == 0)
                throw new InvalidStructureException(String.Format("Property name could not be null, exception str {0}", str));

            if (index == s.Length - 1)
                return new Property(name, null);

            string value = s.Substring(index + 1).Trim();

            bool important = value.EndsWith(ImportantToken, StringComparison.OrdinalIgnoreCase);
            if (important)
            {
                value = value.Substring(0, value.Length - ImportantToken.Length);
                return new Property(name, value) { Important = true };
            }
            else
            {
                return new Property(name, value);
            }
        }

        public static bool TryParse(string str, out Property property)
        {
            try
            {
                property = Parse(str);
                return true;
            }
            catch
            {
                property = null;
                return false;
            }
        }
    }
}
