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
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Query.Translator.String
{
    public static class TextTranslator
    {
        public static string Translate<T>(IContentQuery<T> contentQuery)
            where T : ContentBase
        {
            if (contentQuery is MediaContentQuery)
            {
                var translator = new MediaContentQueryTranslator();
                return translator.Translate((MediaContentQuery)contentQuery).ToString();
            }
            else
            {
                var translator = new TextContentQueryTranslator();
                return translator.Translate((TextContentQuery)contentQuery).ToString();
            }

        }
    }
}
