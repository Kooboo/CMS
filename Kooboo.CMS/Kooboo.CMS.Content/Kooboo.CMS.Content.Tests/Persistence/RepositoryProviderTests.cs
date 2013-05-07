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
    public class RepositoryProviderTests
    {
        string repositoryName = "RepositoryProviderTests";
        Repository contentRepository = null;
        public RepositoryProviderTests()
        {
            contentRepository = new Repository(repositoryName) { DisplayName = "DisplayName" };
        }
        
        [TestMethod]
        public void Test1()
        {
            RepositoryProvider repository = new RepositoryProvider();
            repository.Add(contentRepository);
            var item = repository.Get(contentRepository);
            Assert.AreEqual(contentRepository.Name, item.Name);
            Assert.AreEqual(contentRepository.DisplayName, item.DisplayName);

            repository.Remove(item);
        }
    }
}
