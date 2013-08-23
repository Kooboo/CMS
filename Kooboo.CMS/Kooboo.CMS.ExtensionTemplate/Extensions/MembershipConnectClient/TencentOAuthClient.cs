using DotNetOpenAuth.AspNet.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kooboo.Web.Url;
namespace Kooboo.CMS.ExtensionTemplate.Extensions.MembershipConnectClient
{
    public sealed class TencentOAuthClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";
        private const string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";
        private const string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";
        private const string UserInfoEndpoint = "https://graph.qq.com/user/get_user_info";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Tencent";

        public TencentOAuthClient(string appId, string appSecret)
            : base(providerName)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var dict = new Dictionary<string, string>();
            dict["client_id"] = this.appId;
            dict["redirect_uri"] = returnUrl.AbsoluteUri;
            dict["response_type"] = "code";
            var url = dict.Aggregate(AuthorizationEndpoint, (current, param) => current.AddQueryParam(param.Key, param.Value));
            return new Uri(url);
        }

        private string GetOpenid(string accessToken)
        {
            var requestUri = UrlUtility.CombineQueryString(OpenIdEndpoint, accessToken);
            var text = WebRequestExtensions.Get(requestUri);
            var result = string.Empty;
            var regex = new Regex("\"openid\":\"(.*?)\"");
            if (regex.IsMatch(text))
            {
                result = regex.Match(text).Groups[1].Value;
            }
            return result;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var openid = GetOpenid(accessToken);
            var url = (UserInfoEndpoint + "?" + accessToken)
                .AddQueryParam("openid", openid)
                .AddQueryParam("oauth_consumer_key", this.appId)
                .AddQueryParam("format", "json");

            var text = WebRequestExtensions.Get(url);
            var graphData = JsonHelper.Deserialize<TencentGraphData>(text);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", openid);
            dictionary.AddItemIfNotEmpty("username", graphData.NickName);
            dictionary.AddItemIfNotEmpty("name", graphData.NickName);
            //dictionary.AddItemIfNotEmpty("msg", graphData.Msg);
            //dictionary.AddItemIfNotEmpty("gender", graphData.Gender);
            //dictionary.AddItemIfNotEmpty("figureurl", graphData.Figureurl);
            //dictionary.AddItemIfNotEmpty("figureurl_1", graphData.Figureurl_1);
            //dictionary.AddItemIfNotEmpty("figureurl_2", graphData.Figureurl_2);
            //dictionary.AddItemIfNotEmpty("figureurl_qq_1", graphData.Figureurl_qq_1);
            //dictionary.AddItemIfNotEmpty("figureurl_qq_2", graphData.Figureurl_qq_2);
            //dictionary.AddItemIfNotEmpty("is_yellow_vip", graphData.Is_yellow_vip.ToString());
            //dictionary.AddItemIfNotEmpty("vip", graphData.Vip.ToString());
            //dictionary.AddItemIfNotEmpty("level", graphData.Level.ToString());
            return dictionary;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var dict = new Dictionary<string, string>();
            dict["code"] = authorizationCode;
            dict["client_id"] = this.appId;
            dict["client_secret"] = this.appSecret;
            dict["redirect_uri"] = returnUrl.AbsoluteUri.UrlEncode();
            dict["grant_type"] = "authorization_code";
            var json = WebRequestExtensions.Post(TokenEndpoint, dict);
            if (!string.IsNullOrEmpty(json))
            {
                return json;
            }

            throw new HttpException(500, "Get AccessToken failed!");
        }
    }
}