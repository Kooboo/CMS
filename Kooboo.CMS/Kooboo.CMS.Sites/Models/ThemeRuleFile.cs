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

namespace Kooboo.CMS.Sites.Models
{
	public class ThemeRuleFile : ThemeFile
	{
		public ThemeRuleFile(string physicalPath)
			: base(physicalPath)
		{
		}
		public ThemeRuleFile(Theme theme)
			: base(theme, "Theme.rule")
		{
		}
	}
}
