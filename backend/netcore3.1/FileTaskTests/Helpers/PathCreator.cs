using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace FileTaskTests
{
    public class PathCreator : IDisposable
    {
        private class FileNamesComparer : IComparer<string>
        {
            public int Compare([AllowNull] string x, [AllowNull] string y)
            {
                if (!string.IsNullOrEmpty(Path.GetExtension(x)) && string.IsNullOrEmpty(Path.GetExtension(y)))
                {
                    return 1;
                }
                if (string.IsNullOrEmpty(Path.GetExtension(x)) && !string.IsNullOrEmpty(Path.GetExtension(y)))
                {
                    return -1;
                }
                return x.CompareTo(y);
            }
        }

        private readonly LinkedList<string> _files = new LinkedList<string>();        
        private readonly string[] _filesContent;                

        public PathCreator(ICollection<string> files) : this(files, null) { }

        public PathCreator(ICollection<string> files, string[] filesContent)
        {
            _filesContent = filesContent;
            var userDir = Path.GetTempPath();
            var fileNames = files.OrderBy(x => x, new FileNamesComparer()).Select(x => $"{userDir}{x}").ToArray();
            _files = new LinkedList<string>(fileNames);
        }

        public void CreateFiles()
        {
            foreach (var item in _files)
            {
                if (string.IsNullOrEmpty(Path.GetExtension(item)))
                {
                    Directory.CreateDirectory(item);
                }
                else
                {
                    if (_filesContent != null)
                    {
                        File.WriteAllLines(item, _filesContent);
                    }
                    else
                    {
                        using var file = File.Create(item);
                    }
                }
            }
        }

        public string GetFullPath(string filePath)
        {
            return _files.FirstOrDefault(item => item.EndsWith(filePath));
        }

        public void Dispose()
        {
            var item = _files.Last;

            while (item != null)
            {                
                if (File.GetAttributes(item.Value).HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(item.Value);
                }
                else
                {
                    File.Delete(item.Value);
                }

                item = item.Previous;
            }
        }
    }
}
