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
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Content.Services;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.Web.Areas.Contents.Menus
{
    public class ReceviedMessageGroup_MenuItem : MenuItem
    {
        private class ReceviedMessageGroupMenuItemInitializer : DefaultMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
            {
                string yearMonth = controllerContext.RequestContext.GetRequestValue("yearMonth");
                return string.Compare(yearMonth, menuItem.RouteValues["yearMonth"].ToString(), true) == 0;
            }
            protected override bool GetIsVisible(MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
            {
                return Repository.Current.EnableBroadcasting;
            }
        }
        public ReceviedMessageGroup_MenuItem(DateTime yearMonth)
        {
            this.Action = "Index";
            this.Controller = "ReceivedMessage";
            this.Text = yearMonth.ToString("yyyy-MM");
            this.Visible = true;
            this.RouteValues = new System.Web.Routing.RouteValueDictionary();
            this.RouteValues.Add("yearMonth", this.Text);

            this.Initializer = new ReceviedMessageGroupMenuItemInitializer();
        }


        //protected override bool DefaultActive(ControllerContext controllContext)
        //{
        //    return false;
        //}
    }
    //public class ReceivedMessageGroupMenuItems : IMenuItemContainer
    //{
    //    #region IMenuItemContainer Members

    //    public IEnumerable<MenuItem> GetItems(System.Web.Mvc.ControllerContext controllerContext)
    //    {
    //        var repository = Repository.Current;

    //        List<MenuItem> items = new List<MenuItem>();
    //        if (repository != null)
    //        {
    //            var receivedMessageManager = ServiceFactory.ReceivedMessageManager;

    //            var groups = receivedMessageManager.AllGroups(repository).Where(it => it.Month != DateTime.Now.Month).OrderByDescending(it => it).Take(6);
    //            foreach (var group in groups)
    //            {
    //                items.Add(new ReceviedMessageGroup_MenuItem(group));
    //            }
    //        }
    //        return items;
    //    }

    //    #endregion
    //}
}