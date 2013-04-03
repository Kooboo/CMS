using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public class RegexPatterns
    {
        /// <summary>
        /// See http://en.wikipedia.org/wiki/Email_address#Syntax
        /// </summary>
        public const string EmailAddress = @"^\s*[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+(\.[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+)*@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\s*$";
        public const string Domain = @"\s*[\w.-]+\.[a-z]{2,4}\s*";
        public const string Alphanum = @"[\w\d_]+";
        public const string Version = @"^([0-9]+)\.([0-9]+)\.([0-9]+)\.([0-9])+$";
        public const string HttpUrl = @"(http|https):\/\/([\w\-_]+(\.[\w\-_]+)+|localhost)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
        public const string SimpleName = @"^[A-Za-z][\w]*$";
        public const string FileName = @"^[^\\\./:\*\?\""<>\|~ ]{1}[^\\/:\*\?\""<>\|~ ]{0,254}$";
        public const string IP = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
        public const string UserName = @"^[a-zA-Z]\w*$";
    }
}
