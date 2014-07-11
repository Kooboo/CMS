#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Content.Persistence.Default
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextFolderProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<TextFolder>))]
    public class TextFolderProvider : FolderProvider<TextFolder>, ITextFolderProvider
    {
        #region IFolderProvider Members

        public IQueryable<TextFolder> BySchema(Schema schema)
        {
            return BySchemaEnumerable(schema).AsQueryable();
        }

        public IEnumerable<TextFolder> BySchemaEnumerable(Schema schema)
        {
            IEnumerable<TextFolder> schemaFolders = new TextFolder[0];
            foreach (var item in All(schema.Repository))
            {
                schemaFolders = schemaFolders.Concat(BySchema(item, schema));
            }
            return schemaFolders;
        }

        private IEnumerable<TextFolder> BySchema(TextFolder folder, Schema schema)
        {
            IEnumerable<TextFolder> schemaFolders = new TextFolder[0];
            folder = folder.AsActual();
            if (folder is TextFolder && string.Compare(((TextFolder)folder).SchemaName, schema.Name, true) == 0)
            {
                schemaFolders = schemaFolders.Concat(new[] { folder });
            }

            foreach (var child in ChildFolders(folder))
            {
                schemaFolders = schemaFolders.Concat(BySchema(child, schema));
            }

            return schemaFolders;
        }

        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        } 
        #endregion
    }
}
