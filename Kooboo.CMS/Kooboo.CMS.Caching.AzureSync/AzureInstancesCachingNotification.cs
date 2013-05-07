#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region Usings
using Kooboo.Web.Url;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
#endregion

namespace Kooboo.CMS.Caching.AzureSync
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureInstancesCachingNotification : INotifyCacheExpired
    {
        #region Methods
        public void Notify(string objectCacheName, string key)
        {
            List<string> cacheInstances = new List<string>();
            foreach (var appRole in RoleEnvironment.Roles)
            {
                var instances = appRole.Value.Instances;
                foreach (var instance in instances)
                {
                    if (instance.Id != RoleEnvironment.CurrentRoleInstance.Id)
                    {
                        foreach (RoleInstanceEndpoint instanceEndpoint in instance.InstanceEndpoints.Values)
                        {
                            if (instanceEndpoint.Protocol == "http")
                            {
                                cacheInstances.Add("http://" + instanceEndpoint.IPEndpoint.Address.ToString() + "/sites/cache/clear");
                            }
                        }
                    }
                }
            }
            ProcessNotification(cacheInstances.ToArray(), objectCacheName, key);
        }

        private void ProcessNotification(string[] urls, string objectCacheName, string key)
        {
            var thread = new Thread(() =>
            {
                foreach (var item in urls)
                {
                    try
                    {
                        SendRequest(item, objectCacheName, key);
                    }
                    catch (Exception e)
                    {
                        HealthMonitoring.Log.LogException(e);
                    }

                }

            });
            thread.Start();
        }
        private void SendRequest(string url, string objectCacheName, string key)
        {
            NameValueCollection queryString = new NameValueCollection();
            queryString["ObjectCacheName"] = objectCacheName;
            queryString["CacheKey"] = key;
            queryString["_ts"] = DateTime.Now.Ticks.ToString();
            queryString["source"] = AppDomain.CurrentDomain.FriendlyName;
            url = CombineQueryString(url, queryString);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
        }
        private static string CombineQueryString(string url, NameValueCollection queryString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in queryString.AllKeys)
            {
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(queryString[key]));
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return UrlUtility.CombineQueryString(url, sb.ToString());
        }
        #endregion
    }
}
