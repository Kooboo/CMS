using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    internal class HttpPostedFileImp : HttpPostedFileBase
    {
        // Fields
        private string _contentType;
        private string _filename;
        private MemoryStream _stream;


        internal HttpPostedFileImp(string filename, string contentType, byte[] data)
        {
            this._filename = filename;
            this._contentType = contentType;
            this._stream = new MemoryStream(data);
        }

        public override void SaveAs(string filename)
        {
            if (!Path.IsPathRooted(filename))
            {
                throw new ArgumentException("SaveAs_requires_rooted_path");
            }
            FileStream s = new FileStream(filename, FileMode.Create);
            try
            {
                this._stream.WriteTo(s);
                s.Flush();
            }
            finally
            {
                s.Close();
            }
        }

        // Properties
        public override int ContentLength
        {
            get
            {
                return (int)this._stream.Length;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._contentType;
            }
        }

        public override string FileName
        {
            get
            {
                return this._filename;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return this._stream;
            }
        }

    }
    internal class InternalHttpFileCollection : System.Collections.Specialized.NameObjectCollectionBase
    {
        // Fields
        private HttpPostedFileBase[] _all;
        private string[] _allKeys;

        // Methods
        public InternalHttpFileCollection()
            : base()
        {
        }


        internal void AddFile(string key, string fileName, string contentType, byte[] data)
        {
            this._all = null;
            this._allKeys = null;

            HttpPostedFileImp file = new HttpPostedFileImp(fileName, contentType, data);

            base.BaseAdd(key, file);
        }

        public void CopyTo(Array dest, int index)
        {
            if (this._all == null)
            {
                int count = this.Count;
                HttpPostedFileBase[] fileArray = new HttpPostedFileBase[count];
                for (int i = 0; i < count; i++)
                {
                    fileArray[i] = this.Get(i);
                }
                this._all = fileArray;
            }
            if (this._all != null)
            {
                this._all.CopyTo(dest, index);
            }
        }

        public HttpPostedFileBase Get(int index)
        {
            HttpPostedFileBase file = (HttpPostedFileBase)base.BaseGet(index);

            return file;
        }

        public HttpPostedFileBase Get(string name)
        {
            HttpPostedFileBase file = (HttpPostedFileBase)base.BaseGet(name);

            return file;
        }

        public string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }

        //public override IList<HttpPostedFileBase> GetMultiple(string name)
        //{
        //    List<HttpPostedFileBase> list = new List<HttpPostedFileBase>();
        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        if (string.Equals(this.GetKey(i), name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            list.Add(this.Get(i));
        //        }
        //    }
        //    return list.AsReadOnly();
        //}
        // Properties
        public string[] AllKeys
        {
            get
            {
                if (this._allKeys == null)
                {
                    this._allKeys = base.BaseGetAllKeys();
                }
                return this._allKeys;
            }
        }

        public HttpPostedFileBase this[string name]
        {

            get
            {
                return this.Get(name);
            }
        }

        public HttpPostedFileBase this[int index]
        {
            get
            {
                return this.Get(index);
            }
        }
    }

    public class HttpFileCollectionImp : HttpFileCollectionBase
    {
        InternalHttpFileCollection _collection = new InternalHttpFileCollection();
        public HttpFileCollectionImp()
        {
        }

        internal void AddFile(string key, string fileName, string contentType, byte[] data)
        {
            _collection.AddFile(key, fileName, contentType, data);
        }

        public override void CopyTo(Array dest, int index)
        {
            _collection.CopyTo(dest, index);
        }

        public override HttpPostedFileBase Get(int index)
        {
            return _collection.Get(index);
        }

        public override HttpPostedFileBase Get(string name)
        {
            return _collection.Get(name);
        }

        public override string GetKey(int index)
        {
            return _collection.GetKey(index);
        }
        public override int Count
        {
            get
            {
                return _collection.Count;
            }
        }
        //public override IList<HttpPostedFileBase> GetMultiple(string name)
        //{
        //    List<HttpPostedFileBase> list = new List<HttpPostedFileBase>();
        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        if (string.Equals(this.GetKey(i), name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            list.Add(this.Get(i));
        //        }
        //    }
        //    return list.AsReadOnly();
        //}
        // Properties
        public override System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return _collection.Keys;
            }
        }
        public override string[] AllKeys
        {
            get
            {
                return _collection.AllKeys;
            }
        }

        public override HttpPostedFileBase this[string name]
        {

            get
            {
                return _collection[name];
            }
        }

        public override HttpPostedFileBase this[int index]
        {
            get
            {
                return _collection[index];
            }
        }

    }
}
