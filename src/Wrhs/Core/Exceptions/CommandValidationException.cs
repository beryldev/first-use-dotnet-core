using System;
using System.Collections.Generic;

namespace Wrhs.Core.Exceptions
{
    public class CommandValidationException : WrhsException
    {
        public ICommand Command { get; }

        public IEnumerable<ValidationResult> ValidationResults { get;}

        public CommandValidationException(string message, ICommand command, 
            IEnumerable<ValidationResult> validationResults) : base(message)
        {
            Command = command;
            ValidationResults = validationResults;
        }

        public CommandValidationException(string message, ICommand command, 
            IEnumerable<ValidationResult> validationResults, Exception innerException) 
            : base(message, innerException)
        {
            Command = command;
            ValidationResults = validationResults;
        }
    }
}