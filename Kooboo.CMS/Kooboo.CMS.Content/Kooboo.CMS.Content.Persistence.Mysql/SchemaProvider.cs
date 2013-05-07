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
using System.Data.SqlClient;
using Ionic.Zip;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class SchemaProvider : Default.SchemaProvider
    {

        #region IProvider<Schema> Members

        public override void Update(Models.Schema @new, Models.Schema old)
        {
            base.Update(@new, old);
            SchemaManager.Update(@new, old);
        }

        public override void Remove(Models.Schema item)
        {
            base.Remove(item);
            SchemaManager.Delete(item);
        }

        #endregion

        #region IImportProvider Members

        public override void Import(Models.Repository repository, System.IO.Stream zipStream, bool @override)
        {
            List<string> schemaNames = new List<string>();
            Dictionary<string, Schema> oldSchemas = new Dictionary<string, Schema>();
            using (var zipFile = ZipFile.Read(zipStream))
            {

                foreach (var entry in zipFile.Entries)
                {
                    if (entry.FileName.IndexOf('/') == entry.FileName.Length - 1)
                    {
                        var schemaName = entry.FileName.Substring(0, entry.FileName.Length - 1);
                        schemaNames.Add(schemaName);
                        oldSchemas[schemaName] = this.Get(new Schema(repository, schemaName));
                    }
                }
            }
            zipStream.Position = 0;
            base.Import(repository, zipStream, @override);

            foreach (var name in schemaNames)
            {
                var oldSchema = oldSchemas[name];
                var newSchema = this.Get(new Models.Schema(repository, name));
                if (oldSchema != null)
                {
                    SchemaManager.Update(newSchema, oldSchema);
                }
                else
                {
                    Initialize(newSchema);
                }
            }
        }

        #endregion

        public override void Initialize(Models.Schema schema)
        {
            SchemaManager.Add(schema);
        }
    }
}
