using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiffPlex.DiffBuilder.Model;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class TextContentDiffModel
    {
        public static readonly string DIFFSEPERATOR = "\r\nCMS_DIFFSEPERATOR_CMS_DIFFSEPERATOR_{0}\r\n";

        public string UUID { get; set; }

        public object Version1Name { get; set; }
        public object Version2Name { get; set; }


        private IDictionary<string, DiffPaneModel> version1 = new Dictionary<string, DiffPaneModel>();
        public IDictionary<string, DiffPaneModel> Version1
        {
            get
            {
                return version1;
            }
            set
            {
                version1 = value;
            }
        }
        private IDictionary<string, DiffPaneModel> version2 = new Dictionary<string, DiffPaneModel>();
        public IDictionary<string, DiffPaneModel> Version2
        {
            get
            {
                return version2;
            }
            set
            {
                version2 = value;
            }
        }
    }
}