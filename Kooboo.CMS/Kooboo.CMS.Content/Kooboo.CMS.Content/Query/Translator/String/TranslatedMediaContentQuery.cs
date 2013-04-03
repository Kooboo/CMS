using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Query.Translator.String
{
    public class TranslatedMediaContentQuery : TranslatedQuery
    {
        public TranslatedMediaContentQuery(Repository repository, MediaFolder mediaFolder)
        {
            this.Repository = repository;
            this.MediaFolder = mediaFolder;
        }
        public Repository Repository { get; private set; }
        public MediaFolder MediaFolder { get; private set; }

        public override string ToString()
        {
            return string.Format("[MediaContent] SELECT {0} FROM [{1}.{2}] WHERE {3} ORDER {4} | OP:{5} PageSize:{6} TOP:{7} Skip:{8} ",
                SelectText, Repository.Name, MediaFolder.FullName, ClauseText, SortText, CallType, TakeCount, Top, Skip);
        }
    }
}
