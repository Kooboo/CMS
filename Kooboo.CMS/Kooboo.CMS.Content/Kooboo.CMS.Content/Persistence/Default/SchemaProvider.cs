using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.Web.Url;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Form;
using Ionic.Zip;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public class SchemaProvider : FileSystemProviderBase<Schema>, ISchemaProvider
    {
        protected override IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[]{
                typeof(ColumnValidation),
                typeof(RequiredValidation),
                typeof(StringLengthValidation),
                typeof(RangeValidation),
                typeof(RegexValidation),
                };
            }
        }
        #region IRepository<Schema> Members

        public IQueryable<Schema> All(Repository repository)
        {
            return AllEnumerable(repository).AsQueryable();
        }
        public IEnumerable<Schema> AllEnumerable(Repository repository)
        {
            var baseDir = SchemaPath.GetBaseDir(repository);
            List<Schema> list = new List<Schema>();
            if (Directory.Exists(baseDir))
            {
                foreach (var item in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    list.Add(new Schema(repository, item.Name));
                }
            }
            return list;
        }
        public override void Remove(Schema item)
        {
            base.Remove(item);
            try
            {
                var file = item.GetContentFile();
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch
            {
            }

        }
        public override void Add(Schema item)
        {
            base.Add(item);
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Initialize(item);
        }
        #endregion

        #region IImportProvider Members

        public virtual void Export(Repository repository, IEnumerable<Schema> models, Stream outputStream)
        {
            var list = models;
            GetLocker().EnterReadLock();
            try
            {
                ImportHelper.Export(list.Select(it => new SchemaPath(it).PhysicalPath), outputStream);
            }
            finally
            {
                GetLocker().ExitReadLock();
            }

        }

        public virtual void Import(Repository repository, Stream zipStream, bool @override)
        {
            GetLocker().EnterWriteLock();
            try
            {
                ImportHelper.Import(SchemaPath.GetBaseDir(repository), zipStream, @override);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }

        #endregion

        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

        #region Initialize,ImportData,ExportData
        public virtual void Initialize(Schema schema)
        {
            //do nothing
        }
        #endregion


        public virtual Schema Create(Repository repository, string schemaName, Stream templateStream)
        {
            Schema schema = new Schema(repository, schemaName);
            SchemaPath path = new SchemaPath(schema);
            if (path.Exists())
            {
                throw new KoobooException("The item is already exists.");
            }
            GetLocker().EnterReadLock();
            try
            {
                using (ZipFile zipFile = ZipFile.Read(templateStream))
                {
                    ExtractExistingFileAction action = ExtractExistingFileAction.OverwriteSilently;
                    zipFile.ExtractAll(path.PhysicalPath, action);
                }
                Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Initialize(schema);
            }
            finally
            {
                GetLocker().ExitReadLock();
            }
            return schema;
        }


        public Schema Copy(Repository repository, string sourceName, string destName)
        {

            SchemaPath sourcePath = new SchemaPath(new Schema(repository, sourceName));

            var destSchema = new Schema(repository, destName);

            var destPath = new SchemaPath(destSchema);



            Kooboo.IO.IOUtility.CopyDirectory(sourcePath.PhysicalPath, destPath.PhysicalPath);


            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Initialize(destSchema);

            return destSchema;

        }
    }
}
