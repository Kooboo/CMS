#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;

using System.Linq.Expressions;

using System.Diagnostics;
using Kooboo.CMS.Content.Query.Translator;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Persistence.Default.ContentQuery;
namespace Kooboo.CMS.Content.Persistence.Default
{
    #region TypeConvert
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
    #endregion

    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextContentProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IContentProvider<TextContent>))]
    public class TextContentProvider : ITextContentProvider
    {
        #region ClearCategories/AddCategories/DeleteCategories
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
        #endregion

        #region Add/Update/Delete
        void IContentProvider<Models.TextContent>.Add(Models.TextContent content)
        {
            content.StoreFiles();
            var list = content.GetSchema().GetContents();
            ((IPersistable)content).OnSaving();
            list.Add(content.ConvertToUTCTime());
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
                list.Insert(index, @new.ConvertToUTCTime());
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
        #endregion

        #region Execute
        object IContentProvider<Models.TextContent>.Execute(Query.IContentQuery<Models.TextContent> query)
        {
            return ContentQueryExecutor.Execute(query);
        }

        #endregion

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
            var list = new List<TextContent>(data.Select(it => new TextContent(it) { Repository = schema.Repository.Name }.ConvertToUTCTime()));

            schema.SaveContents(list);

        }

        public void ImportCategoryData(Repository repository, IEnumerable<Category> data)
        {
            var list = new List<Category>(data);

            repository.SaveCategoryData(list);
        }
        #endregion

        #region ExecuteQuery
        public void ExecuteNonQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }

        public object ExecuteScalar(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for XML provider");
        }
        #endregion

        #region CreateTransaction
        public ITransactionUnit CreateTransaction(Repository repository)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region QueryCategories
        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            return content.GetRepository().GetCategoryData()
                .Where(it => it.ContentUUID == content.UUID)
                .ToArray();
        }
        #endregion
    }
}
