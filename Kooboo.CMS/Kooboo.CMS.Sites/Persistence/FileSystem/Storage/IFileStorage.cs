#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem.Storage
{
    public interface IFileStorage<T>
         where T : IPersistable, new()
    {
        IEnumerable<T> GetList();
        T Get(T dummy);
        void Add(T item, bool @override = true);
        void Update(T item, T oldItem);
        void Remove(T item);

        void Export(IEnumerable<T> items, Stream outputStream);

        void Import(Stream zipStream, bool @override);
    }
}
