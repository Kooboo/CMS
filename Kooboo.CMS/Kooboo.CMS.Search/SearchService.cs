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
using System.IO;
using Lucene.Net.Documents;
using Kooboo.CMS.Search.Models;
using System.Collections.Specialized;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Content.Models;
using System.Threading;
using Kooboo.CMS.Common.Runtime;
using Lucene.Net.Search.Highlight;

namespace Kooboo.CMS.Search
{
    public class DocumentConverter
    {
        public DocumentConverter(Analyzer analyzer)
        {
            this.TitleFieldName = "_TitleIndex_";
            this.BodyFieldName = "_BodyIndex_";
            this.NativeTypeNameField = "_NativeType_";

            this.Analyzer = analyzer;
        }


        public virtual string TitleFieldName { get; set; }
        public virtual string BodyFieldName { get; set; }
        public virtual string NativeTypeNameField { get; set; }

        public virtual Analyzer Analyzer { get; set; }

        protected virtual bool IsReservedField(string fieldName)
        {
            return fieldName.EqualsOrNullEmpty(TitleFieldName, StringComparison.OrdinalIgnoreCase)
                || fieldName.EqualsOrNullEmpty(BodyFieldName, StringComparison.OrdinalIgnoreCase)
                || fieldName.EqualsOrNullEmpty(NativeTypeNameField, StringComparison.OrdinalIgnoreCase);
        }
        public virtual Document ToDocument(object o)
        {
            Document doc = null;
            var converter = ObjectConverters.GetConverter(o.GetType());
            var indexObject = converter.GetIndexObject(o);
            if (indexObject != null)
            {
                doc = new Document();
                var key = converter.GetKeyField(o);
                doc.Add(new Field(key.Key, key.Value.ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field(TitleFieldName, indexObject.Title, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(BodyFieldName, indexObject.Body, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(NativeTypeNameField, indexObject.NativeType, Field.Store.YES, Field.Index.NO));
                if (indexObject.StoreFields != null)
                {
                    foreach (var item in indexObject.StoreFields.AllKeys)
                    {
                        if (!item.EqualsOrNullEmpty(key.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            doc.Add(new Field(item, indexObject.StoreFields[item], Field.Store.YES, Field.Index.NO));
                        }
                    }

                }
                if (indexObject.SystemFields != null)
                {
                    foreach (var item in indexObject.SystemFields.AllKeys)
                    {
                        if (!item.EqualsOrNullEmpty(key.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            doc.Add(new Field(item, indexObject.SystemFields[item], Field.Store.YES, Field.Index.NOT_ANALYZED));
                        }
                    }
                }
            }
            return doc;
        }
        public virtual ResultObject ToResultObject(Highlighter highlighter, Document doc)
        {
            var nativeTypeName = doc.GetField(NativeTypeNameField).StringValue;
            var nativeType = Type.GetType(nativeTypeName);
            var converter = ObjectConverters.GetConverter(nativeType);

            ResultObject result = new ResultObject();

            result.Title = doc.GetField(TitleFieldName).StringValue;
            result.HighlightedTitle = highlighter.GetBestFragment(this.Analyzer, TitleFieldName, result.Title);

            if (string.IsNullOrEmpty(result.HighlightedTitle))
            {
                result.HighlightedTitle = result.Title;
            }

            result.Body = doc.GetField(BodyFieldName).StringValue;
            result.HighlightedBody = string.Join("...", highlighter.GetBestFragments(Analyzer, BodyFieldName, result.Body, 5));

            if (string.IsNullOrEmpty(result.HighlightedBody))
            {
                result.HighlightedBody = result.Body;
            }

            NameValueCollection fields = new NameValueCollection();
            foreach (Field field in doc.GetFields())
            {
                if (!IsReservedField(field.Name))
                {
                    fields[field.Name] = field.StringValue;
                }
            }

            result.NativeObject = converter.GetNativeObject(fields);

            result.Url = converter.GetUrl(result.NativeObject);

            return result;

        }
        public virtual Term GetKeyTerm(object o)
        {
            var converter = ObjectConverters.GetConverter(o.GetType());
            var key = converter.GetKeyField(o);
            return new Term(key.Key, key.Value.ToLower());
        }
    }

    public class SearchService : ISearchService
    {
        public string IndexName { get; private set; }

        private string indexDir = null;

        public SearchService(Repository repository)
        {
            this.IndexName = repository.Name;

            indexDir = Path.Combine(SearchDir.GetBasePhysicalPath(repository), "Index");

            Analyzer = new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);
            Converter = new DocumentConverter(Analyzer);
        }


        public virtual Analyzer Analyzer { get; set; }
        public virtual DocumentConverter Converter { get; set; }

        #region Build index
        public virtual void Add<T>(T o)
        {
            BatchAdd(new[] { o });
        }

        public virtual void Update<T>(T o)
        {
            BatchUpdate(new[] { o });
        }

        public virtual void Delete<T>(T o)
        {
            BatchDelete(new[] { o });
        }

        private IndexWriter CreateIndexWriter()
        {
            var writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(indexDir)), Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            return writer;
        }

        public virtual void BatchAdd<T>(IEnumerable<T> list)
        {
            var writer = CreateIndexWriter();
            try
            {
                foreach (var item in list)
                {
                    var doc = Converter.ToDocument(item);
                    if (doc != null)
                    {
                        writer.AddDocument(doc);
                    }
                    LogLastAction(item, ContentAction.Add);
                }

            }
            finally
            {
                writer.Optimize();
                writer.Close();
            }
        }

        public virtual void BatchUpdate<T>(IEnumerable<T> list)
        {
            var writer = CreateIndexWriter();
            try
            {
                foreach (var item in list)
                {
                    var doc = Converter.ToDocument(item);
                    var keyTerm = Converter.GetKeyTerm(item);
                    if (doc != null)
                    {
                        writer.UpdateDocument(keyTerm, doc);
                    }

                    LogLastAction(item, ContentAction.Update);
                }
            }
            finally
            {
                writer.Optimize();
                writer.Close();
            }

        }

        public virtual void BatchDelete<T>(IEnumerable<T> list)
        {
            var writer = CreateIndexWriter();
            try
            {
                var keyTerms = list.Select(it => Converter.GetKeyTerm(it)).ToArray();
                writer.DeleteDocuments(keyTerms);

                foreach (var item in list)
                {
                    LogLastAction(item, ContentAction.Delete);
                }
            }
            finally
            {
                writer.Optimize();
                writer.Close();
            }

        }
        public void BatchDelete(string folderName)
        {
            var writer = CreateIndexWriter();
            try
            {
                writer.DeleteDocuments(new Lucene.Net.Index.Term("FolderName", folderName));
            }
            finally
            {
                writer.Optimize();
                writer.Close();
            }

        }
        #endregion

        protected virtual void LogLastAction(object o, ContentAction action)
        {
            Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (o is TextContent)
                        {
                            var textContent = (TextContent)o;
                            var repository = textContent.GetRepository();
                            var folderName = textContent.FolderName;
                            var contentSummary = textContent.GetSummary();
                            LastAction lastAction = new LastAction()
                            {
                                Repository = repository,
                                FolderName = folderName,
                                ContentSummary = contentSummary,
                                Action = action,
                                UtcActionDate = DateTime.UtcNow
                            };
                            EngineContext.Current.Resolve<Kooboo.CMS.Search.Persistence.ILastActionProvider>().Add(lastAction);
                        }
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }

                });
            thread.Start();
        }

