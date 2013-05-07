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
using Kooboo.CMS.Content.Persistence.SqlServer;
using Kooboo.CMS.Content.Query;
using System.IO;
namespace Kooboo.CMS.Content.Persistence.SqlServer.Tests
{
    /// <summary>
    /// Summary description for BinaryContentProviderTests
    /// </summary>
    [TestClass]
    public class MediaContentProviderTests
    {
        Repository repository;
        MediaFolder binaryFolder;
        IMediaContentProvider binaryContentProvider;

        public MediaContentProviderTests()
        {
            Providers.DefaultProviderFactory = new ProviderFactory();

            repository = new Repository("MediaContentProviderTests");
            Providers.RepositoryProvider.Add(repository);

            binaryFolder = new MediaFolder(repository, "images");
            Providers.DefaultProviderFactory.GetProvider<IMediaFolderProvider>().Add(binaryFolder);

            binaryContentProvider = Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>();

        }
        [TestMethod]
        public void TestSimple()
        {
            //var binaryContentProvider = Providers.GetProviderFactory(repository.DBProvider).GetProvider<IContentProvider<BinaryContent>>();

            var binaryContent1 = new MediaContent(repository.Name, binaryFolder.Name);
            binaryContent1.UserKey = "image1";
            binaryContent1.VirtualPath = "~/binary/image1.jpg";
            binaryContent1.ContentFile = new ContentFile()
            {
                Name = "image1.jpg",
                FileName = "image1.jpg",
                Stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6 })
            };
            binaryContentProvider.Add(binaryContent1);

            var binaryContent2 = new MediaContent(repository.Name, binaryFolder.Name);
            binaryContent2.UserKey = "image2";
            binaryContent2.VirtualPath = "~/binary/image2.jpg";
            binaryContent2.ContentFile = new ContentFile()
            {
                Name = "image2.jpg",
                FileName = "image2.jpg",
                Stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6 })
            };
            binaryContentProvider.Add(binaryContent2);

            var query1 = binaryFolder.CreateQuery().WhereEquals("UUID", (string)(binaryContent1.UUID));
            Assert.AreEqual(1, query1.Count());
            var binaryContentItem1 = query1.First();
            Assert.AreEqual(binaryContent1.UUID, binaryContentItem1.UUID);

        }
    }
}
