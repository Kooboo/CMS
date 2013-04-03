using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class Comment : Node
    {
        public const string StartToken = "/*";

        public const string EndToken = "*/";

        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public static Comment Parse(string str)
        {
            return new Comment
            {
                Text = str
            };
        }
    }
}
