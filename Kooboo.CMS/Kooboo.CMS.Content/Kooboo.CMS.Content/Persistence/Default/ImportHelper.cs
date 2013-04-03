using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public static class ImportHelper
    {

        public static void Import(string basePath, Stream stream, bool @override)
        {
            using (ZipFile zipFile = ZipFile.Read(stream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.DoNotOverwrite;
                if (@override)
                {
                    action = ExtractExistingFileAction.OverwriteSilently;
                }
                zipFile.ExtractAll(basePath, action);
                stream.Position = 0;
            }
        }

        public static void Export(IEnumerable<string> paths, Stream stream)
        {
            using (ZipFile zipFile = new ZipFile())
            {
                foreach (var p in paths)
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(p);

                    if ((di.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        zipFile.AddDirectory(p, di.Name);
                    }
                    else
                    {
                        zipFile.AddFile(p, "");
                    }
                }
                zipFile.Save(stream);
            }
        }
    }
}
