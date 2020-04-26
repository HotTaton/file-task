namespace FileTaskApiCore.Settings
{
    /// <summary>
    /// Настройки для файлового сервиса
    /// </summary>
    public class FileServiceSettings
    {
        /// <summary>
        /// Максимальная глубина обхода файлов
        /// </summary>
        public int MaxLoadDepth { get; set; }
        /// <summary>
        /// Начальный путь по умолчанию
        /// </summary>
        public string InitialPath { get; set; }
        /// <summary>
        /// Разделитель файла
        /// </summary>
        public string FileDelimeter { get; set; }
    }
}
