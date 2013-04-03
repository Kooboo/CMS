using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Form.Html.Controls;
using System.IO;
using System.CodeDom.Compiler;
using System.Web;

namespace Kooboo.CMS.Form.Html
{
    public static class FormHelper
    {
        public static string TemplateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Html");

        static IDictionary<string, ISchemaForm> forms = new Dictionary<string, ISchemaForm>(StringComparer.CurrentCultureIgnoreCase);

        static FormHelper()
        {
            forms.Add("Grid", new GridForm());
            forms.Add("Create", new CreateForm());
            forms.Add("Update", new UpdateForm());
            forms.Add("Selectable", new SelectableForm());
            forms.Add("List", new ListForm());
            forms.Add("Detail", new DetailForm());
        }

        public static void RegisterFormGenerator(string formType, ISchemaForm schemaForm)
        {
            lock (forms)
            {
                forms[formType] = schemaForm;
            }
        }

        public static string Enctype(this ISchema schema)
        {
            var upload = schema.Columns.Where(it => string.Compare("file", it.ControlType) == 0).Count();
            return upload > 0 ? "multipart/form-data" : "application/x-www-form-urlencoded";
        }

        public static string Generate(this ISchema schema, string formType)
        {
            string html = string.Empty;
            //string templateFile = Path.Combine(TemplateDir, formType + ".tt");            
            //if (Kooboo.Web.TrustLevelUtility.CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted && System.IO.File.Exists(templateFile))
            //{
            //    string templateContent = IO.IOUtility.ReadAsString(templateFile);
            //    html = ProcessT4(templateContent, schema);
            //}
            //else
            //{
            ISchemaForm schemaForm = forms[formType];
            html = schemaForm.Generate(schema);
            //}

            return html;
        }
        private static string ProcessT4(string content, ISchema schema)
        {
            return "";
            //Engine engine = new Engine();

            //var host = new CustomTextTemplatingEngineHost();
            //host.Session = new TextTemplatingSession();
            //host.Session["schema"] = schema;
            //string output = engine.ProcessTemplate(content, host);

            //if (host.Errors.Count > 0)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    foreach (CompilerError item in host.Errors)
            //    {
            //        sb.AppendLine(item.ToString());
            //    }
            //    output = sb.ToString();
            //}
            //return output;
        }
    }
}
