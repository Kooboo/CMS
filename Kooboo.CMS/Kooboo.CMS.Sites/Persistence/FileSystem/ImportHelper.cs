using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Ionic.Zip;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public static class ImportHelper
    {
        public static void Export(IEnumerable<PathResource> sources, Stream outputStream)
        {
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {

                foreach (var item in sources)
                {
                    if (item.Exists())
                    {
                        if (item is DirectoryResource)
                        {
                            zipFile.AddDirectory(item.PhysicalPath, item.Name);
                        }
                        else
                        {
                            zipFile.AddFile(item.PhysicalPath, "");
                        }
                    }
                }

                zipFile.Save(outputStream);
            }
        }

        public static void Import(Site site, string destDir, Stream zipStream, bool @override)
        {
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.DoNotOverwrite;
                if (@override)
                {
                    action = ExtractExistingFileAction.OverwriteSilently;
                }
                zipFile.ExtractAll(destDir, action);
            }
        }

        public static void ImportData<T>(Site site, IProvider<T> provider, string fileName, Stream zipStream, bool @override)
            where T : IPersistable
        {
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                if (zipFile.ContainsEntry(fileName))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var entry = zipFile[fileName];
                        entry.Extract(ms);
                        ms.Position = 0;
                        var list = Deserialize<List<T>>(ms, null);
                        foreach (var item in list)
                        {
                            item.Site = site;
                            var o = provider.Get(item);
                            if (o != null && @override)
                            {
                                provider.Update(item, o);
                            }
                            if (o == null)
                            {
                                provider.Add(item);
                            }
                        }
                    }
                }
            }
        }
        private static T Deserialize<T>(Stream stream, IEnumerable<Type> knownTypes)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);

            return (T)ser.ReadObject(stream);
        }
    }
}
