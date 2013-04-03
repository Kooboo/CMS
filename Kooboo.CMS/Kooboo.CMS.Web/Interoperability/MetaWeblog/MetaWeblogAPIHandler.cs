using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using CookComputing.MetaWeblog;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using Kooboo.IO;
using Kooboo.Web.Url;
using System.Collections.Specialized;
using CookComputing.Blogger;
using Kooboo.Globalization;

namespace Kooboo.CMS.Content.Interoperability.MetaWeblog
{
    public class MetaWeblogAPIHandler : XmlRpcService, IMetaWeblog, IBlogger
    {
        private static class MetaWeblogHelper
        {
            public static string CompositePostId(TextContent content)
            {
                return content.FolderName + "$$$" + content.UUID + "$$$" + content.UserKey;
            }
            public static string ParsePostId(string postId, out string folderName, out string userKey)
            {
                string[] s = postId.Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
                folderName = s[0];
                userKey = s[2];
                return s[1];
            }
        }
        private string repositoryName;
        public string RepositoryName
        {
            get
            {
                if (string.IsNullOrEmpty(repositoryName))
                {
                    return Context.Request.QueryString["repository"];
                }
                return repositoryName;
            }
            set
            {
                repositoryName = value;
            }
        }
        private Repository Repository
        {
            get
            {
                if (!string.IsNullOrEmpty(repositoryName))
                {
                    return new Repository(RepositoryName);
                }
                return Repository.Current;
            }
        }
        private void CheckUserPassword(string userName, string password)
        {
            if (Repository == null)
            {
                throw new KoobooException("The repository is null.".Localize());
            }
            if (!CMS.Account.Services.ServiceFactory.UserManager.ValidateUser(userName, password))
            {
                throw new KoobooException("The user or password is invalid.".Localize());
            }
            bool allow = false;
            if (Kooboo.CMS.Sites.Models.Site.Current != null)
            {
                allow = CMS.Sites.Services.ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, userName, CMS.Account.Models.Permission.Contents_ContentPermission);
            }
            else
            {
                allow = CMS.Sites.Services.ServiceFactory.UserManager.IsAdministrator(userName);
            }
            if (!allow)
            {
                throw new KoobooException("The user have no permission to edit content.".Localize());
            }
        }

        #region IMetaWeblog Members

        public object editPost(string postid, string username, string password, CookComputing.MetaWeblog.Post post, bool publish)
        {
            CheckUserPassword(username, password);
            string folderName;
            string userKey;
            string contentId = MetaWeblogHelper.ParsePostId(postid, out folderName, out userKey);
            var textFolder = new TextFolder(Repository, FolderHelper.SplitFullName(folderName));
            var content = textFolder.CreateQuery().WhereEquals("UUID", contentId).First();


            var values = new NameValueCollection();
            values["title"] = post.title;
            values["description"] = post.description;
            values["body"] = post.description;
            values["userKey"] = userKey;

            ServiceFactory.GetService<TextContentManager>().Update(Repository, textFolder, content.UUID, values);


            var old_categories = GetCategories(textFolder, content);

            var removedCategories = old_categories.Where(it => !post.categories.Any(c => string.Compare(c, it, true) == 0));
            var addedCategories = post.categories.Where(it => !old_categories.Any(c => string.Compare(c, it, true) == 0));

            var removed = GetCategories(textFolder, removedCategories).ToArray();
            if (removed.Length > 0)
            {
                ServiceFactory.GetService<TextContentManager>().RemoveCategories(Repository, (TextContent)content, removed);
            }

            var added = GetCategories(textFolder, addedCategories).ToArray();
            if (added.Length > 0)
            {
                ServiceFactory.GetService<TextContentManager>().AddCategories(Repository, (TextContent)content, added);
            }

            return MetaWeblogHelper.CompositePostId(content);
        }

