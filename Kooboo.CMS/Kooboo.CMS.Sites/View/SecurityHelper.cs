using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace Kooboo.CMS.Sites.View
{
    public static class SecurityHelper
    {
        #region Encrypt/Decrypt
        /// <summary>
        /// Encrypts the specified original string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The string which needs to be encrypted can not be null.</exception>
        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                return originalString;
            }
            var key = GetKey();
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        /// <summary>
        /// Decrypts the specified crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The string which needs to be decrypted can not be null.</exception>
        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                return cryptedString;
            }
            var key = GetKey();
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(key, key), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
        private static byte[] GetKey()
        {
            var site = Site.Current;
            if (site == null)
            {
                throw new ArgumentNullException("SecurityHelper requires the Site.Current context.".Localize());
            }
            return ASCIIEncoding.ASCII.GetBytes(site.Security.EncryptKey);
        }
        #endregion

        #region IsSubmissionServiceAvailable
        public static bool IsSubmissionServiceAvailable()
        {
            if (Site.Current == null)
            {
                return false;
            }
            return Site.Current.Security.TurnOnSubmissionAPI;
        }
        #endregion
    }
}
