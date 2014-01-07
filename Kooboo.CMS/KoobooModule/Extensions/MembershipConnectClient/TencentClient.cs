using Kooboo.CMS.Membership.OAuthClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.VSExtensionTemplate.Extensions.MembershipConnectClient
{  
    //uncomment below code to use this as an example
    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IAuthClient), Key = "tencent")]
    //public class TencentClient : OpenAuthClient, IAuthClient
    //{
    //    public override DotNetOpenAuth.AspNet.IAuthenticationClient GetOpenAuthClient()
    //    {
    //        System.Diagnostics.Contracts.Contract.Requires(MembershipConnect != null);

    //        return new TencentOAuthClient(MembershipConnect.AppId, MembershipConnect.AppSecret);
    //    }

    //    public override string ProviderName
    //    {
    //        get
    //        {
    //            return "Tencent";
    //        }
    //    }

    //    public override bool RequiresAppId
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }
    //}
}