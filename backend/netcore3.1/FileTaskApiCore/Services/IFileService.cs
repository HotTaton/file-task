using FileTaskApiCore.DataContract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTaskApiCore.Services
{
    public interface IFileService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        FileViewModel GetRootDirectory();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileViewModel GetFileTree(string path);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<IEnumerable<IEnumerable<string>>> ReadFileData(string fileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> SaveData(SaveFileViewModel file);
    }
}