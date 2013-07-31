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
    public class LengthType : PropertyValueType
    {
        public const string Zero = "0";
        public const string Units = "em | pt | px | ex | in | cm | mm | pc";

        public override string DefaultValue
        {
            get { return Zero; }
        }

        public override bool IsValid(string value)
        {
            return IsUnit(value) || IsPercentage(value) || IsZero(value);
        }

        private bool IsUnit(string value)
        {
            if (value.Length <= 2)
                return false;

            var unit = value.Substring(value.Length - 2);
            if (!(" " + Units + " ").Contains(" " + unit + " "))
                return false;

            decimal dec;
            return Decimal.TryParse(value.Substring(0, value.Length - 2), out dec);
        }

        private bool IsPercentage(string value)
        {
            decimal dec;
            return value.EndsWith("%") && Decimal.TryParse(value.Substring(0, value.Length - 2), out dec);
        }

        private bool IsZero(string value)
        {
            return value == Zero;
        }

    }
}
