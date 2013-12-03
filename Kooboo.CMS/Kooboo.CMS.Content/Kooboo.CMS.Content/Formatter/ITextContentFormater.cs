using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Formatter
{
    public interface ITextContentFormater
    {
        string Name { get; }
        string DisplayName { get; }
        string FileExtension { get; }
        void Export(IEnumerable<TextContent> textContents, Stream outputStream);
        void Import(TextFolder textFolder, Stream inputStream);
    }
}
