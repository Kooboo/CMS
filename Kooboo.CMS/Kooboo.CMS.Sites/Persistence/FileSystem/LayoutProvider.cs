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
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.Composition;
using System.IO;
using Kooboo.Web.Url;
using Kooboo.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILayoutProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Layout>))]
    public class LayoutProvider : TemplateProvider<Layout>, ILayoutProvider
    {
        public class LayoutVersionLogger : TemplateProvider<Layout>.TemplateVersionLogger
        {
            static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
            protected override TemplateProvider<Layout> GetTemplateProvider()
            {
                return new LayoutProvider();
            }
            public override void Revert(Layout o, int version, string userName)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    versionData.UserName = userName;
                    versionData.LastUpdateDate = DateTime.UtcNow;
                    Providers.LayoutProvider.Update(versionData, o);
                    //log a new version when revert
                    LogVersion(versionData);
                }
            }

            protected override System.Threading.ReaderWriterLockSlim GetLocker()
            {
                return locker;
            }
        }
        static LayoutProvider()
        {

        }
        #region Layout

        public void Localize(Layout o, Site targetSite)
        {
            ILocalizableHelper.Localize<Layout>(o, targetSite);
        }
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        //#region LayoutSample

        //public class LayoutSamplePath
        //{
        //    static LayoutSamplePath()
        //    {
        //        PhysicalPath = Kooboo.Web.Mvc.AreaHelpers.CombineAreaFilePhysicalPath("Sites", "Samples", "Layout");
        //        VirtualPath = Kooboo.Web.Mvc.AreaHelpers.CombineAreaFileVirtualPath("Sites", "Samples", "Layout");
        //    }
        //    public static string PhysicalPath { get; private set; }
        //    public static string VirtualPath { get; private set; }
        //}
        //public IEnumerable<LayoutSample> AllSamples()
        //{
        //    var basePath = LayoutSamplePath.PhysicalPath;
        //    if (Directory.Exists(basePath))
        //    {
        //        foreach (var item in Directory.EnumerateDirectories(basePath))
        //        {
        //            if (((new DirectoryInfo(Path.Combine(LayoutSamplePath.PhysicalPath, item)).Attributes) & FileAttributes.Hidden) != FileAttributes.Hidden)
        //            {
        //                yield return GetLayoutSample(Path.GetFileNameWithoutExtension(item));
        //            }
        //        }
        //    }
        //}
        //public LayoutSample GetLayoutSample(string name)
        //{
        //    string templateFile = Path.Combine(LayoutSamplePath.PhysicalPath, name, "template.aspx");
        //    string thumbnailFile = Path.Combine(LayoutSamplePath.PhysicalPath, name, "thumbnail.png");
        //    LayoutSample sample = new LayoutSample() { Name = name };
        //    if (File.Exists(templateFile))
        //    {
        //        sample.Template = Kooboo.IO.IOUtility.ReadAsString(templateFile);
        //    }
        //    if (File.Exists(thumbnailFile))
        //    {
        //        sample.ThumbnailVirtualPath = UrlUtility.Combine(LayoutSamplePath.VirtualPath, name, "thumbnail.png");
        //    }
        //    return sample;
        //}

        //#endregion

        public Layout Copy(Site site, string sourceName, string destName)
        {
            GetLocker().EnterWriteLock();

            try
            {
                var sourceLayout = new Layout(site, sourceName).LastVersion();
                var destLayout = new Layout(site, destName);

                IOUtility.CopyDirectory(sourceLayout.PhysicalPath, destLayout.PhysicalPath);

                return destLayout;
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
    }
}
