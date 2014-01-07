#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Kooboo.CMS.Sites.Membership
{
    public class MembershipAuthentication : IMembershipAuthentication
    {
        #region .ctor
        Site _site = null;
        HttpContextBase _httpContext = null;
        Kooboo.CMS.Membership.Models.Membership _membership;

        public MembershipAuthentication(Site site, Kooboo.CMS.Membership.Models.Membership membership, HttpContextBase httpContextBase)
        {
            if (site == null)
            {
                throw new ArgumentNullException("site");
            }
            if (httpContextBase == null)
            {
                throw new ArgumentNullException("httpContextBase");
            }
            this._site = site;
            this._membership = membership;

            this._httpContext = httpContextBase;
        }
        #endregion

        #region static fields
        public static string AuthCookieName = ".{0}-MemberAUTH";
        public static string MemberPrincipalName = "{0}-MemberPrincipal";
        #endregion

        #region SetAuthCookie
        public virtual void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie);
            cookie.Name = GetCookieName(_site);

            if (!string.IsNullOrEmpty(_membership.AuthCookieDomain))
            {
                cookie.Domain = _membership.AuthCookieDomain;
            }

            HttpContext.Current.Response.SetCookie(cookie);
        }
        #endregion

        #region SignOut
        public virtual void SignOut()
        {
            var authCookie = GetAuthCookie(_site, _httpContext.Request.Cookies);
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-100);
                HttpContext.Current.Response.SetCookie(authCookie);
            }
        }
        #endregion

        #region GetMember
        public virtual IPrincipal GetMember()
        {
            var principalName = GetPrincipalName(_site);
            IPrincipal memberPrincipal = (IPrincipal)HttpContext.Current.Items[principalName];
            if (memberPrincipal == null)
            {
                memberPrincipal = DefaultPrincipal();

                var authCookie = GetAuthCookie(_site, _httpContext.Request.Cookies);
                if (authCookie != null && authCookie.Expires < DateTime.Now)
                {
                    try
                    {
                        var encryptedTicket = authCookie.Value;
                        var ticket = FormsAuthentication.Decrypt(encryptedTicket);
                        if (!ticket.Expired)
                        {
                            var membershipUser = GetMembershipUser(ticket.Name);
                            if (membershipUser != null)
                            {
                                memberPrincipal = new GenericPrincipal(new FormsIdentity(ticket), membershipUser.MembershipGroups);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }
                }

                HttpContext.Current.Items[principalName] = memberPrincipal;
            }
            return memberPrincipal;
        }
        public virtual MembershipUser GetMembershipUser()
        {
            var principalName = GetPrincipalName(_site) + "-MembershipUser";
            MembershipUser membershipUser = (MembershipUser)HttpContext.Current.Items[principalName];
            if (membershipUser == null)
            {
                var principal = GetMember();
                if (principal.Identity.IsAuthenticated && !string.IsNullOrEmpty(principal.Identity.Name))
                {
                    membershipUser = GetMembershipUser(principal.Identity.Name);
                }
                HttpContext.Current.Items[principalName] = membershipUser;
            }

            return membershipUser;
        }

        private MembershipUser GetMembershipUser(string memberUserName)
        {
            var membership = _site.GetMembership();
            if (membership != null)
            {
                return new MembershipUser() { Membership = membership, UserName = memberUserName }.AsActual();
            }
            return null;
        }

        private GenericPrincipal DefaultPrincipal()
        {
            return new GenericPrincipal(new GenericIdentity(""), new string[0]);
        }

        protected virtual HttpCookie GetAuthCookie(Site site, HttpCookieCollection cookies)
        {
            var cookieName = GetCookieName(site);
            for (int i = 0; i < cookies.Count; i++)
            {
                var cookie = cookies[i];
                if (cookie.Name == cookieName)
                {
                    return cookie;
                }
            }
            return null;
        }
        #endregion

        #region IsAuthenticated
        public virtual bool IsAuthenticated()
        {
            var principal = GetMember();
            if (principal != null && principal.Identity.IsAuthenticated == true)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GetCookieName
        private static string GetCookieName(Site site)
        {
            var membership = site.GetMembership().AsActual();
            if (!string.IsNullOrEmpty(membership.AuthCookieName))
            {
                return membership.AuthCookieName;
            }
            return string.Format(AuthCookieName, site.FullName);
        }
        private static string GetPrincipalName(Site site)
        {
            var membership = site.GetMembership().AsActual();
            if (!string.IsNullOrEmpty(membership.AuthCookieName))
            {
                return membership.AuthCookieName;
            }
            return string.Format(MemberPrincipalName, site.FullName);
        }
        #endregion

    }
}
