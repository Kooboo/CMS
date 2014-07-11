#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Query;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// UUID生成器
    /// </summary>
    public class UUIDGenerator
    {
        static UUIDGenerator()
        {
            DefaultGenerator = new UUIDGenerator();
        }
        public static UUIDGenerator DefaultGenerator { get; set; }

        public virtual string Generate(ContentBase content)
        {
            return UniqueIdGenerator.GetInstance().GetBase32UniqueId(16);
        }
    }
    /// <summary>
    /// UserKey生成器
    /// </summary>
    public class UserKeyGenerator
    {
        static UserKeyGenerator()
        {
            DefaultGenerator = new UserKeyGenerator();
        }
        public static UserKeyGenerator DefaultGenerator { get; set; }
        public virtual string Generate(ContentBase content)
        {
            string userKey = content.UserKey;
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = GetColumnValueForUserKey(content);
            }
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = content.UUID;
            }
            else
            {
                if (userKey.Length > 256)
                {
                    userKey = userKey.Substring(0, 256);
                }

               var escapedUserKey = EscapeUserKey(content, userKey);

               var tmpUserKey = escapedUserKey;

                int tries = 0;
                while (IfUserKeyExists(content, tmpUserKey))
                {
                    tries++;
                    tmpUserKey = escapedUserKey + "-" + UniqueIdGenerator.GetInstance().GetBase32UniqueId(tries);
                }
                userKey = tmpUserKey;
            }

            return userKey;
        }
        protected virtual string EscapeUserKey(ContentBase content, string userKey)
        {
            string tmpUserKey = userKey.StripAllTags();

            //http://stackoverflow.com/questions/9565360/how-to-convert-utf-8-characters-to-ascii-for-use-in-a-url/9628594#9628594
            tmpUserKey = RemoveDiacritics(tmpUserKey);

            Repository repository = content.GetRepository().AsActual();
            tmpUserKey = Regex.Replace(tmpUserKey, repository.UserKeyReplacePattern, repository.UserKeyHyphens);

            return tmpUserKey;
        }
        protected virtual string RemoveDiacritics(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            string normalized = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            Encoding nonunicode = Encoding.GetEncoding(850);
            Encoding unicode = Encoding.Unicode;

            byte[] nonunicodeBytes = Encoding.Convert(unicode, nonunicode, unicode.GetBytes(sb.ToString()));
            char[] nonunicodeChars = new char[nonunicode.GetCharCount(nonunicodeBytes, 0, nonunicodeBytes.Length)];
            nonunicode.GetChars(nonunicodeBytes, 0, nonunicodeBytes.Length, nonunicodeChars, 0);

            return new string(nonunicodeChars);
        }
        protected virtual string GetColumnValueForUserKey(ContentBase content)
        {
            if (content is TextContent)
            {
                var textContent = (TextContent)content;
                var repository = new Repository(textContent.Repository);
                var schema = new Schema(repository, textContent.SchemaName).AsActual();
                var summarizeField = schema.Columns.Where(it => it.Summarize == true).FirstOrDefault();
                if (summarizeField == null || textContent[summarizeField.Name] == null)
                {
                    return textContent.UUID;
                }
                else
                {
                    return textContent[summarizeField.Name].ToString();
                }
            }
            else if (content is MediaContent)
            {
                return ((MediaContent)content).FileName;
            }
            return null;
        }
        protected virtual bool IfUserKeyExists(ContentBase content, string userKey)
        {
            var repository = new Repository(content.Repository);
            if (content is TextContent)
            {
                var textContent = (TextContent)content;
                var schema = new Schema(repository, textContent.SchemaName);
                var contentExists = schema.CreateQuery().WhereEquals("UserKey", userKey).FirstOrDefault();
                if (contentExists != null)
                {
                    return contentExists.UUID != content.UUID;
                }
                return false;
            }
            else if (content is MediaContent)
            {
                var mediaContent = (MediaContent)content;
                var folder = new MediaFolder(repository, mediaContent.FolderName);
                var contentExists = folder.CreateQuery().WhereEquals("UserKey", userKey).FirstOrDefault();
                if (contentExists != null)
                {
                    return contentExists.UUID != content.UUID;
                }
                return false;
            }
            return false;
        }
    }
}
