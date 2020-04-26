using FileTaskApiCore.DataContract;
using FileTaskApiCore.DataContract.Response;
using System.Threading.Tasks;

namespace FileTaskApiCore.Services
{
    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Получает все файлы из директории по умолчанию
        /// </summary>
        /// <returns>Дерево с файлами из директории по умолчанию</returns>
        Task<Response<FileViewModel>> GetRootDirectory();
        /// <summary>
        /// Получает дерево файлов по заданному пути
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>Дерево с файлами</returns>
        Task<Response<FileViewModel>> GetFileTree(string path);
        /// <summary>
        /// Читает данные из файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Содержимое файла</returns>
        Task<Response<FileContentViewModel>> ReadFileData(string fileName);
        /// <summary>
        /// Сохраняет данные в файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Состояние выполнения операции</returns>
        Task<Response<bool>> SaveData(SaveFileViewModel file);
    }
}