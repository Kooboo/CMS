#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Models
{
    public class Json_netModelBinder : EntityModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsComplexType || !IsJSON_netRequest(controllerContext))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            // Get the JSON data that's been posted
            var request = controllerContext.HttpContext.Request;
            request.InputStream.Position = 0;
            var jsonStringData = new StreamReader(request.InputStream).ReadToEnd();


            return JsonConvert.DeserializeObject(jsonStringData, bindingContext.ModelMetadata.ModelType, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

        }

        private static bool IsJSON_netRequest(ControllerContext controllerContext)
        {
            var contentType = controllerContext.HttpContext.Request.ContentType;
            return contentType.Contains("application/json_net");
        }
    }
}