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
using System.Web.Mvc;

namespace Kooboo.CMS.Content.Models.Binder
{
    public class RuleViolationException : Exception
    {
        public IEnumerable<RuleViolation> Issues { get; private set; }
        public object Model { get; private set; }
        public RuleViolationException(object model, IEnumerable<RuleViolation> issues)
            : this("There is one or more values invalid", model, issues)
        {
        }
        public RuleViolationException(string msg, object model, IEnumerable<RuleViolation> issues)
            : base(msg)
        {
            this.Model = model;
            this.Issues = issues;
        }
        public override string Message
        {
            get
            {
                string baseMessage = base.Message;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("Exception Message:{0} \n Issues:\n", baseMessage);
                foreach (var issue in Issues)
                {
                    stringBuilder.AppendFormat("PropertyName:{0};PropertyValue:{1};ErrorMessage:{2} \n", issue.PropertyName, issue.PropertyValue, issue.ErrorMessage);
                }
                return stringBuilder.ToString();
            }
        }

        public void FillIssues(ModelStateDictionary modelState)
        {
            if (Issues != null)
            {
                foreach (var item in Issues)
                {
                    modelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
        }
    }
}
