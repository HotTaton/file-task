using FileTaskApiCore.DataContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileTaskApiCore.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Вспомогательный класс для работы стека
        /// </summary>
        private class StackNode
        {
            /// <summary>
            /// Текущий уровень обхода
            /// </summary>
            public int Level { get; set; }
            /// <summary>
            /// Текущий объект файловой системы
            /// </summary>
            public FileSystemInfo Item { get; set; }
            /// <summary>
            /// Родительская сущность
            /// </summary>
            public FileViewModel Parent { get; set; }
        }

        /// <summary>
        /// Максимальная глубина обхода дерева каталогов
        /// </summary>
        private const int MAX_LOAD_DEPTH = 1;

        /// <summary>
        /// Корневой каталог для обхода
        /// </summary>
        private const string INITIAL_PATH_CONST = @"C:/";

        /// <summary>
        /// Разделитель
        /// </summary>
        private const string DELIMITER = "\t";

        /// <inheritdoc/>
        public FileViewModel GetRootDirectory()
        {
            return GetFileTree(INITIAL_PATH_CONST);
        }

        /// <inheritdoc/>
        public FileViewModel GetFileTree(string path)
        {
            var treeStack = new Stack<StackNode>();
            var rootDirectory = new DirectoryInfo(path); //TODO: add check is directory & is exist & not system & not hidden            
            StackNode currentNode = null;
            FileViewModel rootItem = null, current = null;

            treeStack.Push(new StackNode { Item = rootDirectory });

            while (treeStack.Count > 0 && (currentNode = treeStack.Pop()) != null)
            {
                var directory = currentNode.Item as DirectoryInfo;
                try
                {
                    current = new FileViewModel
                    {
                        IsDirectory = directory != null,
                        Name = currentNode.Item.FullName,
                        IsExpandable = directory?.GetFileSystemInfos().Length > 0 //buffer > 0, buffer vlivat 
                    };
                }
                catch (Exception e)
                {
                    continue;
                }

                if (directory != null && currentNode.Level <= MAX_LOAD_DEPTH)
                {
                    foreach (var child in directory.GetFileSystemInfos())
                    {
                        try
                        {
                            if ((child.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (child.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                treeStack.Push(new StackNode { Level = currentNode.Level + 1, Item = child, Parent = current });
                            }
                        }
                        catch (Exception e) //TODO: add check for rights or improve catch
                        {
                        }
                    }

                }

                if (rootItem == null)
                {
                    rootItem = current;
                }
                else
                {
                    currentNode.Parent.AddChild(current);
                }
            }

            return rootItem;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IEnumerable<string>>> ReadFileData(string fileName)
        {
            LinkedList<LinkedList<string>> result = new LinkedList<LinkedList<string>>();

            try
            {
                //if (File.Exists(fileName))
                {
                    using (StreamReader sr = new StreamReader(fileName.Trim((char)8234)))
                    {
                        while (!sr.EndOfStream)
                        {
                            var nextLine = await sr.ReadLineAsync();

                            if (string.IsNullOrEmpty(nextLine))
                            {
                                continue;
                            }

                            var data = nextLine.Split(DELIMITER);

                            var newRow = new LinkedList<string>();

                            foreach (var s in data)
                            {
                                newRow.AddLast(s);
                            }

                            result.AddLast(newRow);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<string> SaveData(SaveFileViewModel file)
        {
            string result = "OK!";
            try
            {
                //if (File.Exists(file.FileName))
                {
                    using (StreamWriter sr = new StreamWriter(file.FileName.Trim((char)8234)))
                    {
                        var content = new StringBuilder();
                        foreach (var fileRow in file.Content)
                        {
                            content.AppendLine(string.Join(DELIMITER, fileRow));
                        }

                        await sr.WriteAsync(content.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                result = "Error!";
            }

            return result;
        }
    }
}
