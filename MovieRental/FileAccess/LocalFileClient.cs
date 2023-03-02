using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieRental.FileAccess
{
    public class LocalFileClient : IFileClient
    {
        private const string RootPath = "wwwroot\\uploads";
        public void Delete(string container, string fileName)
        {
            var path = GetPath(container, fileName);

            if(!File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }

        public Stream Get(string container, string fileName)
        {
            var path = GetPath(container, fileName);

            if (!File.Exists(path))
            {
                return null;
            }

            return File.OpenRead(path);
        }

        public IList<string> List(string container, string prefix)
        {
            var path = Path.Combine(RootPath, container);
            return Directory.EnumerateFiles(path).ToList();
        }

        public void Save(string container, string fileName, Stream inputStream)
        {
            Delete(container, fileName);

            var path = GetPath(container, fileName);
            using (var outputStream = new FileStream(path, FileMode.CreateNew))
            {
                inputStream.CopyTo(outputStream);
            }
        }

        private string GetPath(string container, string fileName)
        {
            return Path.Combine(RootPath, container, fileName);
        }
    }

}
