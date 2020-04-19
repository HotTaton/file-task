using System.Collections.Generic;

namespace FileTaskApiCore.DataContract
{
    public class FileViewModel
    {
        private LinkedList<FileViewModel> _childNodes;

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

        public string Name { get; set; }

        public bool IsDirectory { get; set; }

        public void AddChild(FileViewModel child)
        {
            if (child != null)
            {
                ChildNodes.Add(child);
            }
        }

        public bool IsExpandable { get; set; }
    }
}
