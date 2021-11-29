using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Mvc4WebRole.App_Start
{
    public class WhitespaceStream : Stream
    {
        private readonly Stream _base;
        private StringBuilder _s = new StringBuilder();
        public WhitespaceStream(Stream responseStream)
        {
            if (responseStream == null)
                throw new ArgumentNullException("WhitespaceStream.ResponseStream is null.");
            _base = responseStream;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            var html = Encoding.UTF8.GetString(buffer, offset, count);
            var reg = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
            html = reg.Replace(html, string.Empty);
            buffer = Encoding.UTF8.GetBytes(html);
            this._base.Write(buffer, 0, buffer.Length);
        }
        #region Default Members
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _base.Read(buffer, offset, count);
        }
        public override bool CanRead
        {
            get { return false; }
        }
        public override bool CanSeek
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override long Length
        {
            get { return _base.Length; }
        }
        public override long Position { get; set; }
        public override void Flush()
        {
            _base.Flush();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _base.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _base.SetLength(value);
        }
        #endregion
    }
}