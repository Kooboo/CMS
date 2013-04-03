using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Form.Html.Controls;
using System.IO;

//using Microsoft.VisualStudio.TextTemplating;
//using Kooboo.CMS.Form.Html.T4;
using System.CodeDom.Compiler;
using System.Web;

namespace Kooboo.CMS.Form.Html
{
    public static class ControlHelper
    {
        public static string TemplateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Html", "Controls");

        static IDictionary<string, Controls.IControl> controls = new Dictionary<string, IControl>(StringComparer.CurrentCultureIgnoreCase);

        static ControlHelper()
        {
            RegisterControl(new TextBox());
            RegisterControl(new InputInt32());
            RegisterControl(new InputFloat());
            RegisterControl(new CheckBox());
            RegisterControl(new Date());
            RegisterControl(new Kooboo.CMS.Form.Html.Controls.File());
            RegisterControl(new ImageCrop());
            RegisterControl(new Display());
            RegisterControl(new Hidden());
            RegisterControl(new Empty());
            RegisterControl(new DropDownList());
            RegisterControl(new CheckBoxList());
            RegisterControl(new RadioList());
            RegisterControl(new MultiFiles());
            RegisterControl(new TextArea());
            RegisterControl(new Tinymce());
            RegisterControl(new HighlightEditor());
            RegisterControl(new Password());
            //RegisterControl(new InputNumber());
            //RegisterControl(new CLEditor());
        }

        public static void RegisterControl(IControl control)
        {
            controls[control.Name] = control;
        }

        public static IControl Resolve(string controlType)
        {
            IControl control = null;
            if (controls.ContainsKey(controlType))
            {
                control = controls[controlType];
            }
            return control;
        }
        public static IEnumerable<string> ResolveAll()
        {
            var controlNames = controls.Keys.AsEnumerable();
            if (Kooboo.Web.TrustLevelUtility.CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
            {
                if (Directory.Exists(TemplateDir))
                {
                    var files = Directory.EnumerateFiles(TemplateDir, "*.tt");
                    controlNames = controlNames.Concat(files.Select(it => Path.GetFileNameWithoutExtension(it)))
                       .Distinct(StringComparer.CurrentCultureIgnoreCase);
                }
            }
            return controlNames;
        }

        public static bool Contains(string controlType)
        {
            return ResolveAll().Where(it => string.Compare(it, controlType, true) == 0).Count() > 0;
        }

        static string AllowedEditWraper(string template)
        {
            var result = string.Empty;
            result = string.Format(@"
            @if (allowedEdit) {{
                {0}
            }}", template);
            return result;
        }

        static string AllowedViewWraper(string template)
        {
            var result = string.Empty;
            result = string.Format(@" 
            @if (allowedView) {{
                {0}
            }}", template);
            return result;
        }

        public static string Render(this IColumn column, ISchema schema, bool isUpdate)
        {
            var controlType = column.ControlType;
            if (isUpdate && !column.Modifiable)
            {
                controlType = "Hidden";
            }
            if (string.IsNullOrEmpty(controlType))
            {
                return string.Empty;
            }
            if (!Contains(controlType))
            {
                throw new Exception(string.Format("Control type {0} does not exists.", controlType));
            }

            string controlHtml = string.Empty;

            controlHtml = controls[controlType].Render(schema, column);

            if (string.Equals(column.Name, "published", StringComparison.OrdinalIgnoreCase))
            {
                controlHtml = AllowedEditWraper(controlHtml);
            }

            return controlHtml;
        }
        private static string ProcessT4(string content, IColumn column)
        {
            return "";
            //Engine engine = new Engine();

            //var host = new CustomTextTemplatingEngineHost();
            //host.Session = new TextTemplatingSession();
            //host.Session["column"] = column;
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
