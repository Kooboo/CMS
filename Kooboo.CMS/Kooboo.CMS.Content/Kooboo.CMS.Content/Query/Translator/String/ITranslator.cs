using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Query.Translator.String
{
    public interface ITranslator<T>
        where T : ContentBase
    {
        TranslatedQuery Translate(IContentQuery<T> contentQuery);
    }
}
