using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

using System.IO;
using System.ComponentModel.Composition;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public class ScriptProvider : IScriptProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        public IQueryable<ScriptFile> All(Site site)
        {
            return AllEnumerable(site).AsQueryable();
        }
        public IEnumerable<ScriptFile> AllEnumerable(Site site)
        {
            var fileNames = ScriptsWithParent(site);

            fileNames = FileOrderHelper.OrderFiles(GetOrderFile(site), fileNames);

            return fileNames.Select(it => new ScriptFile(site, it).LastVersion()).ToArray();
        }
        private IEnumerable<string> ScriptsWithParent(Site site)
        {
            List<string> results = new List<string>();

            while (site != null)
            {
                var baseDir = GetScriptBasePath(site);
                if (Directory.Exists(baseDir))
                {
                    var tempResults = EnumerateScripts(baseDir);
                    if (results.Count == 0)
                    {
                        results.AddRange(tempResults);
                    }
                    else
                    {
                        foreach (var item in tempResults)
                        {
                            if (!results.Any(it => Path.GetFileName(it).Equals(Path.GetFileName(item), StringComparison.InvariantCultureIgnoreCase)))
                            {
                                results.Add(item);
                            }
                        }
                    }
                }
                site = site.Parent;
            }
            return results;
        }
        private IEnumerable<string> EnumerateScripts(string baseDir)
        {
            List<string> list = new List<string>();
            foreach (var file in Directory.EnumerateFiles(baseDir, "*.js"))
            {
                list.Add(Path.GetFileName(file));
            }
            return list;
        }
        private string GetOrderFile(Site site)
        {
            while (site != null)
            {
                var orderFile = FileOrderHelper.GetOrderFile(GetScriptBasePath(site));
                if (File.Exists(orderFile))
                {
                    return orderFile;
                }
                site = site.Parent;
            }
            return null;
        }

        protected void Save(ScriptFile item)
        {
            item.Save();
        }

        #region IRepository<ScriptFile> Members


        public ScriptFile Get(ScriptFile dummy)
        {
            throw new NotImplementedException();
        }

        public void Add(ScriptFile item)
        {
            Save(item);
        }

        public void Update(ScriptFile @new, ScriptFile old)
        {
            Save(@new);
        }

        public void Remove(ScriptFile item)
        {
            if (item.Exists())
            {
                item.Delete();
            }
        }

        #endregion

        #region IImportRepository Members

        public void Export(IEnumerable<ScriptFile> sources, System.IO.Stream outputStream)
        {
            ImportHelper.Export(sources, outputStream);
        }
        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            ImportHelper.Import(site, (new ScriptFile(site, "dummy.js")).BasePhysicalPath, zipStream, @override);
        }
        #endregion

        public void SaveOrders(Site site, IEnumerable<string> filesOrder)
        {
            var baseDir = GetScriptBasePath(site);
            FileOrderHelper.SaveFilesOrder(baseDir, filesOrder);

        }

        private string GetScriptBasePath(Site site)
        {
            ScriptFile dummy = ModelActivatorFactory<ScriptFile>.GetActivator().CreateDummy(site);
            return dummy.BasePhysicalPath;
        }

        public void Localize(ScriptFile file, Site targetSite)
        {
            var targetFile = new ScriptFile(targetSite, file.FileName);
            targetFile.Body = file.Read();
            targetFile.Save();
        }
    }
}
