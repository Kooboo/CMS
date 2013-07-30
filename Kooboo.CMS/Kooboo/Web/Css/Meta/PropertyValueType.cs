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
    public abstract class PropertyValueType
    {
        public static readonly PropertyValueType Any = new AnyType();

        public static readonly LengthType Length = new LengthType();

        public static readonly SizeType Size = new SizeType();

        public static readonly UrlType Url = new UrlType();

        public static readonly ColorType Color = new ColorType();

        public abstract string DefaultValue
        {
            get;
        }

        public abstract bool IsValid(string value);

        public virtual string Standardlize(string value)
        {
            return value;
        }
    }

    public class AnyType : PropertyValueType
    {
        public override string  DefaultValue
        {
	        get { return String.Empty; }
        }

        public override bool  IsValid(string value)
        {
 	        return true;
        }
    }
}
