using System.Collections.Generic;

namespace FileTaskApiCore.ViewModels
{
    public class SaveFileViewModel 
    {
        public string FileName { get; set; }
        public IEnumerable<IEnumerable<string>> Content { get; set; }
    }
}
