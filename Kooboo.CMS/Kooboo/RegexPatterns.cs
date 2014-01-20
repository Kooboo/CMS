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
    public class RegexPatterns
    {
        /// <summary>
        /// See http://en.wikipedia.org/wiki/Email_address#Syntax
        /// </summary>
        public const string EmailAddress = @"^\s*[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+(\.[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+)*@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\s*$";
        /// <summary>
        /// 
        /// </summary>
        public const string Domain = @"\s*[\w.-]+\.[a-z]{2,4}\s*";
        /// <summary>
        /// 
        /// </summary>
        public const string Alphanum = @"[\w\d_-]+";
        /// <summary>
        /// 
        /// </summary>
        public const string Version = @"^([0-9]+)\.([0-9]+)\.([0-9]+)\.([0-9])+$";
        /// <summary>
        /// 
        /// </summary>
        public const string HttpUrl = @"(http|https):\/\/([\w\-_]+(\.[\w\-_]+)+|localhost)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
        /// <summary>
        /// 
        /// </summary>
        public const string SimpleName = @"^[A-Za-z][\w]*$";
        /// <summary>
        /// 
        /// </summary>
        public const string FileName = @"^[^\\\./:\*\?\""<>\|~ ]{1}[^\\/:\*\?\""<>\|~ ]{0,254}$";
        /// <summary>
        /// 
        /// </summary>
        public const string IP = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
        /// <summary>
        /// 
        /// </summary>
        public const string UserName = @"^[a-zA-Z]\w*$";

        /// <summary>
        /// The veriable name
        /// </summary>
        public const string VeriableName = "^[a-zA-Z_][a-zA-Z0-9_]*$";
    }
}
