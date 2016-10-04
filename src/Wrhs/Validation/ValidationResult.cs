using System.Collections.Generic;

namespace Wrhs.Validation
{
    
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public List<ValidationMessage> Errors { get; set; }

        public List<ValidationMessage> Warings { get; set;}
    }
}