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

namespace Kooboo
{
    /// <summary>
    /// 
    /// </summary>
    public class FileExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Image = ".jpg,.jpeg,.gif,.png,.bmp";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string[] ImageArray = Image.Split(',');

        /// <summary>
        /// 
        /// </summary>
        public const string Html = ".txt,.htm,.html";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string[] HtmlArray = Html.Split(',');

        /// <summary>
        /// 
        /// </summary>
        public const string Css = ".css";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string[] CssArray = Css.Split(',');
    }
}
