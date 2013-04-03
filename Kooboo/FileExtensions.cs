using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public class FileExtensions
    {
        public const string Image = ".jpg,.jpeg,.gif,.png,.bmp";

        public static readonly string[] ImageArray = Image.Split(',');

        public const string Html = ".txt,.htm,.html";

        public static readonly string[] HtmlArray = Html.Split(',');

        public const string Css = ".css";

        public static readonly string[] CssArray = Css.Split(',');
    }
}
