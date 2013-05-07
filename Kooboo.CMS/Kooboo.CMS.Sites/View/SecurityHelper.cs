using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.View
{
    public static class SecurityHelper
    {
        #region Encrypt/Decrypt
        public static string Encrypt(string plainText)
        {
            return Encrypt(Site.Current, plainText);
        }
        /// <summary>
        /// Encrypts the specified original string.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="plainText">The plain string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The string which needs to be encrypted can not be null.</exception>
        public static string Encrypt(Site site, string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return plainText;
            }
            var key = GetKey(site);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(plainText);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        public static string Decrypt(string cryptedString)
        {
            return Decrypt(Site.Current, cryptedString);
        }
        public static string Decrypt(Site site, string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                return cryptedString;
            }
            var key = GetKey(site.AsActual());
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(key, key), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
        private static byte[] GetKey(Site site)
        {
            if (site == null)
            {
                throw new ArgumentNullException("SecurityHelper requires the Site.Current context.".Localize());
            }
            return ASCIIEncoding.ASCII.GetBytes(site.Security.EncryptKey);
        }
        #endregion
    }
}
