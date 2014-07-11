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
using System.IO;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class TextContentPath : IPath
    {
        public TextContentPath(TextContent content)
        {
            var repository = new Repository(content.Repository);

            var textContent = content;
            if (string.IsNullOrEmpty(textContent.FolderName))
            {
                var schemaPath = new SchemaPath(new Schema(repository, textContent.SchemaName));
                // Compatible with the ContentFolderName has been change (_contents=>.contents)
                //this.PhysicalPath = Path.Combine(schemaPath.PhysicalPath, ContentFolderName, content.UUID);
                //this.VirtualPath = UrlUtility.Combine(schemaPath.VirtualPath, ContentFolderName, content.UUID);

                //if (!Directory.Exists(this.PhysicalPath))
                //{
                    this.PhysicalPath = Path.Combine(schemaPath.PhysicalPath, ContentAttachementFolder, content.UUID);
                    this.VirtualPath = UrlUtility.Combine(schemaPath.VirtualPath, ContentAttachementFolder, content.UUID);
                //}
            }
            else
            {
                FolderPath folderPath = null;

                folderPath = new FolderPath(FolderHelper.Parse<TextFolder>(repository, content.FolderName));
                //// Compatible with the ContentFolderName has been change (_contents=>.contents)
                //this.PhysicalPath = Path.Combine(folderPath.PhysicalPath, ContentFolderName, content.UUID);
                //this.VirtualPath = UrlUtility.Combine(folderPath.VirtualPath, ContentFolderName, content.UUID);
                //if (!Directory.Exists(this.PhysicalPath))
                //{
                    this.PhysicalPath = Path.Combine(folderPath.PhysicalPath, ContentAttachementFolder, content.UUID);
                    this.VirtualPath = UrlUtility.Combine(folderPath.VirtualPath, ContentAttachementFolder, content.UUID);
                //}

            }
            var sysFolderName = Path.GetDirectoryName(this.PhysicalPath);
            if (!Directory.Exists(sysFolderName))
            {
                Directory.CreateDirectory(sysFolderName);
                var sysFolder = new DirectoryInfo(sysFolderName);
                sysFolder.Attributes = sysFolder.Attributes | FileAttributes.Hidden;
            }

        }
        [Obsolete("Please use ContentAttachementFolder.")]
        public static string ContentFolderName = "_contents";

        public static string ContentAttachementFolder = "~contents";

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
            return Directory.Exists(this.PhysicalPath);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
