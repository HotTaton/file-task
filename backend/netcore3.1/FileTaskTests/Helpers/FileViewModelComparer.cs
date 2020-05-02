using FileTaskApiCore.DataContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FileTaskTests
{
    class FileViewModelComparer : IEqualityComparer<FileViewModel>
    {
        public bool Equals([AllowNull] FileViewModel x, [AllowNull] FileViewModel y)
        {
            return x.IsDirectory == y.IsDirectory &&
                    x.IsExpandable == y.IsExpandable &&
                    (x.Name.EndsWith(y.Name, StringComparison.OrdinalIgnoreCase) || y.Name.EndsWith(x.Name, StringComparison.OrdinalIgnoreCase));
        }

        public int GetHashCode([DisallowNull] FileViewModel obj)
        {
            return Path.GetFileName(obj.Name).GetHashCode();
        }
    }
}
