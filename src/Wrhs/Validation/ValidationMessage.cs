
namespace Wrhs.Validation
{
    public class ValidationMessage
    {
        public ValidationMessageType Type { get; set; }

        public string Message { get; set; }
    }


    public enum ValidationMessageType
    {
        Error = 1,
        Waring = 2
    }
}