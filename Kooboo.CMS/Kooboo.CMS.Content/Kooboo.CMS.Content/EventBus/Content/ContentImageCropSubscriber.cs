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
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.Extensions;
using System.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;

namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentImageCropSubscriber : ISubscriber
    {
        public EventResult Receive(IEventContext context)
        {
            EventResult eventResult = new EventResult();
            if (context is ContentEventContext && HttpContext.Current != null)
            {
                try
                {
                    ContentEventContext contentEventContext = (ContentEventContext)context;
                    var content = contentEventContext.Content;

                    var result = (contentEventContext.ContentAction & ContentAction.PreAdd) == ContentAction.PreAdd || (contentEventContext.ContentAction & ContentAction.PreUpdate) == ContentAction.PreUpdate;

                    if (result)
                    {
                        var cropField = HttpContext.Current.Request.Form["Kooboo-Image-Crop-Field"];
                        var toRemove = new List<string>();
                        if (!string.IsNullOrEmpty(cropField))
                        {
                            var fields = cropField.Split(',');
                            foreach (var field in fields)
                            {

                                var imgParam = JsonHelper.Deserialize<ImageParam>(HttpContext.Current.Request.Form[field + "_param"].ToString());
                                if (imgParam != null)
                                {
                                    string sourceFilePath = HttpContext.Current.Server.MapPath(imgParam.Url);

                                    toRemove.Add(sourceFilePath);

                                    var contentPath = new TextContentPath(content);

                                    var vPath = Kooboo.Web.Url.UrlUtility.Combine(contentPath.VirtualPath, "kooboo-crop-" + Path.GetFileName(imgParam.Url));

                                    Kooboo.IO.IOUtility.EnsureDirectoryExists(contentPath.PhysicalPath);

                                    var phyPath = HttpContext.Current.Server.MapPath(vPath);

                                    Kooboo.Drawing.ImageTools.CropImage(sourceFilePath, phyPath, imgParam.X, imgParam.Y, imgParam.Width, imgParam.Height);
                                    content[field] = vPath;
                                }
                            }

                        }
                        foreach (var r in toRemove)
                        {
                            if (File.Exists(r))
                            {
                                File.Delete(r);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    eventResult.Exception = e;
                }
            }

            return eventResult;
        }

        class ImageParam
        {
            public string Url { get; set; }

            public int X { get; set; }
            public int Y { get; set; }

            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
