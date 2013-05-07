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
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;

namespace Kooboo.CMS.Content.Tests.Persistence
{
    /// <summary>
    /// Summary description for ContentRepository_RepositoryTests
    /// </summary>
    [TestClass]
    public class FolderProviderTests
    {
        string repositoryName = "FolderProviderTests";
        Repository contentRepository = null;
        public FolderProviderTests()
        {
            contentRepository = new Repository(repositoryName) { DisplayName = "DisplayName" };
        }

        //[TestMethod]
        //public void TestContainerFolder()
        //{
        //    FolderProvider repository = new FolderProvider();


        //    var folderName = "Container";
        //    var folder = new ContainerFolder(contentRepository, folderName);

        //    repository.Add(folder);

        //    var all = repository.All(contentRepository);

        //    Assert.AreEqual(1, all.Count());

        //    var item = repository.Get(all.First());

        //    Assert.AreEqual(folder.Name, item.Name);
        //    Assert.AreEqual(folder.DisplayName, item.DisplayName);
        //    Assert.AreEqual(folder.Repository, item.Repository);

        //    repository.Remove(item);
        //}

        [TestMethod]
        public void TestTextContentFolder()
        {
            TextFolderProvider repository = new TextFolderProvider();           

            var folderName = "News";
            var folder = new TextFolder(contentRepository, folderName)
            {              
            };

            repository.Add(folder);

            var all = repository.All(contentRepository);

            Assert.AreEqual(1, all.Count());

            var item = (TextFolder)repository.Get(all.First());

            Assert.AreEqual(folder.Name, item.Name);
            Assert.AreEqual(folder.DisplayName, item.DisplayName);
            Assert.AreEqual(folder.Repository, item.Repository);
            Assert.AreEqual(folder.SchemaName, item.SchemaName);

            //repository.Remove(item);
        }
        [TestMethod]
        public void TestBinaryContentFolder()
        {
            MediaFolderProvider repository = new MediaFolderProvider();

            var folderName = "Images";
            var folder = new MediaFolder(contentRepository, folderName) { AllowedExtensions = new[] { ".jpg", ".png" } };

            repository.Add(folder);

            var all = repository.All(contentRepository);

            Assert.AreEqual(1, all.Count());

            var item = (MediaFolder)repository.Get(all.First());

            Assert.AreEqual(folder.Name, item.Name);
            Assert.AreEqual(folder.DisplayName, item.DisplayName);
            Assert.AreEqual(folder.Repository, item.Repository);
            Assert.AreEqual(folder.AllowedExtensions.Count(), item.AllowedExtensions.Count());

         //   repository.Remove(item);
        }
    }
}
