using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
    public class TextContentExtensionDuck : ExtensionDuck
    {
        public static readonly Type[] EXTENSION_TYPES =
               new Type[]
				{
					typeof(TextContentQueryExtensions)	
				};
        public TextContentExtensionDuck(TextContent textContent)
            : this(textContent, EXTENSION_TYPES)
        {
        }

        public TextContentExtensionDuck(TextContent textContent, params Type[] extentionTypes)
            : base(textContent, extentionTypes)
        {
        }
    }
    public abstract class IContentQueryExtensionDuck<T> : ExtensionDuck, IContentQuery<T>
        where T : ContentBase
    {
        IContentQuery<T> innerContentQuery;
        public IContentQueryExtensionDuck(IContentQuery<T> contentQuery, params Type[] extentionTypes)
            : base(contentQuery, extentionTypes)
        {
            this.innerContentQuery = contentQuery;
        }

        public Repository Repository
        {
            get { return this.innerContentQuery.Repository; }
        }

        public Content.Query.Expressions.IExpression Expression
        {
            get { return this.innerContentQuery.Expression; }
        }

        public IContentQuery<T> Select(params string[] fields)
        {
            var newContentQuery = this.innerContentQuery.Select(fields);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> Skip(int count)
        {
            var newContentQuery = this.innerContentQuery.Skip(count);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> OrderBy(string field)
        {
            var newContentQuery = this.innerContentQuery.OrderBy(field);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> OrderByDescending(string field)
        {
            var newContentQuery = this.innerContentQuery.OrderByDescending(field);
            return this.CreateDuck(newContentQuery);
        }
        public IContentQuery<T> OrderBy(Content.Query.Expressions.OrderExpression expression)
        {
            var newContentQuery = this.innerContentQuery.OrderBy(expression);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> Or(Content.Query.Expressions.IWhereExpression expression)
        {
            var newContentQuery = this.innerContentQuery.Or(expression);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> Where(Content.Query.Expressions.IWhereExpression expression)
        {
            var newContentQuery = this.innerContentQuery.Where(expression);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> Where(string whereClause)
        {
            var newContentQuery = this.innerContentQuery.Where(whereClause);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereBetween(string fieldName, object start, object end)
        {
            var newContentQuery = this.innerContentQuery.WhereBetween(fieldName, start, end);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereBetweenOrEqual(string fieldName, object start, object end)
        {
            var newContentQuery = this.innerContentQuery.WhereBetweenOrEqual(fieldName, start, end);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereContains(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereContains(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereEndsWith(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereEndsWith(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereEquals(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereEquals(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereNotEquals(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereNotEquals(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereGreaterThan(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereGreaterThan(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereGreaterThanOrEqual(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereGreaterThanOrEqual(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereLessThan(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereLessThan(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereLessThanOrEqual(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereLessThanOrEqual(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }

        public IContentQuery<T> WhereStartsWith(string fieldName, object value)
        {
            var newContentQuery = this.innerContentQuery.WhereStartsWith(fieldName, value);
            return this.CreateDuck(newContentQuery);
        }
        public IContentQuery<T> WhereIn(string fieldName, object[] values)
        {
            var newContentQuery = this.innerContentQuery.WhereIn(fieldName, values);
            return this.CreateDuck(newContentQuery);
        }
        public IContentQuery<T> WhereNotIn(string fieldName, params object[] values)
        {
            var newContentQuery = this.innerContentQuery.WhereNotIn(fieldName, values);
            return this.CreateDuck(newContentQuery);
        }
        public IContentQuery<T> Take(int count)
        {
            var newContentQuery = this.innerContentQuery.Take(count);
            return this.CreateDuck(newContentQuery);
        }

        public int Count()
        {
            return this.innerContentQuery.Count();
        }

        public T First()
        {
            return this.innerContentQuery.First();
        }

        public T FirstOrDefault()
        {
            return this.innerContentQuery.FirstOrDefault();
        }

        public T Last()
        {
            return this.innerContentQuery.Last();
        }

        public T LastOrDefault()
        {
            return this.innerContentQuery.LastOrDefault();
        }

        public IContentQuery<T> Create(Content.Query.Expressions.IExpression expression)
        {
            var newContentQuery = this.innerContentQuery.Create(expression);
            return this.CreateDuck(newContentQuery);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.innerContentQuery.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerContentQuery.GetEnumerator();
        }

        protected abstract IContentQueryExtensionDuck<T> CreateDuck(IContentQuery<T> contentQuery);
    }
    public class ITextContentQueryExtensionDuck : IContentQueryExtensionDuck<TextContent>
    {
        public static readonly Type[] EXTENSION_TYPES =
                new Type[]
				{
					typeof(IContentQueryExtensions),
                    typeof(IContentQueryToPagedListExtension),
                    typeof(TextContentQueryExtensions)				
				};
        public ITextContentQueryExtensionDuck(IContentQuery<TextContent> contentQuery)
            : this(contentQuery, EXTENSION_TYPES)
        {
        }

        public ITextContentQueryExtensionDuck(IContentQuery<TextContent> contentQuery, params Type[] extentionTypes)
            : base(contentQuery, extentionTypes)
        {
        }

        protected override IContentQueryExtensionDuck<TextContent> CreateDuck(IContentQuery<TextContent> contentQuery)
        {
            return new ITextContentQueryExtensionDuck(contentQuery);
        }
    }
    public class IMediaContentQueryExtensionDuck : IContentQueryExtensionDuck<MediaContent>
    {
        public static readonly Type[] EXTENSION_TYPES =
                new Type[]
				{
					typeof(IContentQueryExtensions),
                    typeof(IContentQueryToPagedListExtension),
                    typeof(MediaContentQueryExtensions)				
				};
        public IMediaContentQueryExtensionDuck(IContentQuery<MediaContent> contentQuery)
            : this(contentQuery, EXTENSION_TYPES)
        {
        }

        public IMediaContentQueryExtensionDuck(IContentQuery<MediaContent> contentQuery, params Type[] extentionTypes)
            : base(contentQuery, extentionTypes)
        {
        }

        protected override IContentQueryExtensionDuck<MediaContent> CreateDuck(IContentQuery<MediaContent> contentQuery)
        {
            return new IMediaContentQueryExtensionDuck(contentQuery);
        }
    }

    public class ContentHelper
    {
        public class MediaFolderEx : MediaFolder
        {
            public MediaFolderEx() : base() { }
            public MediaFolderEx(Repository repository, string fullName) : base(repository, fullName) { }

            public IMediaContentQueryExtensionDuck CreateQuery()
            {
                var contentQuery = MediaContentQueryExtensions.CreateQuery(this);
                return new IMediaContentQueryExtensionDuck(contentQuery);
            }
        }
        public class TextFolderEx : TextFolder
        {
            public TextFolderEx() : base() { }
            public TextFolderEx(Repository repository, string fullName) : base(repository, fullName) { }

            public ITextContentQueryExtensionDuck CreateQuery()
            {
                var contentQuery = TextContentQueryExtensions.CreateQuery(this);
                return new ITextContentQueryExtensionDuck(contentQuery);
            }
        }
        public class SchemaEx : Schema
        {
            public SchemaEx()
                : base()
            {
            }
            public SchemaEx(Repository repository, string name)
                : base(repository, name)
            {

            }
            public ITextContentQueryExtensionDuck CreateQuery()
            {
                var contentQuery = TextContentQueryExtensions.CreateQuery(this);
                return new ITextContentQueryExtensionDuck(contentQuery);
            }
        }
        public static MediaFolder NewMediaFolderObject(string folderName)
        {
            return new MediaFolderEx(Repository.Current, folderName);
        }
        public static TextFolder NewTextFolderObject(string folderName)
        {
            return new TextFolderEx(Repository.Current, folderName);
        }
        public static Schema NewSchemaObject(string schemaName)
        {
            return new SchemaEx(Repository.Current, schemaName);
        }

        public static IEnumerable<string> SplitMultiFiles(string files)
        {
            if (string.IsNullOrEmpty(files))
            {
                return new string[0];
            }
            return files.Split('|').Select(it => Kooboo.Web.Url.UrlUtility.ResolveUrl(it)).ToArray();
        }
    }
}
