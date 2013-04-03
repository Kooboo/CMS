using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.Content.Persistence.AzureBlobService.Tests
{
    [TestClass]
    public class MediaContentProviderTest
    {
        MediaContentProvider provider = new MediaContentProvider();
        Repository repository = null;
        MediaFolder mediaFolder = null;
        public MediaContentProviderTest()
        {
            Kooboo.CMS.Content.Persistence.AzureBlobService.AssemblyInitializer.ResetProviders();

            repository = new Repository(Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(5));
            mediaFolder = new MediaFolder(repository, "MediaFolder1/MediaFolder2");

            Kooboo.CMS.Content.Persistence.Providers.DefaultProviderFactory.GetProvider<IRepositoryProvider>().Initialize(repository);
        }

        [TestMethod]
        public void Test_Add_Content()
        {
            MediaContent content = new MediaContent(repository.Name, mediaFolder.FullName);
            content.Published = true;
            content.ContentFile = new ContentFile()
            {
                FileName = "Test_Add_Content.dat",
                Stream = new MemoryStream(new byte[10])
            };
            provider.Add(content);
            Console.WriteLine(content.VirtualPath);
        }

        [TestMethod]
        public void Test_Update_Content()
        {
            MediaContent content = new MediaContent(repository.Name, mediaFolder.FullName);
            content.Published = true;
            content.ContentFile = new ContentFile()
            {
                FileName = "Test_Update_Content.png",
                Stream = new MemoryStream(new byte[10])
            };
            provider.Add(content);

            content.ContentFile = new ContentFile()
            {
                FileName = "Test_Update_Content.png",
                Stream = new MemoryStream(new byte[20])
            };
            provider.Update(content, content);
        }
        [TestMethod]
        public void Test_Delete_Content()
        {
            MediaContent content = new MediaContent(repository.Name, mediaFolder.FullName);
            content.Published = true;
            content.ContentFile = new ContentFile()
            {
                FileName = "Test_Delete_Content.zip",
                Stream = new MemoryStream(new byte[10])
            };
            provider.Add(content);

            provider.Delete(content);
        }
        [TestMethod]
        public void Test_Get_Content()
        {
            MediaContent content = new MediaContent(repository.Name, mediaFolder.FullName);
            content.Published = true;
            content.UserId = "admin";
            content.ContentFile = new ContentFile()
            {
                FileName = "Test_Get_Content.zip",
                Stream = new MemoryStream(new byte[10])
            };
            provider.Add(content);

            var gotContent = mediaFolder.CreateQuery().WhereEquals("FileName", content.FileName).First();
            Assert.AreEqual(content.FileName, gotContent.FileName);
            Assert.AreEqual(content.UserId, gotContent.UserId);
            Assert.AreEqual(content.Published, gotContent.Published);
            Console.WriteLine(content.VirtualPath, gotContent.VirtualPath);
        }
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_Only_Support_QueryByFileName()
        {
            mediaFolder.CreateQuery().WhereEquals("VirtualPath", "abc.zip").First();
        }
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_Only_Support_WhereEquals_WhereStartsWith()
        {
            mediaFolder.CreateQuery().WhereContains("VirtualPath", "abc.zip").First();
        }
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_Not_Support_MultiCondition()
        {
            mediaFolder.CreateQuery().WhereEquals("FileName", "abc.zip").WhereStartsWith("FileName", "abc.zip").First();
        }
    }
}
