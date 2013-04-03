using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    partial class PropertyMeta
    {
        private static PropertyMetaCollection _metas = new PropertyMetaCollection();

        public static PropertyMeta GetMeta(string name)
        {
            if (_metas.ContainsKey(name))
            {
                return _metas[name];
            }
            else
            {
                return null;
            }
        }

        static PropertyMeta()
        {
            /*
             * Font
             * 
             */
            _metas.Add(new PropertyMeta(
                "font-style", new EnumType("normal | italic | oblique")));

            _metas.Add(new PropertyMeta(
                "font-variant", new EnumType("normal | small-caps")));

            _metas.Add(new PropertyMeta(
                "font-weight", new EnumType("normal | bold | bolder | lighter | 100 | 200 | 300 | 400 | 500 | 600 | 700 | 800 | 900")));

            _metas.Add(new PropertyMeta(
                "font-size", PropertyValueType.Size));

            _metas.Add(new PropertyMeta(
                "line-height", PropertyValueType.Size));

            _metas.Add(new PropertyMeta(
                "font-family", new FontFamilyType()));

            _metas.Add(new PropertyMeta(
                "font", new ShorthandType(new FontShorthandRule())));

            /*
             * Border
             * 
             */

            foreach (var position in BorderPositionShorthandRule.Positions)
            {
                foreach (var property in BorderPositionShorthandRule.BorderProperties)
                {
                    _metas.Add(new PropertyMeta(
                        "border-" + position + "-" + property.Key, property.Value));
                }
            }

            foreach (var property in BorderPositionShorthandRule.BorderProperties)
            {
                _metas.Add(new PropertyMeta(
                    "border-" + property.Key, new ShorthandType(ShorthandRule.BorderProperty, property.Value)));
            }

            foreach (var position in BorderPositionShorthandRule.Positions)
            {
                string grammar = "[" + String.Join(" ", BorderPositionShorthandRule.BorderProperties.Select(o => "border-" + position + "-" + o.Key)) + "]";
                _metas.Add(new PropertyMeta(
                    "border-" + position, new ShorthandType(new ValueDiscriminationRule(grammar))));
            }

            _metas.Add(new PropertyMeta(
                "border", new ShorthandType(new ValueDiscriminationRule(
                    String.Join(" ", BorderPositionShorthandRule.BorderProperties.Select(o => "border-" + o.Key))))));


            /*
             * List style
             * 
             */

            _metas.Add(new PropertyMeta(
                "list-style-type", new EnumType("disc | circle | square | decimal | decimal-leading-zero | lower-roman | upper-roman | lower-greek | lower-latin | upper-latin | armenian | georgian | lower-alpha | upper-alpha | none")));

            _metas.Add(new PropertyMeta(
                "list-style-image", PropertyValueType.Url));

            _metas.Add(new PropertyMeta(
                "list-style-position", new EnumType("inside | outside")));

            _metas.Add(new PropertyMeta(
                "list-style", new ShorthandType(new ValueDiscriminationRule("[list-style-type list-style-image list-style-position]"))));

            /*
             * Background
             * 
             */

            _metas.Add(new PropertyMeta(
                "background-color", PropertyValueType.Color));

            _metas.Add(new PropertyMeta(
                "background-image", PropertyValueType.Url));

            _metas.Add(new PropertyMeta(
                "background-repeat", new EnumType("repeat | repeat-x | repeat-y | no-repeat")));

            _metas.Add(new PropertyMeta(
                "background-attachment", new EnumType("scroll | fixed")));

            _metas.Add(new PropertyMeta(
                "background-position", PropertyValueType.Any));

            _metas.Add(new PropertyMeta(
                "background-position-left", new CombinedType(new EnumType("left | center | right"), PropertyValueType.Length)));

            _metas.Add(new PropertyMeta(
                "background-position-top", new CombinedType(new EnumType("top | center | bottom"), PropertyValueType.Length)));

            _metas.Add(new PropertyMeta(
                "background", new ShorthandType(new BackgroundShorthandRule())));

            /*
             * Margin
             * 
             */

            foreach (var each in PositionShorthandRule.Positions)
            {
                _metas.Add(new PropertyMeta(
                    "margin-" + each, PropertyValueType.Length));
            }

            _metas.Add(new PropertyMeta(
                "margin", new ShorthandType(PositionDescriminationRule.Margin)));

            /*
             * Padding
             * 
             */

            foreach (var each in PositionShorthandRule.Positions)
            {
                _metas.Add(new PropertyMeta(
                    "padding-" + each, PropertyValueType.Length));
            }


            _metas.Add(new PropertyMeta(
                "padding", new ShorthandType(PositionDescriminationRule.Margin)));
        }
    }
}
