using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    /// <summary>
    /// Encode Container name and Blob name.
    /// </summary>
    public static class StorageNamesEncoder
    {
        /// <summary>
        /// Now we use the simple hex encode to get a standard container name.
        /// Container names must start with a letter or number, and can contain only letters, numbers, and the dash (-) character.
        /// Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted in container names.
        /// All letters in a container name must be lowercase.
        /// Container names must be from 3 through 63 characters long.
        /// </summary>
        public static string EncodeContainerName(string source)
        {
            string dest = string.Empty;
            foreach (char c in source)
            {
                dest += String.Format("{0:X}", Convert.ToInt32(c));
            }

            return dest.ToLower();
        }

        public static string DecodeContainerName(string source)
        {
            string dest = string.Empty;
            for (int i = 0; i < source.Length; i += 2)
            {
                string hex = source.Substring(i, 2);
                dest += (char)Convert.ToInt32(hex, 16);
            }

            return dest;
        }

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
