using System.Collections.Generic;

namespace FileTaskApiCore.DataContract
{
    /// <summary>
    /// Модель представления для работы с файлами
    /// </summary>
    public class FileViewModel
    {
        private LinkedList<FileViewModel> _childNodes;

        /// <summary>
        /// Древовидная структура отржающая иерархию файлов на устройстве
        /// </summary>
        public ICollection<FileViewModel> ChildNodes
        {
            get
            {
                if (_childNodes == null)
                {
                    _childNodes = new LinkedList<FileViewModel>();
                }
                return _childNodes;
            }
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Является ли файл директорией
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Метод добавления потомка к текущему узлу
        /// </summary>
        /// <param name="child">Потомок</param>
        public FileViewModel AddChild(FileViewModel child)
        {
            if (child != null)
            {
                ChildNodes.Add(child);
            }

            return this;
        }

        /// <summary>
        /// Есть ли возможность дальнейшего раскрытия данного файла
        /// </summary>
        public bool IsExpandable { get; set; }
    }
}
