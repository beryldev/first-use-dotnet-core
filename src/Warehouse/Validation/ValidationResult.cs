using System;
using System.Collections.Generic;

namespace Warehouse.Validation
{
    
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public List<ValidationMessage> Errors { get; set; }

        public List<ValidationMessage> Warings { get; set;}
    }
}