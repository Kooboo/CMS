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
using Kooboo.CMS.Sites.View.CodeSnippet;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.CMS.Sites.TemplateEngines.WebForm
{
	public class WebFormLayoutPositionParser : ILayoutPositionParser
	{
		public IEnumerable<string> Parse(string layoutBody)
		{
			Regex regex = new Regex(@".Position\s*\(\s*""(?<PositionId>[^'|""]+)"".*\)", RegexOptions.IgnoreCase);
			MatchCollection matches = regex.Matches(layoutBody);
			foreach (Match item in matches)
			{
				yield return item.Groups["PositionId"].Value;
			}
		}

		public IHtmlString RegisterClientParser()
		{
			string script = "function getPositionList(body){" +

                @"var list = [], reg = /Html.FrontHtml\(\s*\).Position\(\s*""[^""|']*""\s*\)/g,r = reg.exec(body);
			while (r) {{
				r = r.toString();
				var position = r.substr(r.indexOf('""') + 1, r.lastIndexOf('""') - r.indexOf('""') - 1);
				list.push(position);
				r = reg.exec(body);
			}}
			return list;" +

				" }";

			return new HtmlString(script);
		}

		public IHtmlString RegisterClientAddPosition()
		{
			string script = "function getPositionCodeSnippet(name){" +
				"return '<%:Html.FrontHtml().Position(\"' + name + '\")%>';" +
				" }";

			return new HtmlString(script);
		}

		public IHtmlString RegisterClientRemovePosition()
		{
			string script = "function removePosition(name,body){" +
				"var reg = /<%[:|=]\\s*Html.FrontHtml\\(\\s*\\).Position\\(\\s*\\\"(\\w+)\\\"\\s*\\)\\s*%>/g,nameReg = new RegExp('\"'+name+'\"');  return body.replace(reg, function (a, b) { if (nameReg.test(a)) { return ''; } else return a; });" +
					" }";

			return new HtmlString(script);
		}
	}
}
