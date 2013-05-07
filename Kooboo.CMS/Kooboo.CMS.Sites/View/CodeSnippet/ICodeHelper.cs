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

namespace Kooboo.CMS.Sites.View.CodeSnippet
{
    public interface ICodeHelper
    {
        string RegisterTitle();
        string RegisterHtmlMeta();
        string RegisterStyles();
        string RegisterScripts();


        string RenderView(string viewName);
        //string RenderParameter(string parameterName);

        string DefaultViewCode();
        string RegisterParameterCode();

        //string PageLink(string linkText, string pageName);
        //string PageLinkWithParams(string linkText, string pageName);
        //string PageUrl(string pageName);
        //string PageUrlWithParams(string pageName);
        //string FileUrl(string fileName);

        //string Label(string labelText);

        //string Menu_Top();
        //string Menu_Sibling();
        //string Menu_Sub();
        //string Menu_Parent();
        //string Menu_Current();
        //string Menu_IsCurrent();
        //string Menu_Breadcrumb();

        
    }
}
