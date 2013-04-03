using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public abstract class ShorthandRule
    {
        public static readonly PositionShorthandRule Margin = new PositionShorthandRule();
        public static readonly BorderPositionShorthandRule BorderProperty = new BorderPositionShorthandRule();

        public abstract IEnumerable<string> SubProperties(PropertyMeta meta);
        
        public IEnumerable<Property> Split(Property property, PropertyMeta meta)
        {
            if (String.IsNullOrEmpty(property.Value))
                return Enumerable.Empty<Property>();

            string value = property.Value;
            List<string> splitted = new List<string>();
            int p = 0;
            int propertyStart = 0;
            ReadStatus status = ReadStatus.Separator;
            while (p < value.Length)
            {
                char ch = value[p];
                if (Char.IsWhiteSpace(ch))
                {
                    if (status == ReadStatus.Property)
                    {
                        splitted.Add(value.Substring(propertyStart, p - propertyStart));
                        status = ReadStatus.Separator;
                    }
                }
                else if (ch == '"' || ch == '\'')
                {
                    if (status == ReadStatus.Quote)
                    {
                        status = ReadStatus.Property;
                    }
                    else
                    {
                        if (status == ReadStatus.Separator)
                        {
                            propertyStart = p;
                        }
                        status = ReadStatus.Quote;
                    }
                }
                else if (ch == ',')
                {
                    status = ReadStatus.Comma;
                }
                else 
                {
                    if (status == ReadStatus.Separator)
                    {
                        propertyStart = p;
                        status = ReadStatus.Property;
                    }
                    else if(status == ReadStatus.Comma)
                    {
                        status = ReadStatus.Property;
                    }
                }
                p++;
            }

            if (status == ReadStatus.Property)
            {
                splitted.Add(value.Substring(propertyStart, p - propertyStart));
            }

            return Split(splitted, meta);
        }

        protected abstract IEnumerable<Property> Split(List<string> splitted, PropertyMeta meta);

        public abstract bool TryCombine(IEnumerable<Property> properties, PropertyMeta meta, out Property property);

        enum ReadStatus
        {
            Separator,
            Quote,
            Comma,
            Property
        }
    }
}
