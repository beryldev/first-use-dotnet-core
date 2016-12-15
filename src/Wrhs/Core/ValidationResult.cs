namespace Wrhs.Core
{
    public class ValidationResult
    {
        public string Field { get; }

        public string Message { get; }

        public ValidationResult(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}