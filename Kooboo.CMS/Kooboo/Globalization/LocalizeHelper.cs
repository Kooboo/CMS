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
using System.IO;
using System.Text.RegularExpressions;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizeHelper
    {
        #region Fields
        public const string StringPattern = @"([^""]|\\""|"""")*";
        public static string LocalizePattern = String.Format(@"(?<![""\\])""(?<s>{0})"".Localize\(\)", StringPattern);
        public static string ErrorMessagePattern = String.Format(@"ErrorMessage\s*=\s*""(?<s>{0})""", StringPattern);
        public static string DisplayNamePattern = String.Format(@"DisplayName(""(?<s>{0})"")", StringPattern);
        public static string DescriptionPattern = String.Format(@"Description(""(?<s>{0})"")", StringPattern); 
        #endregion

        #region Methods

        /// <summary>
        /// Generates the resource.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="patterns">The patterns.</param>
        public static void GenerateResource(string dir, params string[] patterns)
        {
            foreach (var file in GetFiles(dir, "*.cs", "*.ascx", "*.aspx", "*.cshtml"))
            {
                var content = File.ReadAllText(file);
                foreach (var pattern in patterns)
                {
                    foreach (Match match in Regex.Matches(content, pattern))
                    {
                        match.Groups["s"].Value.Localize();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="extensions">The extensions.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetFiles(string dir, params string[] extensions)
        {
            var list = new List<string>();
            foreach (var each in extensions)
            {
                list.AddRange(Directory.GetFiles(dir, each, SearchOption.AllDirectories));
            }
            return list;
        } 
        #endregion
    }
}
