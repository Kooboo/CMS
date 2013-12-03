
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels
{
    [MetadataFor(typeof(Category))]
    [Grid(Checkable = true, IdProperty = "Id")]
    public class Category_Metadata
    {
        //[GridColumnAttribute()]
        public virtual int Id { get; set; }
        [Required]
        [GridColumnAttribute(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public virtual string Title { get; set; }
    }
}