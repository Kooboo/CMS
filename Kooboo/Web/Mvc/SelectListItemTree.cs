using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Kooboo.Web.Mvc
{
	public class SelectListItemTree : SelectListItem
	{
		public IEnumerable<SelectListItemTree> Items { get; set; }
	}

	public static class SelectListItemTreeExtension
	{
		public static IEnumerable<SelectListItemTree> SetActiveItem(this IEnumerable<SelectListItemTree> listItems, object value)
		{
			string strValue = value == null ? "" : value.ToString();
			var list = listItems.ToArray();
			foreach (var item in list)
			{
				if (string.Compare(item.Value, strValue, true) == 0)
				{
					item.Selected = true;
				}
				else
				{
					item.Selected = false;
				}
				if (item.Items != null)
				{
					SetActiveItem(item.Items, value);
				}
			}
			return list;
		}
		public static string Indentation = "&nbsp;&nbsp;&nbsp;&nbsp;";
		public static IHtmlString DropDownListTree(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemTree> selectListItemTree)
		{
			return DropDownListTree(htmlHelper, name, selectListItemTree, null, false, null, Indentation);
		}
		public static IHtmlString DropDownListTree(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemTree> selectListItemTree, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.DropDownListTree(name, selectListItemTree, htmlAttributes, false, null, Indentation);
		}
		public static IHtmlString DropDownListTree(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemTree> selectListItemTree, string optionLabel)
		{
			return DropDownListTree(htmlHelper, name, selectListItemTree, null, false, optionLabel, Indentation);
		}
		public static IHtmlString DropDownListTree(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemTree> selectListItemTree, IDictionary<string, object> htmlAttributes, bool allowMutiple, string optionLabel, string indentation)
		{
			ModelState state;

			name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("NULL", "name");
			}
			int level = 0;
			TagBuilder selectBuilder = new TagBuilder("select");

			StringBuilder optionBuilder = new StringBuilder();

			//var blankOption = new TagBuilder("option");

			//optionBuilder.AppendLine(blankOption.ToString());
			if (optionLabel != null)
			{
				SelectListItemTree item = new SelectListItemTree();
				item.Text = optionLabel;
				item.Value = string.Empty;
				optionBuilder.AppendLine(CreateOption(item, level, indentation));
			}

			foreach (var item in selectListItemTree)
			{
				optionBuilder.AppendLine(CreateOption(item, level, indentation));
			}

			selectBuilder.InnerHtml = optionBuilder.ToString();
			selectBuilder.MergeAttributes<string, object>(htmlAttributes);
			selectBuilder.MergeAttribute("name", name, true);
			selectBuilder.GenerateId(name);
			if (allowMutiple)
			{
				selectBuilder.MergeAttribute("mutiple", "mutiple");
			}
			if (htmlHelper.ViewData.ModelState.TryGetValue(name, out state) && (state.Errors.Count > 0))
			{
				selectBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
			}
			return new HtmlString(selectBuilder.ToString());
		}
		private static string CreateOption(SelectListItemTree item, int level, string indentation)
		{
			StringBuilder htmlBuilder = new StringBuilder();
			TagBuilder option = new TagBuilder("option");
			option.InnerHtml = GetIndentation(level, indentation) + System.Web.HttpUtility.HtmlEncode(item.Text);
			if (item.Value != null)
			{
				option.Attributes["Value"] = item.Value;
			}
			if (item.Selected)
			{
				option.Attributes["selected"] = "selected";
			}
			htmlBuilder.AppendLine(option.ToString());
			if (item.Items != null)
			{
				++level;
				foreach (var i in item.Items)
				{
					htmlBuilder.AppendLine(CreateOption(i, level, indentation));
				}
			}


			return htmlBuilder.ToString();
		}
		private static string GetIndentation(int level, string indentation)
		{
			StringBuilder indentationBuilder = new StringBuilder();

			for (var i = 0; i < level; i++)
			{
				indentationBuilder.AppendLine(indentation);
			}

			return indentationBuilder.ToString();
		}
	}
}
