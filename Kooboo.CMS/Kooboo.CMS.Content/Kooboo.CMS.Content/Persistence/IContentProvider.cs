﻿#region License
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
using System.Linq.Expressions;
using Kooboo.CMS.Content.Query;
using System.IO;
namespace Kooboo.CMS.Content.Persistence
{
    public interface IContentProvider<T>
        where T : ContentBase
    {
        void Add(T content);
        void Update(T @new, T old);
        void Delete(T content);

        object Execute(IContentQuery<T> query);
    }

    public interface ITransactionUnit : IDisposable
    {
        void Rollback();
        void Commit();
    }
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
    public interface ITextContentProvider : IContentProvider<TextContent>
    {
        ITransactionUnit CreateTransaction(Repository repository);

        /// <summary>
        /// Queries the categories.        
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        IEnumerable<Category> QueryCategories(TextContent content);

        void ClearCategories(TextContent content);
        void AddCategories(TextContent content, params Category[] categories);
        void DeleteCategories(TextContent content, params Category[] categories);

        IEnumerable<IDictionary<string, object>> ExportSchemaData(Schema schema);

        IEnumerable<Category> ExportCategoryData(Repository repository);

        void ImportSchemaData(Schema schema, IEnumerable<IDictionary<string, object>> data);

        void ImportCategoryData(Repository repository, IEnumerable<Category> data);

        #region ExecuteQuery
        void ExecuteNonQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters);
        IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters);
        object ExecuteScalar(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters);
        #endregion
    }

    public interface IMediaContentProvider : IContentProvider<MediaContent>
    {
        void Add(MediaContent content, bool @overrided);
        void Move(MediaFolder sourceFolder, string oldFileName, MediaFolder targetFolder, string newFileName);

        byte[] GetContentStream(MediaContent content);
        void SaveContentStream(MediaContent content,Stream stream);

        void InitializeMediaContents(Repository repository);        
    }
}
