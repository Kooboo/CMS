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
using System.Collections;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.DataRule.CodeSnippet
{
    public class DataRuleCodeSnippetFactory
    {
        static Hashtable hashtable = new Hashtable();
        static DataRuleCodeSnippetFactory()
        {
            Register(TakeOperation.List, new ListCodeSnippet());
            Register(TakeOperation.First, new DetailCodeSnippet());
        }
        public static void Register(TakeOperation operation, IDataRuleCodeSnippet codeSnippet)
        {
            lock (hashtable)
            {
                hashtable[operation] = codeSnippet;
            }
        }

        public static string GenerateCodeSnippet(Repository repository, DataRuleSetting dataRule)
        {
            var codeSnippet = (IDataRuleCodeSnippet)hashtable[dataRule.TakeOperation];
            if (codeSnippet == null)
            {
                return "";
            }
            return codeSnippet.Generate(repository, dataRule);
        }
    }
}
