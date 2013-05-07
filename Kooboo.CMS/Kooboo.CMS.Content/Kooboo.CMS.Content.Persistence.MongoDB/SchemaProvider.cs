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


namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public class SchemaProvider : Kooboo.CMS.Content.Persistence.Default.SchemaProvider
    {
        public override void Initialize(Kooboo.CMS.Content.Models.Schema schema)
        {
            base.Initialize(schema);
            schema.CreateIndex();
        }
        public override void Remove(Schema item)
        {
            base.Remove(item);
            item.DropCollection();
        }
    }
}
