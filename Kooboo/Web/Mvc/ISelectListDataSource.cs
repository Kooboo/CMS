using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Globalization;
using System.Web.Routing;
namespace Kooboo.Web.Mvc
{
    public interface ISelectListDataSource
    {
        IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null);
    }
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> SetActiveItem(this IEnumerable<SelectListItem> listItems, object value)
        {
            if (value == null)
            {
                return listItems;
            }
            string[] values = null;

            if (value is IEnumerable<object>)
            {
                values = ((IEnumerable<object>)value).Select(it => it.ToString()).ToArray();
            }
            else if (value is Enum)
            {
                values = new[] { ((int)value).ToString() };
            }
            else
            {
                values = new[] { value.ToString() };
            }

            return listItems.Select(it => new SelectListItem() { Text = it.Text, Value = it.Value, Selected = values.Contains(it.Value, StringComparer.CurrentCultureIgnoreCase) });
        }

        public static IEnumerable<SelectListItem> EmptyItem(this IEnumerable<SelectListItem> listItems, string emptyLabel)
        {
            return new[] { new SelectListItem() { Text = emptyLabel, Value = "" } }.Concat(listItems);
        }
    }

    public class EmptySelectListDataSource : ISelectListDataSource
    {
        #region IDropDownListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            return new SelectListItem[] { };
        }

        #endregion
    }
    public class CultureSelectListDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
            foreach (var c in cultures.OrderBy(c => c.DisplayName))
            {
                yield return new SelectListItem() { Text = c.NativeName, Value = c.Name, Selected = c.Equals(System.Threading.Thread.CurrentThread.CurrentCulture) };
            }
        }

        #endregion
    }
    public class EnumTypeSelectListDataSource : ISelectListDataSource
    {
        public EnumTypeSelectListDataSource(Type enumType)
        {
            EnumType = enumType;
        }
        public Type EnumType { get; private set; }
        #region ISelectListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            foreach (var item in Enum.GetValues(EnumType))
            {
                yield return new SelectListItem() { Text = item.ToString(), Value = ((int)item).ToString() };
            }
        }

        #endregion
    }
}
