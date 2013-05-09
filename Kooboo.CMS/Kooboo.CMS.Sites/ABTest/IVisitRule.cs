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
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.ABTest
{
    public interface IVisitRule
    {
        string Name { get; set; }

        string TemplateVirtualPath { get; }

        string RuleType { get; }

        string DisplayText { get; }

        bool IsMatch(System.Web.HttpRequestBase httpRequest);
    }
}
