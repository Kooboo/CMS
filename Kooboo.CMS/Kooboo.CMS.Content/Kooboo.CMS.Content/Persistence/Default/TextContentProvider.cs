using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.Linq;
using System.Linq.Expressions;
using Kooboo.Extensions;
using System.Diagnostics;
using Kooboo.CMS.Content.Query.Translator;
namespace Kooboo.CMS.Content.Persistence.Default
{
    internal static class TypeConvert
    {
        public static T GetValue<T>(this object o, T defaultValue)
        {
            if (o == null)
            {
                return defaultValue;
            }
            return (T)Convert.ChangeType(o, typeof(T));
        }
    }
    #region EmptyTransactionUnit
    public class EmptyTransactionUnit : ITransactionUnit
    {

        public void Rollback()
        {

        }

        public void Commit()
        {

        }

        public void Dispose()
        {

        }
    }
    #endregion
    public class TextContentProvider : ITextContentProvider
    {

        void ITextContentProvider.ClearCategories(Models.TextContent content)
        {

            var list = content.GetRepository().GetCategoryData();

            list.RemoveAll(it => it.ContentUUID.EqualsOrNullEmpty(content.UUID, StringComparison.CurrentCultureIgnoreCase));

            content.GetRepository().SaveCategoryData(list);

        }

        void ITextContentProvider.AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            var list = content.GetRepository().GetCategoryData();
            foreach (var category in categories)
            {
                list.Add(category);
            }
            content.GetRepository().SaveCategoryData(list);
        }

        void ITextContentProvider.DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            var list = content.GetRepository().GetCategoryData();
            foreach (var category in categories)
            {
                var index = list.IndexOf(category);
                if (index != -1)
                {
                    list.RemoveAt(index);
                }
            }
            content.GetRepository().SaveCategoryData(list);
        }

        void IContentProvider<Models.TextContent>.Add(Models.TextContent content)
        {
            content.StoreFiles();
            var list = content.GetSchema().GetContents();
            ((IPersistable)content).OnSaving();
            list.Add(content);
            content.GetSchema().SaveContents(list);
            ((IPersistable)content).OnSaved();

        }

        void IContentProvider<Models.TextContent>.Update(Models.TextContent @new, Models.TextContent old)
        {
            @new.StoreFiles();

            var list = @new.GetSchema().GetContents();
            var index = list.IndexOf(old);
            if (index != -1)
            {
                list.RemoveAt(index);
                ((IPersistable)@new).OnSaving();
                list.Insert(index, @new);
                ((IPersistable)@new).OnSaved();
                @new.GetSchema().SaveContents(list);
            }

        }

        void IContentProvider<Models.TextContent>.Delete(Models.TextContent content)
        {
            var list = content.GetSchema().GetContents();
            var index = list.IndexOf(content);
            if (index != -1)
            {
                list.RemoveAt(index);
                content.GetSchema().SaveContents(list);
            }
            TextContentFileHelper.DeleteFiles(content);
        }

        object IContentProvider<Models.TextContent>.Execute(Query.IContentQuery<Models.TextContent> query)
        {
            return ContentQueryExecutor.Execute(query);
        }

        #region Import/export
        public IEnumerable<IDictionary<string, object>> ExportSchemaData(Schema schema)
        {
            return schema.GetContents();
        }

        public IEnumerable<Category> ExportCategoryData(Repository repository)
        {
            return repository.GetCategoryData();
        }

        public void ImportSchemaData(Schema schema, IEnumerable<IDictionary<string, object>> data)
        {
            var list = new List<TextContent>(data.Select(it => new TextContent(it) { Repository = schema.Repository.Name }));

            schema.SaveContents(list);

        }

        public void ImportCategoryData(Repository repository, IEnumerable<Category> data)
        {
            var list = new List<Category>(data);

            repository.SaveCategoryData(list);
        }
        #endregion


        #region ExecuteQuery
        public void ExecuteNonQuery(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }

        public object ExecuteScalar(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }
        #endregion

        public ITransactionUnit CreateTransaction(Repository repository)
        {
            return new EmptyTransactionUnit();
        }


        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            return content.GetRepository().GetCategoryData()
                .Where(it => it.ContentUUID == content.UUID)
                .ToArray();
        }
    }
}
