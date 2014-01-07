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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Formula
{
    /// <summary>
    /// parsing and populating the formula with value provider.
    /// The formula like: {Title} - SampleSite
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IFormulaParser))]
    public class FormulaParser : IFormulaParser
    {
        public virtual string Populate(string formula, IValueProvider valueProvider)
        {
            if (string.IsNullOrEmpty(formula))
            {
                return null;
            }
            var matches = Regex.Matches(formula, "{(?<Name>[^{^}]+)}");
            var s = formula;
            foreach (Match match in matches)
            {
                var key = match.Groups["Name"].Value;
                int intKey;
                //ignore {0},{1}... it is the format string.
                if (!int.TryParse(key, out intKey))
                {
                    var value = valueProvider.GetValue(match.Groups["Name"].Value);
                    var htmlValue = value == null ? "" : value.ToString();
                    s = s.Replace(match.Value, htmlValue);
                }

            }
            return s;
        }
    }
}
