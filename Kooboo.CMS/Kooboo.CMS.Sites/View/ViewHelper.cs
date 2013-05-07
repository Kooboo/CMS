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
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Content.Models;
using System.Web;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.View
{
    /// <summary>
    /// For NVelocity
    /// </summary>
    public class ViewHelper
    {
        public static IHtmlString Label(string defaultValue)
        {
            return Label(defaultValue, defaultValue);
        }
        public static IHtmlString Label(string defaultValue, string key)
        {
            return Label(defaultValue, key, "");
        }
        public static IHtmlString Label(string defaultValue, string key, string category)
        {
            return defaultValue.Label(key, category);
        }

        public static DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
        public static DateTime ToLocalDate(DateTime dateTime)
        {
            return dateTime.ToLocalTime();
        }
        public static string DateTimeToString(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        #region InlineEdit
        [Obsolete("Use EditFieldAttributes")]
        public static IHtmlString Edit(TextContent data, string fieldName)
        {
            return EditFieldAttributes(data, fieldName);
        }
        public static IHtmlString EditFieldAttributes(TextContent data, string fieldName)
        {
            return EditFieldAttributes(data, fieldName, FieldDataType.Auto);
        }
        public static IHtmlString EditFieldAttributes(TextContent data, string fieldName, FieldDataType dataType)
        {
            if (dataType == FieldDataType.Auto)
            {
                dataType = QueryFieldDataType(data, fieldName);
            }
            if (data == null || !Page_Context.Current.EnabledInlineEditing(EditingType.Content)
                || !Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableToEditContent(data, Page_Context.Current.ControllerContext.HttpContext.User.Identity.Name))
            {
                return new HtmlString("");
            }
            return new HtmlString(string.Format("editType=\"field\" dataType=\"{0}\" schema=\"{1}\" folder=\"{2}\" uuid=\"{3}\" fieldName=\"{4}\"", dataType.ToString(), data.SchemaName, data.FolderName, data.UUID, fieldName));
        }
        [Obsolete("Use EditItemAttributes")]
        public static IHtmlString Edit(TextContent data)
        {
            return EditItemAttributes(data);
        }
        public static IHtmlString EditItemAttributes(TextContent data)
        {
            var userName = Page_Context.Current.ControllerContext.HttpContext.User.Identity.Name;
            if (data == null || !Page_Context.Current.EnabledInlineEditing(EditingType.Content)
                || !Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableToEditContent(data, userName))
            {
                return new HtmlString("");
            }
            var availableToPublish = Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableToPublish(data.GetFolder(), userName);
            return new HtmlString(string.Format("editType=\"list\" schema=\"{0}\" folder=\"{1}\" uuid=\"{2}\" published=\"{3}\" editUrl=\"{4}\" summary=\"{5}\" publishAvailable=\"{6}\"",
                    data.SchemaName, data.FolderName, data.UUID, data.Published
                , Page_Context.Current.Url.Action("InlineEdit", new
                {
                    controller = "TextContent",
                    Area = "Contents",
                    RepositoryName = data.Repository,
                    SiteName = Page_Context.Current.PageRequestContext.Site.FullName,
                    FolderName = data.FolderName,
                    UUID = data.UUID,
                    Return = Page_Context.Current.ControllerContext.HttpContext.Request.RawUrl
                }), HttpUtility.HtmlAttributeEncode(data.GetSummary())
                , availableToPublish));
        }
        public static IHtmlString EditField(TextContent data, string fieldName)
        {
            return EditField(data, fieldName, FieldDataType.Auto);
        }
        public static IHtmlString EditField(TextContent data, string fieldName, FieldDataType dataType)
        {
            if (dataType == FieldDataType.Auto)
            {
                dataType = QueryFieldDataType(data, fieldName);
            }
            if (data == null || !Page_Context.Current.EnabledInlineEditing(EditingType.Content)
                || !Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableToEditContent(data, Page_Context.Current.ControllerContext.HttpContext.User.Identity.Name))
            {
                return new HtmlString(data[fieldName] == null ? "" : data[fieldName].ToString());
            }
            var format = "<var start=\"true\" editType=\"field\" dataType=\"{0}\" schema=\"{1}\" folder=\"{2}\" uuid=\"{3}\" fieldName=\"{4}\" style=\"display:none;\"></var>{5}<var end=\"true\" style=\"display:none;\"></var>";
            return new HtmlString(string.Format(format, dataType.ToString(), data.SchemaName, data.FolderName, data.UUID, fieldName, data[fieldName]));
        }
        private static FieldDataType QueryFieldDataType(TextContent data, string fieldName)
        {
            var dataType = FieldDataType.RichText;

            fieldName = fieldName ?? string.Empty;
            var column = data.GetSchema().AsActual().AllColumns
                .Where(o => fieldName.Equals(o.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (column != null)
            {
                if ("TextBox".Equals(column.ControlType, StringComparison.OrdinalIgnoreCase) ||
                    "TextArea".Equals(column.ControlType, StringComparison.OrdinalIgnoreCase))
                {
                    dataType = FieldDataType.Text;
                }
                else if ("Tinymce".Equals(column.ControlType, StringComparison.OrdinalIgnoreCase))
                {
                    dataType = FieldDataType.RichText;
                }
                else if ("Date".Equals(column.ControlType, StringComparison.OrdinalIgnoreCase) ||
                         "DateTime".Equals(column.ControlType, StringComparison.OrdinalIgnoreCase))
                {
                    dataType = FieldDataType.Date;
                }
            }
            else
            {
                dataType = FieldDataType.RichText;
            }

            // ret
            return dataType;
        }
        #endregion
    }

    public enum FieldDataType
    {
        Auto,
        Text,
        Date,
        RichText
    }
}
