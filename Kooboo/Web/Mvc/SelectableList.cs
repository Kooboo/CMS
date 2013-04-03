using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Collections;

namespace Kooboo.Web.Mvc
{
    public class SelectableList : IEnumerable<SelectListItem>, IEnumerable
    {
        public SelectableList(IEnumerable<SelectListItem> items)
        {
            this.Items = items;
        }

        IEnumerable<SelectListItem> Items
        {
            get;
            set;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable<SelectListItem> Members

        IEnumerator<SelectListItem> IEnumerable<SelectListItem>.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        //public static SelectableList Build<T>(IEnumerable<T> items, Func<T, object> dataValueField, Func<T, object> dataTextField, IEnumerable<T> selecteds)
        //{
        //    if (selecteds == null)
        //    {
        //        return Build<T>(items, dataValueField, dataTextField);
        //    }
        //    else
        //    {
        //        return Build<T>(items, dataValueField, dataTextField,selecteds.ToArray());
        //    }
        //}

        public static SelectableList Build<T>(IEnumerable<T> items, Func<T, object> dataValueField, Func<T, object> dataTextField, params T[] selecteds)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<object> selectedValues = new List<object>();

            if (selecteds != null && selecteds.Length > 0)
            {
                selectedValues = selecteds.Where(i=>i !=null).Select(i => dataValueField(i)).ToList();
            }

            foreach (var i in items)
            {
                var value = dataValueField(i);
                var text = dataTextField(i);

                var item = new SelectListItem
                {
                    Value = value.ToString(),
                    Text = text.ToString()
                };

                item.Selected = selectedValues.Any(m => m.Equals(value));
             
                list.Add(item);
            }

            return new SelectableList(list);
        }
    }
}
