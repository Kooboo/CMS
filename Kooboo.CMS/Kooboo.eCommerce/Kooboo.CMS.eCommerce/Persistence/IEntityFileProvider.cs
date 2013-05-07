#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Persistence
{
    public interface IEntityFileProvider
    {
        EntityFileOperationResult Save(EntityFile entityFile, string folderName);
        void DeleteFolder(string folderName);
        void Delete(string virutalPath);
    }
}
