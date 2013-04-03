using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Web
{
    //class ObserveResponseLengthStream : Stream
    //{
    //    private Stream _stream = null;
    //    private long _length = 0;
    //    public ObserveResponseLengthStream(Stream stream)
    //    {
    //        this._stream = stream;
    //    }

    //    public override long Seek(long offset,
    //       SeekOrigin origin)
    //    {
    //        return this._stream.Seek(offset, origin);
    //    }

    //    public override void Flush()
    //    {
    //        this._stream.Flush();
    //    }

    //    public override void SetLength(long value)
    //    {
    //        this._stream.SetLength(value);
    //        this._length = value;
    //    }

    //    public override void Write(byte[] buffer,
    //       int offset,
    //       int count)
    //    {
    //        this._stream.Write(buffer, offset, count);
    //        this._length += (long)count;
    //    }

    //    public override void Close()
    //    {
    //        try
    //        {
    //            this._stream.Close();
    //        }
    //        catch { }

    //        base.Close();

    //        try
    //        {
    //            /*now you know the actual length 
    //              of the response in this._length,
    //              so do whatever you want with it*/
    //        }
    //        catch (Exception ex)
    //        {
    //            //make sure your exceptions are handled!
    //        }
    //    }

    //    /*...all the other code simply exposes the 
    //      _stream's properties and methods...*/

    //    public override bool CanRead
    //    {
    //        get { return this._stream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return this._stream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return this._stream.CanWrite; }
    //    }

    //    public override long Length
    //    {
    //        get { return this._length; }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return this._stream.Position;
    //        }
    //        set
    //        {
    //            this._stream.Position = value;
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        return this._stream.Read(buffer, offset, count);
    //    }
    //}
    public class FrontHttpResponseWrapper : System.Web.HttpResponseWrapper
    {
        FrontHttpContextWrapper _context;
        public FrontHttpResponseWrapper(HttpResponse httpResponse, FrontHttpContextWrapper context)
            : base(httpResponse)
        {
            //httpResponse.Filter = new ObserveResponseLengthStream(httpResponse.Filter);
            _context = context;
        }
    }
}
