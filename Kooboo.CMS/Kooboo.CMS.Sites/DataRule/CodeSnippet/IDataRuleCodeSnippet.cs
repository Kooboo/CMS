using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.DataRule.CodeSnippet
{
    public interface IDataRuleCodeSnippet
    {
        string Generate(Repository repository, DataRuleSetting dataRule);
    }
}
