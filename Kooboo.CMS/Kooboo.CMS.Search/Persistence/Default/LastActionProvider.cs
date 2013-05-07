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
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Search.Persistence.Default
{
    public class LastActionPath : IPath
    {
        public LastActionPath(Repository repository)
        {
            this.PhysicalPath = Path.Combine(SearchDir.GetBasePhysicalPath(repository), "LastActions.xml");

        }
        #region IPath Members

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string SettingFile
        {
            get { throw new NotImplementedException(); }
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILastActionProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<LastAction>))]
    public class LastActionProvider : ILastActionProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public IEnumerable<Models.LastAction> All(Content.Models.Repository repository)
        {
            locker.EnterReadLock();
            try
            {
                return GetList(repository).AsQueryable();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }
        private List<LastAction> GetList(Repository repository)
        {
            var filePath = new LastActionPath(repository).PhysicalPath;
            if (!File.Exists(filePath))
            {
                return new List<LastAction>();
            }
            var list = Serialization.DeserializeSettings<List<LastAction>>(filePath);
            foreach (var item in list)
            {
                item.Repository = repository;
            }
            return list;
        }
        private void SaveList(Repository repository, List<LastAction> lastActions)
        {
            var filePath = new LastActionPath(repository).PhysicalPath;
            Serialization.Serialize<List<LastAction>>(lastActions, filePath);
        }
        public Models.LastAction Get(Models.LastAction dummy)
        {
            throw new NotImplementedException();
        }

        public void Add(Models.LastAction item)
        {
            locker.EnterWriteLock();
            try
            {
                var list = GetList(item.Repository);
                if (list.Count >= 5)
                {
                    list.RemoveRange(4, list.Count - 4);
                }
                list.Insert(0, item);
                SaveList(item.Repository, list);
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        public void Update(Models.LastAction @new, Models.LastAction old)
        {
            throw new NotSupportedException();
        }

        public void Remove(Models.LastAction item)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<LastAction> All()
        {
            throw new NotSupportedException();
        }
    }
}
