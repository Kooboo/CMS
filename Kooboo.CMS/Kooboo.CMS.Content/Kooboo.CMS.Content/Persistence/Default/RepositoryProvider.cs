using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Ionic.Zip;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Query;
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Content.Persistence.Default
{
    public class RepositoryProvider : FileSystemProviderBase<Repository>, IRepositoryProvider
    {
        static string Offline_File = "offline.txt";

        public override void Remove(Repository item)
        {
            Offline(item);

            base.Remove(item);
        }

        #region i Members

        public virtual IQueryable<Models.Repository> All()
        {
            return AllEnumerable().AsQueryable();
        }
        public virtual IEnumerable<Models.Repository> AllEnumerable()
        {
            List<Repository> list = new List<Repository>();
            var baseDir = RepositoryPath.BasePhysicalPath;
            if (Directory.Exists(baseDir))
            {
                foreach (var dir in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    if (File.Exists(Path.Combine(dir.FullName, PathHelper.SettingFileName)))
                    {
                        list.Add(new Repository(dir.Name));
                    }
                }
            }
            return list;
        }
        public override void Add(Repository item)
        {
            base.Add(item);

        }

        #endregion

        #region IRepository<Repository> Members

        public virtual IQueryable<Repository> All(Repository repository)
        {
            return All();
        }

        #endregion

        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

        #region Initialize && Export

        public virtual Repository Create(string repositoryName, Stream packageStream)
        {
            Repository repository = new Repository(repositoryName);
            RepositoryPath path = new RepositoryPath(repository);
            if (path.Exists())
            {
                throw new KoobooException("The item is already exists.");
            }
            using (ZipFile zipFile = ZipFile.Read(packageStream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.OverwriteSilently;
                zipFile.ExtractAll(path.PhysicalPath, action);

                AdjustContentValue(repository);
            }
            return repository;
        }
        private void AdjustContentValue(Repository repository)
        {
            ISchemaProvider xmlSchemaProvider = new SchemaProvider();
            foreach (var schema in xmlSchemaProvider.All(repository))
            {
                ReplaceContentFileVirtualPath(repository, schema);
            }
        }
        public static Regex replaceVirtualPathRegex = new Regex("~/Cms_Data/Contents/[^/]+/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static string virtualPathFormat = "~/Cms_Data/Contents/{0}/";
        private void ReplaceContentFileVirtualPath(Repository repository, Schema schema)
        {
            var schemaDataFile = schema.GetContentFile();
            if (File.Exists(schemaDataFile))
            {
                string data = IO.IOUtility.ReadAsString(schemaDataFile);
                string newData = replaceVirtualPathRegex.Replace(data, string.Format(virtualPathFormat, repository));
                IO.IOUtility.SaveStringToFile(schemaDataFile, newData);
            }
        }

        public virtual void Initialize(Repository repository)
        {
            ITextContentProvider textContentProvider = Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>();
            Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>().InitializeMediaContents(repository);
            ISchemaProvider schemaProvider = Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>();

            //if (!(textContentProvider is TextContentProvider))
            //{
            ISchemaProvider xmlSchemaProvider = new SchemaProvider();
            ITextContentProvider xmlContentProvider = new TextContentProvider();
            foreach (var schema in xmlSchemaProvider.All(repository))
            {

                schemaProvider.Initialize(schema);

                textContentProvider.ImportSchemaData(schema, xmlContentProvider.ExportSchemaData(schema));
            }

            textContentProvider.ImportCategoryData(repository, xmlContentProvider.ExportCategoryData(repository));
            //}
        }

        public virtual void Export(Repository repository, Stream outputStream)
        {
            IRepositoryProvider repositoryProvider = Providers.DefaultProviderFactory.GetProvider<IRepositoryProvider>();

            BackupContentAsXML(repository);

            repositoryProvider.Offline(repository);
            try
            {
                RepositoryPath path = new RepositoryPath(repository);
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    //zipFile.ZipError += new EventHandler<ZipErrorEventArgs>(zipFile_ZipError);

                    zipFile.ZipErrorAction = ZipErrorAction.Skip;


                    zipFile.AddSelectedFiles("name != *\\~versions\\*.* and name != *\\.svn\\*.* and name != *\\_svn\\*.*", path.PhysicalPath, "", true);

                    zipFile.Save(outputStream);
                }
            }
            finally
            {
                repositoryProvider.Online(repository);
            }
        }

        private static void BackupContentAsXML(Repository repository)
        {
            ITextContentProvider textContentProvider = Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>();
            ISchemaProvider schemaProvider = Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>();
            if (!(textContentProvider is TextContentProvider))
            {
                ITextContentProvider xmlTextContentProvider = new TextContentProvider();

                foreach (var schema in schemaProvider.All(repository))
                {
                    xmlTextContentProvider.ImportSchemaData(schema, textContentProvider.ExportSchemaData(schema));
                }

                xmlTextContentProvider.ImportCategoryData(repository, textContentProvider.ExportCategoryData(repository));
            }
        }

        #endregion

        #region Online/Offline
        public virtual void Offline(Repository repository)
        {
            var offlineFile = GetOfflineFile(repository);
            if (!File.Exists(offlineFile))
            {
                File.WriteAllText(offlineFile, "The repository is offline, please remove this file to take it online.".Localize());
            }
        }
        private string GetOfflineFile(Repository repository)
        {
            RepositoryPath path = new RepositoryPath(repository);
            return Path.Combine(path.PhysicalPath, Offline_File);
        }
        public virtual void Online(Repository repository)
        {
            var offlineFile = GetOfflineFile(repository);
            if (File.Exists(offlineFile))
            {
                File.Delete(offlineFile);
            }
        }

        public virtual bool IsOnline(Repository repository)
        {
            var offlineFile = GetOfflineFile(repository);
            return File.Exists(offlineFile);
        }
        #endregion


        public virtual Repository Copy(Repository sourceRepository, string destRepositoryName)
        {
            var sourcePath = new RepositoryPath(sourceRepository);
            var destRepository = new Repository(destRepositoryName);
            var destPath = new RepositoryPath(destRepository);

            GetLocker().EnterWriteLock();
            try
            {
                BackupContentAsXML(sourceRepository);
                IO.IOUtility.CopyDirectory(sourcePath.PhysicalPath, destPath.PhysicalPath);
                Initialize(destRepository);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
            return destRepository;
        }


        public virtual bool TestDbConnection()
        {
            return true;
        }
    }
}
