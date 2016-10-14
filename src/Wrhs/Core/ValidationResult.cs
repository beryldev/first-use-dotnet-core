
namespace Wrhs.Core
{ 
    public class ValidationResult
    {
        public string Field { get; set; }

        public string Message { get; set; }

        public ValidationResult() { }

        public ValidationResult(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}