        public virtual PagedList<Models.ResultObject> Search(string key, int pageIndex, int pageSize, params string[] folders)
        {
            var indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDir));
            if (!IndexReader.IndexExists(indexDirectory) || string.IsNullOrEmpty(key) && (folders == null || folders.Length == 0))
            {
                return new PagedList<ResultObject>(new ResultObject[0], pageIndex, pageSize, 0);
            }

            var query = new BooleanQuery();

            key = QueryParser.Escape(key.Trim().ToLower());

            if (string.IsNullOrEmpty(key))
            {
                key = "*:*";
            }

            QueryParser titleParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, Converter.TitleFieldName, this.Analyzer);
            var titleQuery = titleParser.Parse(key);
            titleQuery.Boost = 2;
            query.Add(new BooleanClause(titleQuery, Occur.SHOULD));

            QueryParser bodyParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, Converter.BodyFieldName, this.Analyzer);
            var bodyQuery = bodyParser.Parse(key);
            bodyQuery.Boost=1;
            query.Add(new BooleanClause(bodyQuery, Occur.SHOULD));

            QueryWrapperFilter filter = null;
            if (folders != null && folders.Length > 0)
            {
                var folderQuery = new BooleanQuery();
                //QueryParser folderParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "FolderName", this.Analyzer);
                foreach (var folder in folders)
                {
                    var termQuery = new TermQuery(new Term("FolderName", folder));
                    termQuery.Boost= 3;
                    folderQuery.Add(new BooleanClause(termQuery,Occur.SHOULD));
                }

                filter = new QueryWrapperFilter(folderQuery);
            }

            var searcher = new IndexSearcher(indexDirectory, true);
            var collecltor = TopScoreDocCollector.Create(searcher.MaxDoc, false);
            if (filter == null)
            {
                searcher.Search(query, collecltor);
            }
            else
            {
                searcher.Search(query, filter, collecltor);
            }


            var lighter =
                       new Highlighter(new SimpleHTMLFormatter("<strong class='highlight'>", "</strong>"), new Lucene.Net.Search.Highlight.QueryScorer((Query)query));


            var startIndex = (pageIndex - 1) * pageSize;

            List<ResultObject> results = new List<ResultObject>();
            foreach (var doc in collecltor.TopDocs(startIndex, pageSize).ScoreDocs)
            {
                var document = searcher.Doc(doc.Doc);
                ResultObject result = Converter.ToResultObject(lighter, document);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            return new PagedList<ResultObject>(results, pageIndex, pageSize, collecltor.TotalHits);
        }
    }
}
