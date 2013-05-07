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
using Kooboo.Extensions;
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
            //#warning sqlce test...
            //            return userKey;
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = GetColumnValueForUserKey(content);
                if (!string.IsNullOrEmpty(userKey))
                {
                    Repository repository = content.GetRepository().AsActual();
                    userKey = TrimUserKey(userKey, repository.UserKeyReplacePattern, repository.UserKeyHyphens);
                }
            }
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = content.UUID;
            }
            else
            {
                if (userKey.Length > 90)
                {
                    userKey = userKey.Substring(0, 90);
                }
                int tries = 0;
                string tmpUserKey = userKey.StripAllTags();
                while (IfUserKeyExists(content, tmpUserKey))
                {
                    tries++;
                    tmpUserKey = userKey + "-" + tries.ToString();
                }
                userKey = tmpUserKey;
            }

            return userKey;
        }
        protected virtual string TrimUserKey(string userKey, string replacePattern, string hyphens)
        {
            userKey = Regex.Replace(userKey, replacePattern, hyphens);
            return userKey;
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
