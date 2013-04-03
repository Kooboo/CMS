using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;
using Kooboo.Globalization;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Web.Models;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid(IdProperty = "ModuleName")]
    [GridAction(ActionName = "Relations", RouteValueProperties = "ModuleName", Order = 1, Class = "o-icon relation dialog-link", Title = "Relations")]
    [GridAction(ActionName = "Uninstall", RouteValueProperties = "ModuleName", Order = 2, Class = "o-icon delete dialog-link", Title = "Uninstall")]
    public class ModuleInfo_Metadata
    {
        [GridColumn]
        [Display(Name="Module")]
        public string ModuleName { get; set; }
        [GridColumn]
        public string Version { get; set; }
        
        public string KoobooCMSVersion { get; set; }
    }
    public class InstallModuleModel
    {
        //[Remote("IsNameAvailable", "ModuleManagement")]
        //[Required]
        //[RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        //[Display(Name="Module name")]
        //public string ModuleName { get; set; }
        [Required]
        [UIHint("File")]
        [Display(Name="Module file")]
        public string ModuleFile { get; set; }
    }
    public class ModuleListInSiteCheckVisible : IVisibleArbiter
    {
        public bool IsVisible(object dataItem, ViewContext viewContext)
        {
            var data = (ModuleListInSiteModel)dataItem;
            return !data.Included;
        }
    }
    public class ExcludeCheckVisible : IVisibleArbiter
    {
        public bool IsVisible(object dataItem, ViewContext viewContext)
        {
            var data = (ModuleListInSiteModel)dataItem;
            return data.Included;
        }
    }
    [Grid(IdProperty = "ModuleName", Checkable = true, CheckVisible = typeof(ModuleListInSiteCheckVisible))]
    [GridAction(ActionName = "Exclude", ConfirmMessage = "Are you sure you want to exclude this module from the site?", RouteValueProperties = "ModuleName", Order = 2, Class = "o-icon delete actionCommand", Title = "Exclude", CellVisibleProperty = "Included")]
    public class ModuleListInSiteModel
    {
        [Required]
        [GridColumn]
        public string ModuleName { get; set; }
        [GridColumn(ItemRenderType = typeof(BooleanColumnRender))]
        public bool Included { get; set; }
    }
}