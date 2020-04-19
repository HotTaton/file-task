using System.Collections.Generic;

namespace FileTaskApiCore.DataContract
{
    public class SaveFileViewModel 
    {
        public string FileName { get; set; }
        public IEnumerable<IEnumerable<string>> Content { get; set; }
    }
}
