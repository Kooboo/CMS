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
using Kooboo.CMS.Content.Models;
using System.IO;

namespace Kooboo.CMS.Content.Persistence
{
    public interface ISchemaProvider : IContentElementProvider<Schema>
    {
        void Initialize(Schema schema);

        Schema Create(Repository repository, string schemaName, Stream templateStream);

        Schema Copy(Repository repository, string sourceName, string destName);

        void Export(Repository repository, IEnumerable<Schema> models, System.IO.Stream outputStream);
        void Import(Repository repository, System.IO.Stream zipStream, bool @override);
    }
}