#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.FrontAPI;
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel
{
    public class Page_Context
    {
        #region .ctor
        public Page_Context()
        {
            Styles = new List<IHtmlString>();
            Scripts = new List<IHtmlString>();
        }
        #endregion

        public PageRequestContext PageRequestContext { get; private set; }
        public ControllerContext ControllerContext { get; private set; }

        #region Html

        /// <summary>
        /// Enable to generate the trace info, e.g: <!-- View1 -->
        /// </summary>
        public bool EnableTrace { get; set; }
        /// <summary>
        /// Gets or sets the page layout.
        /// </summary>
        /// <value>
        /// The page layout.
        /// </value>
        public string PageLayout { get; set; }

        public string PageTheme { get; set; }

        private HtmlMeta htmlMeta = new HtmlMeta();
        public HtmlMeta HtmlMeta
        {
            get
            {
                return htmlMeta;
            }
        }

        public IList<IHtmlString> Styles { get; private set; }
        public IList<IHtmlString> Scripts { get; private set; }
        #endregion

        #region Helpers

        private UrlHelper url;
        public System.Web.Mvc.UrlHelper Url
        {
            get
            {
                if (url == null)
                {
                    url = new UrlHelper(this.ControllerContext.RequestContext);
                }
                return url;
            }
        }
        private FrontUrlHelper frontUrl;
        public FrontUrlHelper FrontUrl
        {
            get
            {
                if (frontUrl == null)
                {
                    frontUrl = new FrontUrlHelper(this.Url, this.PageRequestContext.Site, this.PageRequestContext.RequestChannel);
                }
                return frontUrl;
            }
            set
            {
                frontUrl = value;
            }
        }


        #endregion
    }
}
