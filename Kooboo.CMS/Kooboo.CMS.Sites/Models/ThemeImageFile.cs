using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class ThemeImageFile : ThemeFile
    {
        public ThemeImageFile(string physicalPath)
            : base(physicalPath)
        {
        }
        public ThemeImageFile(Theme theme, string fileName)
            : base(theme, fileName)
        {
        }

        const string PATH_NAME = "Images";

        public override IEnumerable<string> RelativePaths
        {
            get
            {
                //
                return Theme.RelativePaths.Concat(new string[] { Theme.Name, PATH_NAME });
            }
        }

        internal override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            var list = relativePaths.ToList();
            list.RemoveAt(relativePaths.Count() - 2);
            //call base return {"site1","themes","default"}
            return base.ParseObject(list);

        }
    }
}
