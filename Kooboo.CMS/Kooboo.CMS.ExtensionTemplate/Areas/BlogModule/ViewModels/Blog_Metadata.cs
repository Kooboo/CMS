
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels.DataSources;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels
{
    [Bind(Exclude = "Categories")]
    [MetadataFor(typeof(Blog))]
    [Grid(Checkable = true, IdProperty = "Id")]
    public class Blog_Metadata
    {
        //[GridColumnAttribute()]
        public virtual int Id { get; set; }
        [Required]
        [GridColumnAttribute(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public virtual string Title { get; set; }
        [DataType("Tinymce")]
        [System.Web.Mvc.AllowHtml]
        public virtual string Description { get; set; }

        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(CategoriesDataSource))]
        public virtual ICollection<Category> Categories { get; set; }
    }
}