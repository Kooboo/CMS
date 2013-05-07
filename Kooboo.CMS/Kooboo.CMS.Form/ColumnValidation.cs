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
using System.Runtime.Serialization;

namespace Kooboo.CMS.Form
{
    public enum ValidationType
    {
        Required,
        Unique,
        StringLength,
        Range,
        Regex
    }
    [Serializable]
    [DataContract(Name = "ColumnValidation")]
    [KnownTypeAttribute(typeof(ColumnValidation))]
    public abstract class ColumnValidation
    {
        public static Func<ValidationType, string> DefaultErrorMessageAccessor = v => SR.GetString(string.Format("{0}Attribute_ValidationError", v.ToString()));

        public abstract ValidationType ValidationType { get; set; }

        private string errorMessage = null;
        [DataMember(Order = 3)]
        public string ErrorMessage
        {
            get
            {
                //if (errorMessage == null)
                //{
                //    return DefaultErrorMessageAccessor(this.ValidationType);
                //}
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }

        public T Parse<T>() where T : ColumnValidation, new()
        {
            return (T)this;
        }
    }
    [Serializable]
    [DataContract(Name = "RequiredValidation")]
    [KnownTypeAttribute(typeof(RequiredValidation))]
    public class RequiredValidation : ColumnValidation
    {
        public override ValidationType ValidationType
        {
            get { return ValidationType.Required; }
            set { }
        }
    }
    [Serializable]
    [DataContract(Name = "StringLengthValidation")]
    [KnownTypeAttribute(typeof(StringLengthValidation))]
    public class StringLengthValidation : ColumnValidation
    {
        public override ValidationType ValidationType
        {
            get { return ValidationType.StringLength; }
            set { }
        }
        [DataMember(Order = 5)]
        public int Min { get; set; }
        [DataMember(Order = 7)]
        public int Max { get; set; }
    }
    [Serializable]
    [DataContract(Name = "RangeValidation")]
    [KnownTypeAttribute(typeof(RangeValidation))]
    public class RangeValidation : ColumnValidation
    {
        public override ValidationType ValidationType
        {
            get { return ValidationType.Range; }
            set { }
        }
        [DataMember(Order = 5)]
        public decimal Start { get; set; }
        [DataMember(Order = 7)]
        public decimal End { get; set; }

    }
    [Serializable]
    [DataContract(Name = "RegexValidation")]
    [KnownTypeAttribute(typeof(RegexValidation))]
    public class RegexValidation : ColumnValidation
    {
        public override ValidationType ValidationType
        {
            get { return ValidationType.Regex; }
            set { }
        }
        [DataMember(Order = 5)]
        public string Pattern { get; set; }
    }

    [Serializable]
    [DataContract(Name = "UniqueValidation")]
    [KnownTypeAttribute(typeof(UniqueValidation))]
    public class UniqueValidation : ColumnValidation
    {
        public override ValidationType ValidationType
        {
            get { return ValidationType.Unique; }
            set { }
        }
    }

}
