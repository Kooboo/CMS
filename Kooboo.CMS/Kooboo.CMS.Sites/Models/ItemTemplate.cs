using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class ItemTemplate
    {
        public ItemTemplate() { }
        public ItemTemplate(string tempateFullName)
        {
            var nameSplit = tempateFullName.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nameSplit.Length > 1)
            {
                Category = nameSplit[0];
                TemplateName = nameSplit[1];
            }
            else
            {
                TemplateName = nameSplit[0];
            }
        }
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Category))
                {
                    return this.TemplateName;
                }
                return Category + "||" + TemplateName;
            }
        }
        public string Category { get; set; }
        public string TemplateName { get; set; }
        public string Thumbnail { get; set; }
        public string TemplateFile { get; set; }
    }
}
