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

namespace Kooboo.CMS.Content.Models.Binder
{
    public class RuleViolation
    {
        public string PropertyName { get; private set; }
        public object PropertyValue { get; private set; }
        public string ErrorMessage { get; private set; }
        public RuleViolation(string propertyName, string propertyValue, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
            this.ErrorMessage = errorMessage;
        }
    }
}
