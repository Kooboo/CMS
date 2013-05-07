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
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.ModuleArea.Areas.SampleModule.Models;
namespace Kooboo.CMS.ModuleArea.Models
{
    [Grid(Checkable = true, IdProperty = "Id")]
    public class News : Kooboo.CMS.Common.Persistence.Relational.IEntity
    {
        [GridColumnAttribute()]
        public int Id { get; set; }
        [Required]
        [GridColumnAttribute(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Title { get; set; }
        [DataType("Tinymce")]
        public string Body { get; set; }
    }
}