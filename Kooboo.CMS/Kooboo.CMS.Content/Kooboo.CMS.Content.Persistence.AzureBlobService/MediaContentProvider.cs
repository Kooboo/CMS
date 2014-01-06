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
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Kooboo.CMS.Content.Query.Expressions;
using System.Linq.Expressions;
using Kooboo.CMS.Content.Query;
using System.IO;
using Kooboo.CMS.Content.Query.Translator;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    #region Translator

    public class QueryExpressionTranslator : Kooboo.CMS.Content.Query.Translator.ExpressionVisitor
    {
        public CallType CallType { get; set; }
        private int? Skip { get; set; }
        private int? Take { get; set; }
        string fileName = null;
        string prefix = null;

        public QueryExpressionTranslator()
        {

        }
        public IEnumerable<CloudBlob> Translate(IExpression expression, CloudBlobClient blobClient, MediaFolder mediaFolder)
        {
            this.Visite(expression);

            if (!string.IsNullOrEmpty(fileName))
            {
                var blob = blobClient.GetBlockBlobReference(mediaFolder.GetMediaFolderItemPath(fileName));
                if (!blob.Exists())
                {
                    return new CloudBlob[] { };
                }
                blob.FetchAttributes();
                return new[] { blob };
            }
            else
            {
                var maxResult = 100;
                if (Take.HasValue)
                {
                    maxResult = Take.Value;
                }
                var take = maxResult;

                var skip = 0;
                if (Skip.HasValue)
                {
                    skip = Skip.Value;
                    maxResult = +skip;
                }
                var blobPrefix = mediaFolder.GetMediaFolderItemPath(prefix);

                if (string.IsNullOrEmpty(prefix))
                {
                    blobPrefix += "/";
                }

                return blobClient.ListBlobsWithPrefixSegmented(blobPrefix, maxResult, null, new BlobRequestOptions() { BlobListingDetails = Microsoft.WindowsAzure.StorageClient.BlobListingDetails.Metadata, UseFlatBlobListing = false })
                    .Results.Skip(skip).Select(it => it as CloudBlob).Take(take);
            }
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
                fileName = expression.Value.ToString();
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
            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(prefix))
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
                prefix = value.ToString();
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
            OrderFields.Add(new OrderField() { FieldName = expression.FieldName, Descending = expression.Descending });
            //ThrowNotSupported();
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
        #endregion
    }
    #endregion
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMediaContentProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IContentProvider<MediaContent>), Order = 2)]
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

                var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

                var contentBlob = blobClient.GetBlobReference(content.GetMediaBlobPath());
                contentBlob = content.MediaContentToBlob(contentBlob);

                contentBlob.UploadFromStream(content.ContentFile.Stream);

                content.VirtualPath = contentBlob.Uri.ToString();
            }
        }

        public void Move(MediaFolder sourceFolder, string oldFileName, MediaFolder targetFolder, string newFileName)
        {
            var oldMediaContent = new MediaContent() { Repository = sourceFolder.Repository.Name, FolderName = sourceFolder.FullName, UUID = oldFileName, FileName = oldFileName };
            var newMediaContent = new MediaContent() { Repository = targetFolder.Repository.Name, FolderName = targetFolder.FullName, UUID = newFileName, FileName = newFileName };

            MoveContent(oldMediaContent, newMediaContent);
        }

        private static void MoveContent(MediaContent oldMediaContent, MediaContent newMediaContent)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var oldContentBlob = blobClient.GetBlockBlobReference(oldMediaContent.GetMediaBlobPath());
            var newContentBlob = blobClient.GetBlockBlobReference(newMediaContent.GetMediaBlobPath());
            if (oldContentBlob.Exists() && !newContentBlob.Exists())
            {
                newContentBlob.CopyFromBlob(oldContentBlob);
                newContentBlob.Metadata["FileName"] = newMediaContent.FileName;
                newContentBlob.SetMetadata();
                oldContentBlob.DeleteIfExists();
            }

        }

        public void Add(MediaContent content)
        {
            Add(content, true);
        }

        public void Update(MediaContent @new, MediaContent old)
        {
            if (!@new.FileName.EqualsOrNullEmpty(old.FileName, StringComparison.OrdinalIgnoreCase))
            {
                MoveContent(old, @new);
            }
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var contentBlob = blobClient.GetBlobReference(@new.GetMediaBlobPath());

            contentBlob = @new.MediaContentToBlob(contentBlob);
            contentBlob.SetMetadata();

            @new.VirtualPath = contentBlob.Uri.ToString();
        }

        public void Delete(MediaContent content)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var contentBlob = blobClient.GetBlobReference(content.GetMediaBlobPath());

            contentBlob.DeleteIfExists();
        }

        public void Delete(MediaFolder mediaFolder)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var contentBlob = blobClient.GetBlobDirectoryReference(mediaFolder.GetMediaDirectoryPath());
            foreach (var item in contentBlob.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true }))
            {
                var blobContent = item as CloudBlob;
                if (blobContent != null)
                {
                    blobContent.DeleteIfExists();
                }
            }
        }

        public object Execute(IContentQuery<MediaContent> query)
        {
            var mediaQuery = (MediaContentQuery)query;

            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            QueryExpressionTranslator translator = new QueryExpressionTranslator();

            var blobs = translator.Translate(query.Expression, blobClient, mediaQuery.MediaFolder)
                .Where(it => it != null)
                .Select(it => it.BlobToMediaContent(new MediaContent(mediaQuery.Repository.Name, mediaQuery.MediaFolder.FullName)));

            foreach (var item in translator.OrderFields)
            {
                if (item.Descending)
                {
                    blobs = blobs.OrderByDescending(it => it.GetType().GetProperty(item.FieldName).GetValue(it,null));
                }
                else
                {
                    blobs = blobs.OrderBy(it => it.GetType().GetProperty(item.FieldName).GetValue(it,null));
                }
            }
            //translator.Visite(query.Expression);

            switch (translator.CallType)
            {
                case CallType.Count:
                    return blobs.Count();
                case CallType.First:
                    return blobs.First();
                case CallType.Last:
                    return blobs.Last();
                case CallType.LastOrDefault:
                    return blobs.LastOrDefault();
                case CallType.FirstOrDefault:
                    return blobs.FirstOrDefault();
                case CallType.Unspecified:
                default:
                    return blobs;
            }
        }
        #endregion


        public void InitializeMediaContents(Repository repository)
        {
            MediaBlobHelper.InitializeRepositoryContainer(repository);

            Kooboo.CMS.Content.Persistence.Default.MediaFolderProvider fileMediaFolderProvider = new Default.MediaFolderProvider();
            foreach (var item in fileMediaFolderProvider.All(repository))
            {
                ImportMediaFolderDataCascading(fileMediaFolderProvider.Get(item));
            }
        }
        private void ImportMediaFolderDataCascading(MediaFolder mediaFolder)
        {
            Kooboo.CMS.Content.Persistence.Default.MediaContentProvider fileProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Content.Persistence.Default.MediaContentProvider>();

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


        public Stream GetContentStream(MediaContent content)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();
            var contentBlob = blobClient.GetBlobReference(content.GetMediaBlobPath());

            var stream = new MemoryStream();
            if (contentBlob.Exists())
            {
                contentBlob.DownloadToStream(stream);
            }
            return stream;
        }

        public void SaveContentStream(MediaContent content, Stream stream)
        {
            if (stream.Length == 0)
            {
                return;
            }
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();
            var contentBlob = blobClient.GetBlobReference(content.GetMediaBlobPath());

            if (contentBlob.Exists())
            {
                contentBlob.UploadFromStream(stream);
            }
        }
    }
}
