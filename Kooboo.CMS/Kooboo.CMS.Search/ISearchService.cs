#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Kooboo.CMS.Search
{
    public interface ISearchService
    {
        void Add<T>(T o);
        void Update<T>(T o);
        void Delete<T>(T o);
        void BatchAdd<T>(IEnumerable<T> list);
        void BatchUpdate<T>(IEnumerable<T> list);
        void BatchDelete<T>(IEnumerable<T> list);
        void BatchDelete(string folderName);

        IPagedList<Models.ResultObject> Search(string key, int pageIndex, int pageSize, params string[] folders);
    }
}
