using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRental.UnitTests.ControllerTests
{
    internal class FakeFormFile : IFormFile
    {
        public string ContentType => throw new NotImplementedException();
        public string ContentDisposition => throw new NotImplementedException();
        public IHeaderDictionary Headers => throw new NotImplementedException();
        public long Length => throw new NotImplementedException();
        public string Name => throw new NotImplementedException();

        public string FileName { get; set; }

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream OpenReadStream()
        {
            return Stream.Null;
        }
    }
}
