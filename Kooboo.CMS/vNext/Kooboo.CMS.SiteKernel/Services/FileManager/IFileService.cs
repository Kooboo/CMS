#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public interface IFileService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="path"></param>
        /// <param name="readContent">读取文件的内容</param>
        /// <returns></returns>
        IEnumerable<File> AllFiles(Site site, string path, bool readContent = false);
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="site"></param>
        /// <param name="absoluteFileName"></param>
        /// <param name="content"></param>
        void AddFile(Site site, string absoluteFileName, string content);

        File GetFile(Site site, string absoluteFileName);

        void DeleteFile(Site site, string absoluteFileName);

        bool IsFileExists(Site site, string absoluteFileName);

        IEnumerable<string> AllDirectories(Site site, string path);
        bool IsDirectoryExists(Site site, string dirPath);
        void AddDirectory(Site site, string absoluteDirPath);
        void DeleteDirectory(Site site, string absoluteDirPath);
    }
}
