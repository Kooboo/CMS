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

namespace Kooboo.CMS.Sites.Models
{
    public class CodeSnippetGroup
    {
        public string ViewEngine { get; set; }
        public string Name { get; set; }
        public CodeSnippetGroup Parent { get; set; }
        public IEnumerable<CodeSnippet> CodeSnippets { get; set; }
        public IEnumerable<CodeSnippetGroup> ChildGroups { get; set; }
    }
    public class CodeSnippet
    {
        public string ViewEngine { get; set; }
        public CodeSnippetGroup Group { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
