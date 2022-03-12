using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class DocumentGenerationModel
    {
        public int OfficerStepId { get; set; }
        public int ServiceBusinessId { get; set; }
        public string FilePath { get; set; }
        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }
        public string Executor { get; set; }

    }
    public class DocumentGenerationFieldModel
    {
        public int OfficerStepId { get; set; }
        public int ServiceBusinessId { get; set; }
        public string Keyword { get; set; }
        public string CapturedValue { get; set; }
        public string Control { get; set; }
        public string Container { get; set; }
        public string ControlNumber { get; set; }
        public string Nature { get; set; }
        public string Label { get; set; }
        public int IsRequired { get; set; }
    }
    public class DocumentGenerationSignModel
    {
        public string DestinationFile { get; set; }
        public string Executor { get; set; }
        public string Email { get; set; }
    }

}
