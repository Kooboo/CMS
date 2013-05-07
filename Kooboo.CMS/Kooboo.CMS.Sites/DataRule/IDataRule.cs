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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;
namespace Kooboo.CMS.Sites.DataRule
{
    public interface IDataRule
    {
        string FolderName { get; set; }

        DataRuleType DataRuleType { get; }

        IContentQuery<TextContent> Execute(DataRuleContext dataRuleContext);

        Schema GetSchema(Repository repository);

        bool HasAnyParameters();

        bool IsValid(Repository repository);

        bool? EnablePaging { get; set; }
    }
}
