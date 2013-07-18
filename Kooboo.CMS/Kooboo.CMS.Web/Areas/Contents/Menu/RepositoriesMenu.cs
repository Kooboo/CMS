﻿#region License
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
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Menu
{
    public class RepositoryMenuForSelect : Kooboo.Web.Mvc.Menu.Menu
	{
		public int Level { get; set; }
	}

	public class RepositoryMenuItem : MenuItem
	{
		private class RepositoryMenuItemInitializer : DefaultMenuItemInitializer
		{
			protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
			{
				string repositoryName = controllerContext.RequestContext.GetRequestValue("repositoryName");

				return string.Compare(repositoryName, menuItem.RouteValues["repositoryName"].ToString(), true) == 0;
			}
		}
		public RepositoryMenuItem(Repository repository)
		{
			base.Action = "index";
			base.Controller = "Home";
			base.Text = repository.Name;
			if (!string.IsNullOrEmpty(repository.DisplayName))
			{
				base.Text = repository.DisplayName;
			}

			base.Visible = true;
			RouteValues = new System.Web.Routing.RouteValueDictionary();
			RouteValues["repositoryName"] = repository.Name;

			this.Initializer = new RepositoryMenuItemInitializer();
		}

		//protected override bool DefaultActive(ControllerContext controllContext)
		//{
		//    return false;
		//}
	}
	public static class RepositoriesMenu
	{
        public static Kooboo.Web.Mvc.Menu.Menu BuildMenu(ControllerContext controllerContext)
		{
            Kooboo.Web.Mvc.Menu.Menu menu = new Kooboo.Web.Mvc.Menu.Menu();
			var repositories = ServiceFactory.RepositoryManager.All();
			List<MenuItem> items = new List<MenuItem>();
			menu.Items = items;
			foreach (var site in repositories)
			{
				items.Add(CreateRepositoryMenuItem(site));
			}

			menu.Initialize(controllerContext);
			return menu;
		}


		private static MenuItem CreateRepositoryMenuItem(Repository repository)
		{
			MenuItem menuItem = new RepositoryMenuItem(repository.AsActual());
			return menuItem;
		}
	}
}