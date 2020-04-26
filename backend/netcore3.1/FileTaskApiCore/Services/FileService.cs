using FileTaskApiCore.DataContract;
using FileTaskApiCore.DataContract.Response;
using FileTaskApiCore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FileTaskApiCore.Services
{
    /// <inheritdoc/>
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
        /// Настройки сервиса
        /// </summary>
        private readonly FileServiceSettings _fileServiceSettings;

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger, IOptions<FileServiceSettings> settings)
        {
            _logger = logger;
            _fileServiceSettings = settings.Value;
        }

        /// <inheritdoc/>
        public async Task<Response<FileViewModel>> GetRootDirectory()
        {
            return await GetFileTree(_fileServiceSettings.InitialPath);
        }

        private FileViewModel TreeBypass(DirectoryInfo rootDirectory)
        {
            var treeStack = new Stack<StackNode>();
            treeStack.Push(new StackNode { Item = rootDirectory });

            StackNode currentNode;
            FileViewModel rootItem = null;

            while (treeStack.Count > 0 && (currentNode = treeStack.Pop()) != null)
            {
                var directory = currentNode.Item as DirectoryInfo;
                FileViewModel current;
                try
                {
                    //Создаем ноду по текущему элементу
                    current = new FileViewModel
                    {
                        IsDirectory = directory != null,
                        Name = currentNode.Item.FullName,
                        IsExpandable = directory?.GetFileSystemInfos().Any(child => !child.Attributes.HasFlag(FileAttributes.Hidden) && !child.Attributes.HasFlag(FileAttributes.System)) ?? false
                    };
                }
                catch (UnauthorizedAccessException uae)
                {
                    _logger.LogInformation("Access denied for files in {FileName}. Skip...", currentNode.Item.FullName);
                    continue;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Unexpected error when work with file {FileName}", currentNode.Item.FullName);
                    continue;
                }

                //В продолжаем если текущая нода является директорией и мы не достигли предельной глубины обхода
                if (directory != null && currentNode.Level <= _fileServiceSettings.MaxLoadDepth)
                {
                    //Берем все файлы из директории
                    foreach (var child in directory.GetFileSystemInfos())
                    {
                        try
                        {
                            //Проверяем их доступность
                            if (!child.Attributes.HasFlag(FileAttributes.Hidden) && !child.Attributes.HasFlag(FileAttributes.System))
                            {
                                //В случае если файл доступен, добавляем его в стек с увеличенным уровнем глубины, а так же проставляем родителя
                                treeStack.Push(new StackNode { Level = currentNode.Level + 1, Item = child, Parent = current });
                            }
                        }
                        catch (SecurityException se)
                        {
                            _logger.LogInformation("Error when work with file {FileName}. Permission {PermissionName} was in state {PermissionState}",
                                child.FullName, se.PermissionType?.Name, se.PermissionState);
                        }
                        catch (Exception e)
                        {
                            _logger.LogWarning(e, "Unexpected error when work with file {FileName}", child.FullName);
                        }
                    }

                }

                //В случае если мы ещё не заполнили корень, заполняем. Иначе - добавляем к дереву потомка
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
        public async Task<Response<FileViewModel>> GetFileTree(string path)
        {
            
            DirectoryInfo rootDirectory = null;
            var result = new Response<FileViewModel>();
            
            //Инициализируем начальную директорию
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentException("Initial path must be set!");
                }

                if (Directory.Exists(path))
                {
                    rootDirectory = new DirectoryInfo(path);
                }
                else
                {
                    throw new ArgumentException("Wrong initial path parameter");
                }
            }
            catch (Exception e)
            {
                var errorMessage = "Error when reading root file {FileName}";
                _logger.LogError(e, errorMessage, path);
                result.AddError(errorMessage.Replace("{FileName}", path));
            }

            if (!result.HasErrors)
            {
                var rootItem = await Task.Run(() => TreeBypass(rootDirectory));
                result.Item = rootItem;
            }            

            return result;
        }

        /// <inheritdoc/>
        public async Task<Response<FileContentViewModel>> ReadFileData(string fileName)
        {
            var result = new Response<FileContentViewModel>() { Item = new FileContentViewModel() };
            var fileContent = new LinkedList<LinkedList<string>>();

            try
            {
                if (!string.IsNullOrWhiteSpace(fileName) 
                    && File.Exists(fileName) 
                    && File.GetAttributes(fileName).HasFlag(FileAttributes.Normal))
                {
                    using StreamReader sr = new StreamReader(fileName);
                    while (!sr.EndOfStream)
                    {
                        var nextLine = await sr.ReadLineAsync();

                        if (string.IsNullOrEmpty(nextLine))
                        {
                            continue;
                        }

                        var data = nextLine.Split(_fileServiceSettings.FileDelimeter);

                        var newRow = new LinkedList<string>();

                        foreach (var s in data)
                        {
                            newRow.AddLast(s);
                        }

                        fileContent.AddLast(newRow);
                    }
                }
                else
                {
                    throw new ArgumentException("Wrong file path!");
                }
            }
            catch (Exception e)
            {
                var errorMessage = "Error when reading file {FileName}";
                _logger.LogError(e, errorMessage, fileName);
                result.AddError(errorMessage.Replace("{FileName}", fileName));
            }

            result.Item.Content = fileContent;

            return result;
        }

        /// <inheritdoc/>
        public async Task<Response<bool>> SaveData(SaveFileViewModel file)
        {
            var result = new Response<bool> { Item = true };

            try
            {
                if (file != null 
                    && !string.IsNullOrWhiteSpace(file.FileName) 
                    && File.Exists(file.FileName) 
                    && File.GetAttributes(file.FileName).HasFlag(FileAttributes.Normal))                
                {
                    using StreamWriter sr = new StreamWriter(file.FileName);
                    var content = new StringBuilder();
                    foreach (var fileRow in file.Content)
                    {
                        content.AppendLine(string.Join(_fileServiceSettings.FileDelimeter, fileRow));
                    }

                    await sr.WriteAsync(content.ToString());
                }
                else
                {
                    throw new ArgumentException("Wrong file chosen!");
                }
            }
            catch (Exception e)
            {
                var errorMessage = "Error when writing file {FileName}";
                _logger.LogError(e, errorMessage, file.FileName);
                result.AddError(errorMessage.Replace(errorMessage, file.FileName));
                result.Item = false;
            }

            return result;
        }
    }
}
