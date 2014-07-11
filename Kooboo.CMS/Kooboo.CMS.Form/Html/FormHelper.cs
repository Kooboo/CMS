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
using Kooboo.CMS.Form.Html.Controls;
using System.IO;
using System.CodeDom.Compiler;
using System.Web;
using Kooboo.Common.ObjectContainer;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Form.Html
{
    public static class FormHelper
    {
        #region Static
        static string TemplateDir = "";
        static string TemplateVirtualPath = "";

        static IDictionary<string, ISchemaForm> forms = new Dictionary<string, ISchemaForm>(StringComparer.CurrentCultureIgnoreCase);

        static FormHelper()
        {
            var baseDir = EngineContext.Current.Resolve<IBaseDir>();
            TemplateDir = Path.Combine(baseDir.Cms_DataPhysicalPath, "ContentType_Templates", "Forms");
            TemplateVirtualPath = UrlUtility.Combine(baseDir.Cms_DataVirtualPath, "ContentType_Templates", "Forms");

            forms.Add("Grid", new GridForm());
            forms.Add("Create", new CreateForm());
            forms.Add("Update", new UpdateForm());
            forms.Add("Selectable", new SelectableForm());
            forms.Add("List", new ListForm());
            forms.Add("Detail", new DetailForm());
        }
        #endregion

        #region Register
        public static void RegisterFormGenerator(string formType, ISchemaForm schemaForm)
        {
            lock (forms)
            {
                forms[formType] = schemaForm;
            }
        }
        #endregion

        #region Tooltip
        public static string Tooltip(string tip)
        {
            if (string.IsNullOrEmpty(tip))
            {
                return "";
            }
            return string.Format("<em class='tip'>{0}</em>", (tip).RazorHtmlEncode());
        }
        #endregion

        #region Enctype
        public static string Enctype(this ISchema schema)
        {
            var upload = schema.Columns.Select(it => string.IsNullOrEmpty(it.ControlType) ? null : ControlHelper.Resolve(it.ControlType)).Any(it => it != null && it.IsFile == true);
            return upload ? "multipart/form-data" : "application/x-www-form-urlencoded";
        }
        #endregion

        #region Generate
        public static string Generate(this ISchema schema, string formType)
        {
            var rendered = false;

            string html = ProcessRazorView(schema, formType, out rendered);
            if (!rendered)
            {
                ISchemaForm schemaForm = forms[formType];
                html = schemaForm.Generate(schema);
            }

            return html;
        }
        #endregion

        #region Razor view
        private static string GetFormView(string form)
        {
            return Path.Combine(TemplateDir, form + ".cshtml");
        }
        private static string GetFormViewVirtualPath(string form)
        {
            return UrlUtility.Combine(TemplateVirtualPath, form + ".cshtml");
        }
        public static string ProcessRazorView(ISchema schema, string form, out bool rendered)
        {
            string controlFile = GetFormView(form);
            rendered = false;
            if (System.IO.File.Exists(controlFile))
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["ControllerContext"] != null)
                {
                    var controllerContext = (ControllerContext)HttpContext.Current.Items["ControllerContext"];
                    var viewData = new ViewDataDictionary() { Model = schema };
                    rendered = true;
                    return RazorViewHelper.RenderView(GetFormViewVirtualPath(form), controllerContext, viewData);
                }
            }

            return "";
        }
        #endregion
    }
}
