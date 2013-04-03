using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class CmisHttpPostedFileWrapper : HttpPostedFileBase
    {
        private int contentLength;
        private string contentType;
        private string fileName;
        Stream inputStream;
        public CmisHttpPostedFileWrapper(NCMIS.ObjectModel.ContentStream contentStream)
        {
            this.contentLength = contentStream.Length.HasValue ? (int)contentStream.Length.Value : 0;
            this.contentType = contentStream.MimeType;
            this.fileName = contentStream.Filename;
            this.inputStream = new MemoryStream(contentStream.Stream);
        }
        public override int ContentLength
        {
            get
            {
                return this.contentLength;
            }
        }
        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }
        public override string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        public override System.IO.Stream InputStream
        {
            get
            {
                return this.inputStream;
            }
        }
    }
    public class CmisHttpFileCollectionWrapper : HttpFileCollectionBase
    {

        public CmisHttpFileCollectionWrapper() { }
        // Methods
        public CmisHttpFileCollectionWrapper(HttpFileCollectionBase files)
        {
            foreach (var key in files.AllKeys)
            {
                this.AddFile(key, files[key]);
            }
        }

        public void AddFile(string key, HttpPostedFileBase file)
        {
            base.BaseAdd(key, file);
        }

        public override void CopyTo(Array dest, int index)
        {
            int count = this.Count;
            var _all = new HttpPostedFileBase[count];
            for (int i = 0; i < count; i++)
            {
                _all[i] = this.Get(i);
            }
            _all.CopyTo(dest, index);
        }

        public override HttpPostedFileBase Get(int index)
        {
            return (HttpPostedFileBase)base.BaseGet(index);
        }

        public override HttpPostedFileBase Get(string name)
        {
            return (HttpPostedFileBase)base.BaseGet(name);
        }

        public override string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }

        // Properties
        public override string[] AllKeys
        {
            get
            {
                return base.BaseGetAllKeys();
            }
        }

        public override HttpPostedFileBase this[string name]
        {
            get
            {
                return this.Get(name);
            }
        }

        public override HttpPostedFileBase this[int index]
        {
            get
            {
                return this.Get(index);
            }
        }
        public void Remove(string key)
        {
            base.BaseRemove(key);
        }


        public override int Count
        {
            get
            {
                return this.AllKeys.Count();
            }
        }

    }
    public static class ContentStreamExtensions
    {
        public static HttpPostedFileBase ToHttpPostedFile(this NCMIS.ObjectModel.ContentStream contentStream)
        {
            return new CmisHttpPostedFileWrapper(contentStream);
        }
        public static HttpFileCollectionBase ToFileCollection(this NCMIS.ObjectModel.ContentStream contentStream)
        {
            CmisHttpFileCollectionWrapper fileCollection = new CmisHttpFileCollectionWrapper();
            fileCollection.AddFile(contentStream.Id, contentStream.ToHttpPostedFile());
            return fileCollection;
        }
    }
}
