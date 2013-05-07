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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.Extensions;
using Kooboo.IO;
using Kooboo.Linq;
using Kooboo.Web.Url;
using System.Linq.Dynamic;
namespace Kooboo.CMS.Content.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMediaContentProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IContentProvider<MediaContent>))]
    public class MediaContentProvider : IMediaContentProvider
    {
        #region .ctor
        MediaContentMetadataStorage _metadataStorage;
        public MediaContentProvider(MediaContentMetadataStorage metadataStorage)
        {
            this._metadataStorage = metadataStorage;
        }
        #endregion

        #region locker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        #endregion

        #region Translator

        private class QueryExpressionTranslator : Kooboo.CMS.Content.Query.Translator.ExpressionVisitor
        {
            public Expression<Func<MediaContent, bool>> LinqExpression { get; set; }
            public Expression<Func<MediaContent, object>> OrderExprssion { get; set; }
            public bool OrderDesc { get; set; }
            public CallType CallType { get; set; }
            public IEnumerable<IContentQuery<TextContent>> CategoryQueries { get; set; }
            public int? Skip { get; set; }
            public int? Take { get; set; }
            public QueryExpressionTranslator()
            {
                LinqExpression = it => true;

                CategoryQueries = new IContentQuery<TextContent>[0];
            }
            public IQueryable<MediaContent> Translate(IExpression expression, IQueryable<MediaContent> contentQueryable)
            {
                this.Visite(expression);

                contentQueryable = contentQueryable.Where(this.LinqExpression);

                if (this.OrderExprssion != null)
                {
                    if (!this.OrderDesc)
                    {
                        contentQueryable = contentQueryable.OrderBy(this.OrderExprssion);
                    }
                    else
                    {
                        contentQueryable = contentQueryable.OrderByDescending(this.OrderExprssion);
                    }
                }

                if (Skip.HasValue)
                {
                    contentQueryable = contentQueryable.Skip(Skip.Value);
                }
                if (Take.HasValue)
                {
                    contentQueryable = contentQueryable.Take(Take.Value);
                }

                return contentQueryable;
            }
            protected override void VisitSkip(Query.Expressions.SkipExpression expression)
            {
                Skip = expression.Count;
            }

            protected override void VisitTake(Query.Expressions.TakeExpression expression)
            {
                Take = expression.Count;
            }

            protected override void VisitSelect(Query.Expressions.SelectExpression expression)
            {
                throw new NotSupportedException("Please instead of using IQueryable<TextContent>.Select()");
            }

            protected override void VisitOrder(Query.Expressions.OrderExpression expression)
            {
                OrderExprssion = it => it[expression.FieldName];
                OrderDesc = expression.Descending;
            }

            protected override void VisitCall(Query.Expressions.CallExpression expression)
            {
                this.CallType = expression.CallType;
            }

            protected override void VisitWhereBetweenOrEqual(Query.Expressions.WhereBetweenOrEqualExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereBetweenOrEqual(it[expression.FieldName], expression.Start, expression.End));
            }
            private bool WhereBetweenOrEqual(object value, object start, object end)
            {
                if (value is int)
                {
                    int value1 = value.GetValue<int>(0);
                    int startValue = start.GetValue<int>(0);
                    int endValue = end.GetValue<int>(0);
                    return (value1 >= startValue) && (value1 <= endValue);
                }
                else if (value is decimal)
                {
                    decimal value1 = value.GetValue<decimal>(0);
                    decimal startValue = start.GetValue<decimal>(0);
                    decimal endValue = end.GetValue<decimal>(0);
                    return (value1 >= startValue) && (value1 <= endValue);
                }
                else if (value is DateTime)
                {
                    DateTime value1 = value.GetValue<DateTime>(DateTime.MinValue);
                    DateTime startValue = start.GetValue<DateTime>(DateTime.MinValue);
                    DateTime endValue = end.GetValue<DateTime>(DateTime.MinValue);
                    return (value1 >= startValue) && (value1 <= endValue);
                }
                else
                {
                    throw new NotSupportedException(string.Format("The type '{0}' does not support WhereBetweenOrEqual method", value == null ? "null" : value.GetType().FullName));
                }
            }

            protected override void VisitWhereBetween(Query.Expressions.WhereBetweenExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereBetween(it[expression.FieldName], expression.Start, expression.End));
            }
            private bool WhereBetween(object value, object start, object end)
            {
                if (value is int)
                {
                    int value1 = value.GetValue<int>(0);
                    int startValue = start.GetValue<int>(0);
                    int endValue = end.GetValue<int>(0);
                    return (value1 > startValue) && (value1 < endValue);
                }
                else if (value is decimal)
                {
                    decimal value1 = value.GetValue<decimal>(0);
                    decimal startValue = start.GetValue<decimal>(0);
                    decimal endValue = end.GetValue<decimal>(0);
                    return (value1 > startValue) && (value1 < endValue);
                }
                else if (value is DateTime)
                {
                    DateTime value1 = value.GetValue<DateTime>(DateTime.MinValue);
                    DateTime startValue = start.GetValue<DateTime>(DateTime.MinValue);
                    DateTime endValue = end.GetValue<DateTime>(DateTime.MinValue);
                    return (value1 > startValue) && (value1 < endValue);
                }
                else
                {
                    throw new NotSupportedException(string.Format("The type '{0}' does not support WhereBetween method", value == null ? "null" : value.GetType().FullName));
                }
            }

            protected override void VisitWhereContains(Query.Expressions.WhereContainsExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereContains(it[expression.FieldName], expression.Value));
            }
            private bool WhereContains(object value, object value1)
            {
                string str1 = value == null ? "" : value.ToString();
                string str2 = value1 == null ? "" : value1.ToString();
                return str1.Contains(str2, StringComparison.CurrentCultureIgnoreCase);
            }

            protected override void VisitWhereEndsWith(Query.Expressions.WhereEndsWithExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => EndsWith(it[expression.FieldName], expression.Value));
            }
            private bool EndsWith(object value, object value1)
            {
                string str1 = value == null ? "" : value.ToString();
                string str2 = value1 == null ? "" : value1.ToString();
                return str1.EndsWith(str2, StringComparison.CurrentCultureIgnoreCase);
            }
            protected override void VisitWhereEquals(Query.Expressions.WhereEqualsExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereEquals(it[expression.FieldName], expression.Value));
            }
            private bool WhereEquals(object value, object value1)
            {
                if (value == value1)
                {
                    return true;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                if (value is string)
                {
                    return (value == null ? "" : value.ToString()).ToString().EqualsOrNullEmpty(value1 == null ? "" : value1.ToString(), StringComparison.OrdinalIgnoreCase);
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                return comparable1.Equals(comparable2);
            }

            protected override void VisitWhereClause(Query.Expressions.WhereClauseExpression expression)
            {
                throw new NotSupportedException();
            }

            protected override void VisitWhereGreaterThan(Query.Expressions.WhereGreaterThanExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereGreaterThan(it[expression.FieldName], expression.Value));
            }
            private bool WhereGreaterThan(object value, object value1)
            {
                if (value == value1)
                {
                    return false;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                var result = comparable1.CompareTo(comparable2);
                return result > 0;
            }
            protected override void VisitWhereGreaterThanOrEqual(Query.Expressions.WhereGreaterThanOrEqualExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereGreaterThanOrEqual(it[expression.FieldName], expression.Value));
            }
            private bool WhereGreaterThanOrEqual(object value, object value1)
            {
                if (value == value1)
                {
                    return true;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                var result = comparable1.CompareTo(comparable2);
                return result > 0 || result == 0;
            }

            protected override void VisitWhereLessThan(WhereLessThanExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereLessThan(it[expression.FieldName], expression.Value));
            }
            private bool WhereLessThan(object value, object value1)
            {
                if (value == value1)
                {
                    return false;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                var result = comparable1.CompareTo(comparable2);
                return result < 0;
            }
            protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereLessThanOrEqual(it[expression.FieldName], expression.Value));
            }
            private bool WhereLessThanOrEqual(object value, object value1)
            {
                if (value == value1)
                {
                    return true;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                var result = comparable1.CompareTo(comparable2);
                return result < 0 || result == 0;
            }

            protected override void VisitWhereStartsWith(Query.Expressions.WhereStartsWithExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereStartsWith(it[expression.FieldName], expression.Value));
            }
            private bool WhereStartsWith(object value, object value1)
            {
                string str1 = value == null ? "" : value.ToString();
                string str2 = value == null ? "" : value1.ToString();
                return str1.StartsWith(str2, StringComparison.CurrentCultureIgnoreCase);
            }
            protected override void VisitWhereNotEquals(Query.Expressions.WhereNotEqualsExpression expression)
            {
                LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereNotEquals(it[expression.FieldName], expression.Value));
            }
            private bool WhereNotEquals(object value, object value1)
            {
                if (value == value1)
                {
                    return true;
                }
                if (value == null || value1 == null)
                {
                    return false;
                }
                IComparable comparable1 = (IComparable)value;
                IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
                var result = comparable1.CompareTo(comparable2);
                return result != 0;
            }
            private Expression<Func<MediaContent, bool>> VisitInner(IExpression expression)
            {
                var visitor = new QueryExpressionTranslator();
                visitor.Visite(expression);
                return visitor.LinqExpression;
            }
            protected override void VisitAndAlso(Query.Expressions.AndAlsoExpression expression)
            {
                Expression<Func<MediaContent, bool>> leftClause = it => true;
                if (!(expression.Left is TrueExpression))
                {
                    leftClause = VisitInner(expression.Left);
                }

                Expression<Func<MediaContent, bool>> rightClause = it => true;
                if (!(expression.Right is TrueExpression))
                {
                    rightClause = VisitInner(expression.Right);
                }

                LinqExpression = PredicateBuilder.And(LinqExpression, PredicateBuilder.And(leftClause, rightClause));
            }

            protected override void VisitOrElse(Query.Expressions.OrElseExpression expression)
            {
                Expression<Func<MediaContent, bool>> leftClause = it => false;
                if (!(expression.Left is FalseExpression))
                {
                    leftClause = VisitInner(expression.Left);
                }

                Expression<Func<MediaContent, bool>> rightClause = it => false;
                if (!(expression.Right is FalseExpression))
                {
                    rightClause = VisitInner(expression.Right);
                }

                var exp = PredicateBuilder.Or(leftClause, rightClause);

                LinqExpression = PredicateBuilder.And(LinqExpression, exp);
            }

            protected override void VisitWhereCategory(WhereCategoryExpression expression)
            {
                throw new NotSupportedException();
                //CategoryQueries = CategoryQueries.Concat(new[] { expression.CategoryQuery });
            }

            protected override void VisitFalse(FalseExpression expression)
            {
                this.LinqExpression = PredicateBuilder.And(LinqExpression, it => false);
            }

            protected override void VisitTrue(TrueExpression expression)
            {
                this.LinqExpression = PredicateBuilder.And(LinqExpression, it => true);
            }

            protected override void VisitWhereIn(WhereInExpression expression)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Add
        public virtual void Add(Models.MediaContent content)
        {
            //throw new NotImplementedException();
            this.Add(content, true);
        }

        public virtual void Add(MediaContent content, bool overrided)
        {
            if (content.ContentFile != null)
            {

                content.FileName = content.ContentFile.FileName;

                content.UserKey = content.FileName;
                content.UUID = content.FileName;

                // if the file already exist and dont need to overrided just return 
                if (content.Exist() && !overrided)
                {
                    return;
                }

                var contentPath = new MediaContentPath(content);

                content.VirtualPath = contentPath.VirtualPath;

                locker.EnterWriteLock();

                try
                {
                    content.ContentFile.Stream.SaveAs(contentPath.PhysicalPath);
                }
                finally
                {
                    locker.ExitWriteLock();
                }

                SetFilePublished(contentPath.PhysicalPath, content.Published);

                _metadataStorage.SaveMetadata(content);
            }
        }
        #endregion

        #region SetFilePublished
        private static void SetFilePublished(string filePath, bool? published)
        {
            FileAttributes fileAttributes = FileAttributes.Normal;
            if (!published.HasValue || published.Value == false)
            {
                fileAttributes = FileAttributes.Hidden;
            }
            File.SetAttributes(filePath, fileAttributes);
        }
        #endregion

        #region IsPublished
        public static bool IsPublished(string filePath)
        {
            return (File.GetAttributes(filePath) & FileAttributes.Hidden) != FileAttributes.Hidden;
        }
        #endregion

        #region Update
        public virtual void Update(Models.MediaContent @new, Models.MediaContent old)
        {
            var contentPath = new MediaContentPath(@new);

            if (!@new.FileName.EqualsOrNullEmpty(old.FileName, StringComparison.OrdinalIgnoreCase))
            {
                Kooboo.IO.IOUtility.RenameFile(old.PhysicalPath, @new.FileName);
            }
            if (@new.ContentFile != null)
            {

                @new.FileName = @new.ContentFile.FileName;

                @new.UserKey = @new.FileName;
                @new.UUID = @new.FileName;

                locker.EnterWriteLock();
                try
                {
                    @new.ContentFile.Stream.SaveAs(contentPath.PhysicalPath);
                }
                finally
                {
                    @new.ContentFile.Stream.Close();
                    locker.ExitWriteLock();
                }

            }

            SetFilePublished(contentPath.PhysicalPath, @new.Published);
            _metadataStorage.SaveMetadata(@new);
        }
        #endregion

        #region Delete
        public void Delete(Models.MediaContent content)
        {
            locker.EnterWriteLock();
            try
            {
                if (File.Exists(content.PhysicalPath))
                {
                    File.Delete(content.PhysicalPath);
                }
                _metadataStorage.DeleteMetadata(content);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
        #endregion

        #region Query
        public IQueryable<MediaContent> All(MediaFolder folder)
        {
            FolderPath folderPath = new FolderPath(folder);
            if (Directory.Exists(folderPath.PhysicalPath))
            {
                return Directory.EnumerateFiles(folderPath.PhysicalPath)
                    .Where(it => string.Compare(Path.GetFileName(it), PathHelper.SettingFileName, true) != 0)
                    .Where(it => string.Compare(Path.GetExtension(it), MediaContentMetadataStorage.MetadataFileExtension, true) != 0)
                    .Select(it => GetMediaContent(folder, it))
                    .ToArray()
                    .AsQueryable();
            }
            return new MediaContent[0].AsQueryable();
        }
        private MediaContent GetMediaContent(MediaFolder mediaFolder, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var folderPath = new FolderPath(mediaFolder);
            var creationDate = File.GetCreationTimeUtc(filePath);
            var lastModifiedDate = File.GetLastWriteTimeUtc(filePath);
            var fileInfo = new FileInfo(filePath);

            var mediaContent = new MediaContent()
            {
                UserKey = fileName,
                FileName = fileName,
                UUID = fileName,
                VirtualPath = UrlUtility.Combine(folderPath.VirtualPath, fileName),
                UtcCreationDate = creationDate,
                UtcLastModificationDate = lastModifiedDate,
                Size = fileInfo.Length,
                Repository = mediaFolder.Repository.Name,
                FolderName = mediaFolder.FullName,
                Published = IsPublished(filePath)
            };
            _metadataStorage.FillMetadata(mediaContent);
            return mediaContent;
        }
        public object Execute(Query.IContentQuery<Models.MediaContent> query)
        {
            var mediaQuery = (MediaContentQuery)query;
            QueryExpressionTranslator translator = new QueryExpressionTranslator();
            var contentQueryable = translator.Translate(query.Expression, All(mediaQuery.MediaFolder));
            //translator.Visite(query.Expression);
            switch (translator.CallType)
            {
                case CallType.Count:
                    return contentQueryable.Count();
                case CallType.First:
                    return contentQueryable.First();
                case CallType.Last:
                    return contentQueryable.Last();
                case CallType.LastOrDefault:
                    return contentQueryable.LastOrDefault();
                case CallType.FirstOrDefault:
                    return contentQueryable.FirstOrDefault();
                case CallType.Unspecified:
                default:
                    return contentQueryable;
            }
        }
        #endregion

        #region Move
        public void Move(MediaFolder sourceFolder, string oldFileName, MediaFolder targetFolder, string newFileName)
        {
            var oldMediaContent = new MediaContent() { Repository = sourceFolder.Repository.Name, FolderName = sourceFolder.FullName, UUID = oldFileName, FileName = oldFileName };
            var newMediaContent = new MediaContent() { Repository = targetFolder.Repository.Name, FolderName = targetFolder.FullName, UUID = newFileName, FileName = newFileName };

            var oldPath = new MediaContentPath(oldMediaContent);
            var newPath = new MediaContentPath(newMediaContent);

            File.Move(oldPath.PhysicalPath, newPath.PhysicalPath);

            _metadataStorage.MoveMetadata(oldMediaContent, newMediaContent);
        }
        #endregion

        #region InitializeMediaContents
        public void InitializeMediaContents(Repository repository)
        {
            //no need to do anything.
        }
        #endregion
    }


    #region MediaContentMetadataStorage
    public class MediaContentMetadataStorage
    {
        #region Fields
        public static string MetadataFileExtension = ".meta";
        #endregion

        #region GetMetadataFilePath
        public virtual string GetMetadataFilePath(MediaContent mediaContent)
        {
            return new MediaContentPath(mediaContent).PhysicalPath + MetadataFileExtension;
        }
        #endregion

        #region MoveMetadata
        public virtual void MoveMetadata(MediaContent old, MediaContent @new)
        {
            var oldPath = GetMetadataFilePath(old);
            if (File.Exists(oldPath))
            {
                var newPath = GetMetadataFilePath(@new);
                File.Move(oldPath, newPath);
            }
        }
        #endregion

        #region SaveMetadata
        public virtual void SaveMetadata(MediaContent mediaContent)
        {
            if (mediaContent.Metadata != null)
            {
                string metadataFile = GetMetadataFilePath(mediaContent);
                Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(mediaContent.Metadata, metadataFile);
            }
        }
        #endregion

        #region FillMetadata
        public virtual void FillMetadata(MediaContent mediaContent)
        {
            string metadataFile = GetMetadataFilePath(mediaContent);
            if (File.Exists(metadataFile))
            {
                var metadata = Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize<MediaContentMetadata>(metadataFile);
                mediaContent.Metadata = metadata;

                mediaContent["Metadata.Title"] = metadata.Title;
                mediaContent["Metadata.AlternateText"] = metadata.AlternateText;
                mediaContent["Metadata.Description"] = metadata.Description;
            }
        } 
        #endregion

        #region DeleteMetadata
        public virtual void DeleteMetadata(MediaContent mediaContent)
        {
            string metadataFile = GetMetadataFilePath(mediaContent);
            if (File.Exists(metadataFile))
            {
                File.Delete(metadataFile);
            }
        } 
        #endregion
    }
    #endregion
}
