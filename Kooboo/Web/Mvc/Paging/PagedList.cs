/*
 ASP.NET MvcPager control
 Copyright:2009-2010 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Web.Mvc.Paging
{
    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
        int TotalPageCount { get; }
        int StartRecordIndex { get; }
        int EndRecordIndex { get; }
    }
    public class PagedList<T> : IEnumerable<T>, IPagedList
    {
        private IEnumerable<T> list;
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPageIndex = pageIndex;
            StartRecordIndex = (CurrentPageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            list = items.Skip(StartRecordIndex - 1).Take(pageSize).ToArray();
            //for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            //{
            //    Add(items[i]);
            //}
        }

        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            list = items;
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }
        public int StartRecordIndex { get; private set; }
        public int EndRecordIndex { get; private set; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
