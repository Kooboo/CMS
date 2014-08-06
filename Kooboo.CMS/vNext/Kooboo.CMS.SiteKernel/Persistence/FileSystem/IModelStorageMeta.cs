#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Data.IsolatedStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    public interface IModelStorageMeta<T>
    {
        /// <summary>
        /// 站点对应的存储单元
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        IIsolatedStorage GetIsolatedStorage(Site site);
        /// <summary>
        /// 对象模型在存储单元的相对路径
        /// </summary>
        /// <returns></returns>
        string GetBasePath();
        /// <summary>
        /// 单个对象对应的存储路径
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetItemPathName(T item);

        /// <summary>
        /// 对象的setting文件目录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetItemSettingFileName(T item);

        T CreateItem(Site site, string pathName);

    }
}
