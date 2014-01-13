using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    /// <summary>
    /// Encode Container name and Blob name.
    /// </summary>
    public static class StorageNamesEncoder
    {
        /// <summary>
        /// Container names must start with a letter or number, and can contain only letters, numbers, and the dash (-) character.
        /// Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted in container names.
        /// All letters in a container name must be lowercase.
        /// Container names must be from 3 through 63 characters long.
        /// </summary>
        public static string EncodeContainerName(string source)
        {
            var container = source.ToLower();

            if (container.Length < 3)
            {
                container = "container-" + container;
            }
            if (container.Length > 63)
            {
                container = container.Substring(0, 63);
            }

            Regex r = new Regex(@"[^a-z0-9]", RegexOptions.Compiled);
            container = r.Replace(container, "-");

            container = container.Trim('-');

            r = new Regex(@"[-]{2,}", RegexOptions.Compiled);
            container = r.Replace(container, @"-");

            return container;

        }

        //public static string DecodeContainerName(string source)
        //{
        //    return source;
        //}

        public static string EncodeBlobName(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            return Uri.EscapeUriString(source);
            //return HttpUtility.UrlEncode(source);
        }

        public static string DecodeBlobName(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            return Uri.UnescapeDataString(source);
            //return HttpUtility.UrlDecode(source);
        }
    }
}
