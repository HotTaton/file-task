﻿using System.Collections.Generic;

namespace FileTaskApiCore.DataContract
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveFileViewModel 
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IEnumerable<string>> Content { get; set; }
    }
}
