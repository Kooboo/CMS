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
namespace Kooboo.CMS.Sites.Models
{
    public static class ModelExtensions
    {
        public static Repository GetRepository(this Site site)
        {
            site = site.AsActual();
            if (!string.IsNullOrEmpty(site.Repository))
            {
                return new Repository(site.Repository);
            }
            return null;
        }
    }
}
