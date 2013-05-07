#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;
using Kooboo.CMS.Form.Html.Controls;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class SchemaExtensions
    {
        public static string CUSTOM_TEMPLATES = "CustomTemplates";
        
        /// <summary>
        /// 生成Schema模板
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="formType">Type of the form.</param>
        /// <returns></returns>
        public static string GenerateForm(this Schema schema, FormType formType)
        {
            ISchema iSchema = schema.AsActual();

            return iSchema.Generate(formType.ToString());
        }
        /// <summary>
        /// 获取Column对应的控件
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static IControl GetControlType(this Column column)
        {
            IControl control = ControlHelper.Resolve(column.ControlType);
            return control;
        }

        #region Template file
        /// <summary>
        /// 取得Schema的模板相对路径
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="formType">Type of the form.</param>
        /// <returns></returns>
        public static string GetFormTemplate(this Schema schema, FormType formType)
        {
            var schemaPath = new SchemaPath(schema);
            var virtualPath = GetCustomTemplateFileVirtualPath(schemaPath, formType);
            if (string.IsNullOrEmpty(virtualPath))
            {
                virtualPath = GetFormFileVirtualPath(schema, formType);
            }
            return virtualPath;
        }
        private static string GetFormFileVirtualPath(Schema schema, FormType type)
        {
            var razorTemplate = GetFormFilePhysicalPath(schema, type);
            var schemaPath = new SchemaPath(schema);
            var templateVirtualPath = UrlUtility.Combine(schemaPath.VirtualPath, string.Format("{0}.ascx", type));
            if (System.IO.File.Exists(razorTemplate))
            {
                templateVirtualPath = UrlUtility.Combine(schemaPath.VirtualPath, string.Format("{0}.cshtml", type));
            }
            return templateVirtualPath;
        }
        /// <summary>
        /// 取得Schema的模板物理路径
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetFormFilePhysicalPath(this Schema schema, FormType type)
        {
            var schemaPath = new SchemaPath(schema);
            return Path.Combine(schemaPath.PhysicalPath, string.Format("{0}.cshtml", type));
        }
        private static string GetCustomTemplateFileVirtualPath(SchemaPath schemaPath, FormType type)
        {
            string fileVirtualPath = "";
            string filePhysicalPath = Path.Combine(schemaPath.PhysicalPath, CUSTOM_TEMPLATES, string.Format("{0}.cshtml", type));
            if (System.IO.File.Exists(filePhysicalPath))
            {
                fileVirtualPath = UrlUtility.Combine(schemaPath.VirtualPath, CUSTOM_TEMPLATES, string.Format("{0}.cshtml", type));
            }
            return fileVirtualPath;
        }
        #endregion
    }
}
