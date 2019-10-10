using FileTaskApiCore.ViewModels;
using System;
using System.IO;

namespace FileTaskApiCore.Models
{
    public class FileModel
    {
        /// <summary>
        /// Максимальная глубина обхода дерева каталогов
        /// </summary>
        private const int MAX_LOAD_DEPTH = 1;

        /// <summary>
        /// Корневой каталог для обхода
        /// </summary>
        private const string INITIAL_PATH = @"C:/";

        public FileViewModel GetFileTree(string path = INITIAL_PATH)
        {
            var rootDirectory = new DirectoryInfo(path); //TODO: add check is directory & is exist & not system & not hidden

            FileViewModel result = null;

            PopulateTree(rootDirectory, ref result, 0);

            return result;
        }

        private void PopulateTree(FileSystemInfo file, ref FileViewModel parent, int currentDepth)
        {
            var directory = file as DirectoryInfo;

            var current = new FileViewModel
            {
                IsDirectory = directory != null,
                Name = file.FullName,
                IsExpandable = directory?.GetFileSystemInfos().Length > 0
            };

            if (directory != null && currentDepth <= MAX_LOAD_DEPTH)
            {
                try
                {
                    foreach (var child in directory.GetFileSystemInfos())
                    {
                        if ((child.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (child.Attributes & FileAttributes.System) != FileAttributes.System)
                        {
                            PopulateTree(child, ref current, ++currentDepth);
                        }
                    }
                }
                catch (Exception e) //TODO: add check for rights or improve catch
                {
                }
            }

            if (parent != null)
            {
                parent.AddChild(current);
            }
            else
            {
                parent = current;
            }
        }
    }
}