        public CategoryInfo[] getCategories(string blogid, string username, string password)
        {
            CheckUserPassword(username, password);

            var folder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository, blogid).AsActual());
            var categories = GetFolderCategories(folder);

            return categories.Select(it => new CategoryInfo()
            {
                categoryid = MetaWeblogHelper.CompositePostId(it),
                description = it.GetSummary(),
                title = it.GetSummary(),
                htmlUrl = "",
                rssUrl = ""
            }).ToArray();


        }

        private IEnumerable<TextContent> GetFolderCategories(TextFolder folder)
        {
            folder = folder.AsActual();
            IEnumerable<TextContent> categories = new TextContent[0];
            if (folder.Categories != null)
            {
                foreach (var categoryFolderName in folder.Categories)
                {
                    var categoryFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository, categoryFolderName.FolderName).AsActual());
                    var contents = categoryFolder.CreateQuery();
                    categories = categories.Concat(contents).ToArray();
                }
            }

            return categories;
        }

        public CookComputing.MetaWeblog.Post getPost(string postid, string username, string password)
        {
            string folderName;
            string userKey;
            string contentId = MetaWeblogHelper.ParsePostId(postid, out folderName, out userKey);
            var textFolder = new TextFolder(Repository, FolderHelper.SplitFullName(folderName));
            var content = textFolder.CreateQuery().WhereEquals("UUID", contentId).First();
            return new CookComputing.MetaWeblog.Post()
            {
                postid = postid,
                categories = GetCategories(textFolder, content),
                description = content["description"] == null ? content["body"] == null ? "" : content["body"].ToString() : content["description"].ToString(),
                title = content["title"] == null ? "" : content["title"].ToString(),
                userid = content.UserId
            };
        }

        public CookComputing.MetaWeblog.Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            CheckUserPassword(username, password);
            var folder = new TextFolder(Repository, FolderHelper.SplitFullName(blogid));
            return folder.CreateQuery().Take(numberOfPosts).ToArray().Select(it => new CookComputing.MetaWeblog.Post()
            {
                categories = GetCategories(folder, it),
                dateCreated = it.UtcCreationDate,
                description = it["description"] == null ? "" : it["description"].ToString(),
                postid = it.UUID,
                title = it["title"] == null ? "" : it["title"].ToString(),
                userid = it.UserId
            }).ToArray();
        }
        private string[] GetCategories(TextFolder textFolder, TextContent content)
        {
            var folder = textFolder.AsActual();
            IEnumerable<string> categories = new string[0];
            if (folder.Categories != null)
            {
                foreach (var categoryFolderName in folder.Categories)
                {
                    var categoryFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository, categoryFolderName.FolderName).AsActual());
                    categories = categories.Concat(textFolder.CreateQuery().WhereEquals("UUID", content.UUID).Categories(categoryFolder).ToArray()
                            .Select(it => it.GetSummary()));
                }
            }
            return categories.ToArray();
        }

        private IEnumerable<TextContent> GetCategories(TextFolder textFolder, IEnumerable<string> categories)
        {
            return GetFolderCategories(textFolder).Where(it => categories.Any(c => string.Compare(c, it.GetSummary(), true) == 0));
        }

        public string newPost(string blogid, string username, string password, CookComputing.MetaWeblog.Post post, bool publish)
        {
            CheckUserPassword(username, password);

            var folder = (new TextFolder(Repository, FolderHelper.SplitFullName(blogid))).AsActual();

            var values = new NameValueCollection();
            values["title"] = post.title;
            values["description"] = post.description;
            values["body"] = post.description;
            values["published"] = publish.ToString();
            var content = ServiceFactory.GetService<TextContentManager>().Add(Repository, folder, values, null
                , GetCategories(folder, post.categories), username);

            var categories = GetCategories(folder, post.categories).ToArray();
            if (categories.Length > 0)
            {
                ServiceFactory.GetService<TextContentManager>().AddCategories(Repository, (TextContent)content, categories);
            }


            return MetaWeblogHelper.CompositePostId((TextContent)content);
        }

        public UrlData newMediaObject(string blogid, string username, string password, FileData file)
        {
            CheckUserPassword(username, password);

            var folder = new TextFolder(Repository, FolderHelper.SplitFullName(blogid));
            var metaweblogPath = new MetaWeblogPath(folder);
            var fileName = Path.GetFileName(file.name);
            var filePath = Path.Combine(metaweblogPath.PhysicalPath, fileName);
            Kooboo.IO.StreamExtensions.SaveAs(file.bits, filePath, true);
            Uri absoluteUri = new Uri(new Uri(Context.Request.Url.AbsoluteUri), UrlUtility.ResolveUrl(UrlUtility.Combine(metaweblogPath.VirtualPath, fileName)));

            return new UrlData() { url = absoluteUri.ToString() };

        }

        #endregion
        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            CheckUserPassword(username, password);

            string folderName = Context.Request.QueryString["Folder"];



            var folders = ServiceFactory.TextFolderManager.All(Repository, folderName);


            return folders.Select(it => new BlogInfo()
            {
                blogid = it.FullName,
                blogName = string.Join("/", it.NamePaths.ToArray()),
                url = ""
            }).ToArray();
        }
        //#region IBlogger Members




        //public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        //{
        //    throw new NotImplementedException();
        //}

        //public object editPost(string appKey, string postid, string username, string password, string content, bool publish)
        //{
        //    throw new NotImplementedException();
        //}

        //CookComputing.Blogger.Category[] IBlogger.getCategories(string blogid, string username, string password)
        //{
        //    CheckUserPassword(username, password);

        //    var folder = (TextFolder)(FolderHelper.Parse(Repository, blogid).AsActual());
        //    var categories = GetFolderCategories(folder);

        //    return categories.Select(it => new CookComputing.Blogger.Category()
        //    {
        //        categoryid = MetaWeblogHelper.CompositePostId(it),
        //        description = it.GetSummarize(),
        //        title = it.GetSummarize(),
        //        htmlUrl = "",
        //        rssUrl = ""
        //    }).ToArray();
        //}

        //public CookComputing.Blogger.Post getPost(string appKey, string postid, string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public CookComputing.Blogger.Post[] getRecentPosts(string appKey, string blogid, string username, string password, int numberOfPosts)
        //{
        //    throw new NotImplementedException();
        //}

        //public string getTemplate(string appKey, string blogid, string username, string password, string templateType)
        //{
        //    throw new NotImplementedException();
        //}

        //public UserInfo getUserInfo(string appKey, string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public string newPost(string appKey, string blogid, string username, string password, string content, bool publish)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool setTemplate(string appKey, string blogid, string username, string password, string template, string templateType)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        #region IBlogger Members

        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            throw new NotImplementedException();
        }

        public object editPost(string appKey, string postid, string username, string password, string content, bool publish)
        {
            throw new NotImplementedException();
        }

        CookComputing.Blogger.Category[] IBlogger.getCategories(string blogid, string username, string password)
        {
            throw new NotImplementedException();
        }

        public CookComputing.Blogger.Post getPost(string appKey, string postid, string username, string password)
        {
            throw new NotImplementedException();
        }

        public CookComputing.Blogger.Post[] getRecentPosts(string appKey, string blogid, string username, string password, int numberOfPosts)
        {
            throw new NotImplementedException();
        }

        public string getTemplate(string appKey, string blogid, string username, string password, string templateType)
        {
            throw new NotImplementedException();
        }

        public UserInfo getUserInfo(string appKey, string username, string password)
        {
            throw new NotImplementedException();
        }

        public string newPost(string appKey, string blogid, string username, string password, string content, bool publish)
        {
            throw new NotImplementedException();
        }

        public bool setTemplate(string appKey, string blogid, string username, string password, string template, string templateType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
