#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Membership
{
    public class ExternalLogin
    {
        Site _site;
        UrlHelper _urlHelper;
        MembershipConnectManager _membershipConnectManager;
        public ExternalLogin(Site site, UrlHelper urlHelper, MembershipConnectManager membershipConnectManager)
        {
            this._site = site;
            this._urlHelper = urlHelper;
            this._membershipConnectManager = membershipConnectManager;
        }
        public IHtmlString ExternalLoginUrl(string returnUrl)
        {
            return new HtmlString(_urlHelper.FrontUrl().WrapperUrl(_urlHelper.Action("Index", "ExternalLogin", new { Area = "Membership", returnUrl = returnUrl })).ToString());
        }
        public IEnumerable<MembershipConnect> GetMembershipConnects()
        {
            return _membershipConnectManager.All(_site.GetMembership(), "");
        }
    }


    public static class ExternalLoginExtension
    {
        public static ExternalLogin ExternalLogin(this Page_Context page_context)
        {
            MembershipConnectManager membershipConnectManager = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<MembershipConnectManager>();

            return new ExternalLogin(page_context.PageRequestContext.Site, page_context.Url, membershipConnectManager);
        }
    }
}
