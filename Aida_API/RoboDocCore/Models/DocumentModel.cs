using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class DocumentModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string EffectiveDate { get; set; }
        public string VersionNo { get; set; }
        public string Status { get; set; }
        public string ServiceName { get; set; }
    }

    public class DocumentFieldModel
    {
        public string Code { get; set; }
        public string Keyword { get; set; }
        public string Label { get; set; }
        public string Control { get; set; }
        public string Nature { get; set; }
        public int IsRequired { get; set; }
    }
}
