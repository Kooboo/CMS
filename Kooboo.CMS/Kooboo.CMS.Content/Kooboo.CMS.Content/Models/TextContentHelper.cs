#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common;
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Models
{
    public static class TextContentHelper
    {
        public static TextContent ConvertToUTCTime(this TextContent textContent)
        {
            ITimeZoneHelper timeZoneHelper = EngineContext.Current.Resolve<ITimeZoneHelper>();
            TextContent newTextContent = new TextContent(textContent);

            foreach (var key in newTextContent.Keys.ToArray())
            {
                if (newTextContent[key] is DateTime)
                {
                    DateTime dt = (DateTime)newTextContent[key];
                    if (dt.Kind != DateTimeKind.Utc)
                    {
                        newTextContent[key] = timeZoneHelper.ConvertToUtcTime(dt);
                    }
                }
            }

            return newTextContent;
        }

        public static TextContent ConvertToLocalTime(this TextContent textContent)
        {
            ITimeZoneHelper timeZoneHelper = EngineContext.Current.Resolve<ITimeZoneHelper>();
            TextContent newTextContent = new TextContent(textContent);

            foreach (var key in newTextContent.Keys.ToArray())
            {
                if (newTextContent[key] is DateTime)
                {
                    DateTime dt = (DateTime)newTextContent[key];
                    if (dt.Kind != DateTimeKind.Local)
                    {
                        newTextContent[key] = timeZoneHelper.ConvertToLocalTime(dt, TimeZoneInfo.Utc);
                    }
                }
            }

            return newTextContent;
        }
    }
}
