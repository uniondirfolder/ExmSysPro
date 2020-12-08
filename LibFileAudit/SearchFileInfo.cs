

using System;
using System.Collections.Generic;
using System.IO;

namespace LibFileAudit
{
    public class SearchFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public List<AuditInfo> FindWords { get; set; }
        public SearchFileInfo()
        {
            FindWords = new List<AuditInfo>();
        }
    }
}