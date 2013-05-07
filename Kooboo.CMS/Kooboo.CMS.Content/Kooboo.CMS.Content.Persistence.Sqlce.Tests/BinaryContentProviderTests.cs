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
using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.Content.Persistence.Sqlce.Tests
{
    /// <summary>
    /// Summary description for BinaryContentProviderTests
    /// </summary>
    [TestClass]
    public class BinaryContentProviderTests
    {
        static Repository repository = null;
        static MediaFolder binaryFolder = null;
        static BinaryContentProviderTests()
        {
            repository = new Repository("SimpleQueryTests");

            binaryFolder = new MediaFolder(repository, "image");

            ProviderFactory providerFactory = new ProviderFactory();
            Providers.DefaultProviderFactory = providerFactory;
            providerFactory.GetProvider<IRepositoryProvider>().Add(repository);
            providerFactory.GetProvider<IMediaFolderProvider>().Add(binaryFolder);
        }
        [TestMethod]
        public void Test1()
        {
            MediaContent binaryContent1 = new MediaContent();
            binaryContent1.Repository = repository.Name;
            binaryContent1.FolderName = binaryFolder.Name;
            binaryContent1.FileName = "file1.jpg";
            binaryContent1.VirtualPath = "~/file1.jpg";
            Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>().Add(binaryContent1);

            MediaContent binaryContent2 = new MediaContent();
            binaryContent2.Repository = repository.Name;
            binaryContent2.FolderName = binaryFolder.Name;
            binaryContent2.FileName = "file2.jpg";
            binaryContent2.VirtualPath = "~/file2.jpg";
            Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>().Add(binaryContent2);


            var query = binaryFolder.CreateQuery().OrderBy("id");

            Assert.AreEqual(2, query.Count());

            Assert.AreEqual("file1.jpg", query.First().FileName);
            Assert.AreEqual("file2.jpg", query.Last().FileName);

            Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>().Delete(binaryContent1);
            Assert.AreEqual(1, query.Count());
        }
    }
}
