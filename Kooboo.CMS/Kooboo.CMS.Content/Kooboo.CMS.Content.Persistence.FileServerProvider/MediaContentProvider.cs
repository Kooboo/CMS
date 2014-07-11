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
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query.Expressions;
using System.Linq.Expressions;
using Kooboo.CMS.Content.Query;
using System.IO;
using Kooboo.CMS.Content.FileServer.Interfaces;

using System.Net;
using System.Web;
using Kooboo.Common.ObjectContainer;
namespace Kooboo.CMS.Content.Persistence.FileServerProvider
{
    #region Translator

    public class QueryExpressionTranslator : Kooboo.CMS.Content.Query.Translator.ExpressionVisitor
    {

        public CallType CallType { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string FileName = null;
        public string Prefix = null;

        public QueryExpressionTranslator()
        {

        }
        public void Translate(IExpression expression)
        {
            this.Visite(expression);
        }


        protected override void VisitSkip(Query.Expressions.SkipExpression expression)
        {
            Skip = expression.Count;
        }

        protected override void VisitTake(Query.Expressions.TakeExpression expression)
        {
            Take = expression.Count;
        }

        protected override void VisitWhereEquals(Query.Expressions.WhereEqualsExpression expression)
        {
            if (expression.Value != null)
            {
                ValidExpression(expression.FieldName);
                FileName = expression.Value.ToString();
            }

        }
        private void ValidExpression(string fieldName)
        {
            if (!fieldName.EqualsOrNullEmpty("FileName", StringComparison.OrdinalIgnoreCase)
                && !fieldName.EqualsOrNullEmpty("UUID", StringComparison.OrdinalIgnoreCase)
                && !fieldName.EqualsOrNullEmpty("UserKey", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("The azure storage provider only support query by FileName,UUID,UserKey.");
            }
            if (!string.IsNullOrEmpty(FileName) || !string.IsNullOrEmpty(Prefix))
            {
                throw new NotSupportedException("The azure storage provider only support query by one condition.");
            }
        }
        protected override void VisitWhereStartsWith(Query.Expressions.WhereStartsWithExpression expression)
        {
            WhereStartWith(expression.FieldName, expression.Value);
        }

        private void WhereStartWith(string fieldName, object value)
        {
            if (value != null)
            {
                ValidExpression(fieldName);
                Prefix = value.ToString();
            }
        }
        protected override void VisitWhereContains(Query.Expressions.WhereContainsExpression expression)
        {
            WhereStartWith(expression.FieldName, expression.Value);
        }

        private void VisitInner(IExpression expression)
        {
            this.Visite(expression);
        }

        protected override void VisitAndAlso(Query.Expressions.AndAlsoExpression expression)
        {
            if (!(expression.Left is TrueExpression))
            {
                VisitInner(expression.Left);
            }

            if (!(expression.Right is TrueExpression))
            {
                VisitInner(expression.Right);
            }
        }

        protected override void VisitOrElse(Query.Expressions.OrElseExpression expression)
        {
            if (!(expression.Left is FalseExpression))
            {
                VisitInner(expression.Left);
            }

            if (!(expression.Right is FalseExpression))
            {
                VisitInner(expression.Right);
            }
        }


        protected override void VisitCall(Query.Expressions.CallExpression expression)
        {
            this.CallType = expression.CallType;
        }

        #region NotSupported

        protected override void VisitWhereCategory(WhereCategoryExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitFalse(FalseExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereIn(WhereInExpression expression)
        {
            ThrowNotSupported();
        }

        private void ThrowNotSupported()
        {
            throw new NotSupportedException("Not supported for azure blob storage.");
        }
        protected override void VisitSelect(Query.Expressions.SelectExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitOrder(Query.Expressions.OrderExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereBetweenOrEqual(Query.Expressions.WhereBetweenOrEqualExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereBetween(Query.Expressions.WhereBetweenExpression expression)
        {
            ThrowNotSupported();
        }


        protected override void VisitWhereEndsWith(Query.Expressions.WhereEndsWithExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereClause(Query.Expressions.WhereClauseExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereGreaterThan(Query.Expressions.WhereGreaterThanExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereGreaterThanOrEqual(Query.Expressions.WhereGreaterThanOrEqualExpression expression)
        {
            ThrowNotSupported();
        }


        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            ThrowNotSupported();
        }

        protected override void VisitWhereNotEquals(Query.Expressions.WhereNotEqualsExpression expression)
        {
            ThrowNotSupported();
        }
        protected override void VisitNot(NotExpression expression)
        {
            ThrowNotSupported();
        }
        #endregion

     
    }
    #endregion
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IMediaContentProvider), Order = 2)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IContentProvider<MediaContent>), Order = 2)]
    public class MediaContentProvider : IMediaContentProvider
    {
        #region IMediaContentProvider
        public void Add(MediaContent content, bool overrided)
        {
            if (content.ContentFile != null)
            {
                content.FileName = content.ContentFile.FileName;

                content.UserKey = content.FileName;
                content.UUID = content.FileName;

                RemoteServiceFactory.CreateService<IMediaContentService>().Add(
                    new MediaContentParameter()
                    {
                        MediaContent = content,
                        FileData = content.ContentFile.Stream.ReadData()
                    });
            }
        }
        public void Move(MediaFolder sourceFolder, string oldFileName, MediaFolder targetFolder, string newFileName)
        {
            RemoteServiceFactory.CreateService<IMediaContentService>().Move(sourceFolder.Repository.Name,
                sourceFolder.FullName, oldFileName, targetFolder.FullName, newFileName);
        }

        public void Add(MediaContent content)
        {
            Add(content, true);
        }

        public void Update(MediaContent @new, MediaContent old)
        {
            if (!@new.FileName.EqualsOrNullEmpty(old.FileName, StringComparison.OrdinalIgnoreCase))
            {
                RemoteServiceFactory.CreateService<IMediaContentService>().Move(old.Repository,
                    old.FolderName, old.FileName, @new.FolderName, @new.FileName);
            }
            else
            {
                var parameter = new MediaContentParameter()
                {
                    MediaContent = @new
                };

                RemoteServiceFactory.CreateService<IMediaContentService>().Update(parameter);
            }

        }

        public void Delete(MediaContent content)
        {
            RemoteServiceFactory.CreateService<IMediaContentService>().Delete(content.Repository, content.FolderName, content.UUID);
        }


        public object Execute(IContentQuery<MediaContent> query)
        {
            var mediaQuery = (MediaContentQuery)query;


            QueryExpressionTranslator translator = new QueryExpressionTranslator();

            translator.Translate(query.Expression);

            //translator.Visite(query.Expression);
            IEnumerable<MediaContent> contents;
            switch (translator.CallType)
            {
                case CallType.Count:
                    return RemoteServiceFactory.CreateService<IMediaContentService>().Count(mediaQuery.MediaFolder.Repository.Name,
                        mediaQuery.MediaFolder.FullName, translator.Prefix);
                case CallType.First:
                    contents = Execute(mediaQuery.MediaFolder, translator);
                    return contents.First();
                case CallType.Last:
                    contents = Execute(mediaQuery.MediaFolder, translator);
                    return contents.Last();
                case CallType.LastOrDefault:
                    contents = Execute(mediaQuery.MediaFolder, translator);
                    return contents.LastOrDefault();
                case CallType.FirstOrDefault:
                    contents = Execute(mediaQuery.MediaFolder, translator);
                    return contents.FirstOrDefault();
                case CallType.Unspecified:
                default:
                    contents = Execute(mediaQuery.MediaFolder, translator);
                    return contents;
            }
        }

        private static IEnumerable<MediaContent> Execute(MediaFolder mediaFolder, QueryExpressionTranslator translator)
        {
            var mediaContentService = RemoteServiceFactory.CreateService<IMediaContentService>();
            if (!string.IsNullOrEmpty(translator.FileName))
            {
                return new[] { mediaContentService.Get(mediaFolder.Repository.Name, mediaFolder.FullName, translator.FileName) };
            }
            else
            {
                var maxResult = 50;
                if (translator.Take.HasValue && translator.Take.Value != 0)
                {
                    maxResult = translator.Take.Value;
                }
                var skip = 0;
                if (translator.Skip.HasValue)
                {
                    skip = translator.Skip.Value;
                }
                return mediaContentService.All(mediaFolder.Repository.Name, mediaFolder.FullName, skip, maxResult, translator.Prefix);
            }
        }
        #endregion


        public void InitializeMediaContents(Repository repository)
        {
            Kooboo.CMS.Content.Persistence.Default.MediaFolderProvider fileMediaFolderProvider = EngineContext.Current.Resolve<Kooboo.CMS.Content.Persistence.Default.MediaFolderProvider>();
            foreach (var item in fileMediaFolderProvider.All(repository))
            {
                ImportMediaFolderDataCascading(fileMediaFolderProvider.Get(item));
            }
        }
        private void ImportMediaFolderDataCascading(MediaFolder mediaFolder)
        {
            Kooboo.CMS.Content.Persistence.Default.MediaContentProvider fileProvider = EngineContext.Current.Resolve<Kooboo.CMS.Content.Persistence.Default.MediaContentProvider>();

            //add media folder
            MediaFolderProvider folderProvider = new MediaFolderProvider();
            folderProvider.Add(mediaFolder);

            foreach (var item in fileProvider.All(mediaFolder))
            {
                item.ContentFile = new ContentFile() { FileName = item.FileName };
                using (var fileStream = new FileStream(item.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    item.ContentFile.Stream = fileStream;
                    Add(item);
                }
            }
            Kooboo.CMS.Content.Persistence.Default.MediaFolderProvider fileMediaFolderProvider = new Default.MediaFolderProvider();
            foreach (var item in fileMediaFolderProvider.ChildFolders(mediaFolder))
            {
                ImportMediaFolderDataCascading(item);
            }
        }

        public byte[] GetContentStream(MediaContent content)
        {
            if (string.IsNullOrEmpty(content.GetRepository().Name))
            {

                var webClient = new WebClient();
                return webClient.DownloadData(content.Url);
            }
            else
            {
                var mediaContentService = RemoteServiceFactory.CreateService<IMediaContentService>();
                return mediaContentService.GetBytes(content.Repository, content.FolderName, content.FileName);
            }
        }

        public void SaveContentStream(MediaContent content, Stream stream)
        {
            RemoteServiceFactory.CreateService<IMediaContentService>().SaveBytes(new MediaContentParameter()
            {
                MediaContent = content,
                FileData = stream.ReadData()
            });
        }
    }
}
