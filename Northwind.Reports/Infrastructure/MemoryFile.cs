using System;
using System.IO;
using System.Web;

namespace Northwind.Web.Infrastructure {

    /// <summary>
    /// MemoryFile for attachment
    /// </summary>
    public class MemoryFile : HttpPostedFileBase, IDisposable {
        private readonly Stream _stream;
        private readonly string _contentType;
        private readonly string _fileName;
        private FileStream _file;

        public MemoryFile(Stream stream, string contentType, string fileName) {
            _stream = stream;
            _contentType = contentType;
            _fileName = fileName;
        }

        public override int ContentLength {
            get { return (int)_stream.Length; }
        }

        public override string ContentType {
            get { return _contentType; }
        }

        public override string FileName {
            get { return _fileName; }
        }

        public override Stream InputStream {
            get { return _stream; }
        }

        public override void SaveAs(string filename) {
            _file = System.IO.File.Open(filename, FileMode.CreateNew);
            _stream.CopyTo(_file);
        }

        public void Dispose() {
            _file?.Dispose();
            _stream.Dispose();
        }
    }

}