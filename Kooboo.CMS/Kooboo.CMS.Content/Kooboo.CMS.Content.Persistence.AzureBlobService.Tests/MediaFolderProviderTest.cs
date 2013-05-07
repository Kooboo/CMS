#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService.Tests
{
    [TestClass]
    public class MediaFolderProviderTest
    {
        Repository repository = new Repository(Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(5));
        MediaFolderProvider provider = new MediaFolderProvider();
        [TestMethod]
        public void Test_Add_Folder()
        {
            MediaFolder mediaFolder = new MediaFolder(repository, "Test_Add_Folder");
            mediaFolder.DisplayName = "Test Add Folder";
            mediaFolder.UserId = "admin";
            provider.Add(mediaFolder);
        }
        [TestMethod]
        public void Test_Update_Folder()
        {
            MediaFolder mediaFolder = new MediaFolder(repository, "Test_Update_Folder");
            mediaFolder.DisplayName = "Test Update Folder";
            mediaFolder.UserId = "admin";
            provider.Add(mediaFolder);

            mediaFolder.DisplayName = "Updated Folder";
            provider.Update(mediaFolder, mediaFolder);
        }
        [TestMethod]
        public void Test_Delete_Folder()
        {
            MediaFolder mediaFolder = new MediaFolder(repository, "Test_Delete_Folder");
            mediaFolder.DisplayName = "Test delete Folder";
            mediaFolder.UserId = "admin";
            provider.Add(mediaFolder);

            provider.Remove(mediaFolder);
        }
        [TestMethod]
        public void Test_Get_Folder()
        {
            MediaFolder mediaFolder = new MediaFolder(repository, "Test_Get_Folder");
            mediaFolder.DisplayName = "Test get Folder";
            mediaFolder.UserId = "admin";
            provider.Add(mediaFolder);

            var gotFolder = provider.Get(mediaFolder);
            Assert.IsNotNull(gotFolder);
            Assert.AreEqual(mediaFolder.DisplayName, gotFolder.DisplayName);
            Assert.AreEqual(mediaFolder.UserId, gotFolder.UserId);
        }
    }
}
