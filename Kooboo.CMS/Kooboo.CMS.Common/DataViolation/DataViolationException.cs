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

namespace Kooboo.CMS.Common.DataViolation
{
    public class DataViolationException : Exception
    {
        public IEnumerable<DataViolationItem> Violations { get; private set; }
        public DataViolationException(IEnumerable<DataViolationItem> violations)
            : this("", violations)
        {
        }
        public DataViolationException(string msg, IEnumerable<DataViolationItem> violations)
            : base(msg)
        {
            this.Violations = violations;
        }
        public override string Message
        {
            get
            {
                string baseMessage = base.Message;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("Exception Message:{0} \n Issues:\n", baseMessage);
                foreach (var issue in Violations)
                {
                    stringBuilder.AppendFormat("PropertyName:{0};PropertyValue:{1};ErrorMessage:{2} \n", issue.PropertyName, issue.PropertyValue, issue.ErrorMessage);
                }
                return stringBuilder.ToString();
            }
        }
    }
}
