#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Web2.Areas.Contents.Models.Grid2;
using Kooboo.CMS.Web2.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Globalization;

using Kooboo.Common.Web.Grid.Design;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web2.Areas.Contents.Models
{


    [MetadataFor(typeof(FolderIndexInfo))]
    [Grid(GridItemType = typeof(FolderIndexInfo_GridItem), Checkable = true, IdProperty = "FolderName")]
    public class FolderIndexInfo_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Folder")]
        public string FolderName { get; set; }
        [GridColumn(Order = 2, HeaderText = "Indexed contents")]
        public int IndexedContents { get; set; }
        [GridColumn(Order = 3, HeaderText = "Rebuilding", GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool Rebuilding { get; set; }
    }

    [MetadataFor(typeof(LastAction))]
    public class LastAction_Metadata
    {
        public TextFolder Folder { get; set; }
        public string ContentSummary { get; set; }
        public ContentAction Action { get; set; }
    }
    public class IndexSummaryModel
    {
        public IEnumerable<FolderIndexInfo> FolderIndexInfoes { get; set; }
        public IEnumerable<LastAction> LastActions { get; set; }
    }
}