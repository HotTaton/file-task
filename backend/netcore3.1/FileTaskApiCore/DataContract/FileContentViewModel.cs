using System.Collections.Generic;

namespace FileTaskApiCore.DataContract
{
    /// <summary>
    /// Представляет содержимое файла
    /// </summary>
    public class FileContentViewModel
    {
        /// <summary>
        /// Массив с содержимым файла
        /// </summary>
        public IEnumerable<IEnumerable<string>> Content { get; set; }
    }
}
