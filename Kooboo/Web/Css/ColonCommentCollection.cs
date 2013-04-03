using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    /// <example>
    /// /*
    ///  * key1: value1
    ///  * key2: value2
    ///  */
    /// </example>
    public class ColonCommentDictionary : Dictionary<string, string>
    {
        public ColonCommentDictionary()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public static ColonCommentDictionary Parse(string comment)
        {
            var result = new ColonCommentDictionary();

            // Remove /* and */
            var str = comment.Substring(2, comment.Length - 4);

            using (var reader = new StringReader(str))
            {
                string line = null;
                string key = null;
                StringBuilder value = new StringBuilder();
                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    // Remove *
                    line = line.Trim().TrimStart('*').Trim();
                    var index = line.IndexOf(':');
                    if (index == 0)
                    {
                        // It's a following line
                        value.AppendLine().Append(line);
                    }
                    else
                    {
                        // It's a key value start line

                        // Append previous key/value
                        if (key != null)
                        {
                            result.Add(key, value.ToString());
                        }

                        key = line.Substring(0, index).Trim();
                        value.Clear();
                        value.Append(line.Substring(index + 1).Trim());
                    }
                }

                // Append final key/value
                if (key != null)
                {
                    result.Add(key, value.ToString());
                }
            }

            return result;
        }
    }
}
