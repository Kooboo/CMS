#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Controllers;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Menus
{
    public class FolderMenuItem : MenuItem
    {
        private class FolderMenuItemInitializer : DefaultMenuItemInitializer
        {
            protected override bool GetIsVisible(MenuItem menuItem, ControllerContext controllerContext)
            {
                var folderName = menuItem.RouteValues["FolderName"].ToString();

                if (string.Equals(menuItem.Controller, "MediaContent", StringComparison.OrdinalIgnoreCase))
                {
                    return base.GetIsVisible(menuItem, controllerContext);
                }
                var textFolder = new TextFolder(Repository.Current, folderName).AsActual();

                if (!textFolder.Visible)
                {
                    return false;
                }

                var allowedView = Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager
                    .AvailableViewContent(textFolder, controllerContext.HttpContext.User.Identity.Name);



                return allowedView &&
                    base.GetIsVisible(menuItem, controllerContext);

            }
            protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
            {
                var baseActive = base.GetIsActive(menuItem, controllerContext);
                if (baseActive)
                {
                    string repositoryName = controllerContext.RequestContext.GetRequestValue("repositoryName");
                    string folder = controllerContext.RequestContext.GetRequestValue("FolderName");
                    return string.Compare(repositoryName, menuItem.RouteValues["repositoryName"].ToString(), true) == 0
                        && string.Compare(folder, menuItem.RouteValues["FolderName"].ToString()) == 0;
                }
                else
                {
                    return baseActive;
                }

            }
        }
        public FolderMenuItem(Folder folder)
        {
            base.Visible = true;
            base.Text = folder.Name;

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            HtmlAttributes = new System.Web.Routing.RouteValueDictionary();

            RouteValues["repositoryName"] = folder.Repository.Name;
            RouteValues["FolderName"] = folder.FullName;

            this.Area = "Contents";
            base.Text = folder.FriendlyText;
            var cssClass = "";
            if (folder is TextFolder)
            {
                if (string.IsNullOrEmpty(((TextFolder)folder).SchemaName))
                {
                    base.Controller = "TextFolder";
                    base.Action = "Index";
                }
                else
                {
                    base.Controller = "TextContent";
                    base.Action = "index";
                }
                cssClass = "TextFolder";
            }
            else if (folder is MediaFolder)
            {
                base.Controller = "MediaContent";
                base.Action = "index";

                cssClass = "TextFolder";
            }
            //Set folder class
            HtmlAttributes["Class"] = cssClass + " " + string.Join("-", folder.NamePaths.ToArray());

            this.Initializer = new FolderMenuItemInitializer();
        }
        //protected override bool DefaultActive(ControllerContext controllContext)
        //{
        //    return false;
        //}
        public override bool Localizable
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
    }
}