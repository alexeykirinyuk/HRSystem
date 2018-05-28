using HRSystem.Common.Errors;

namespace HRSystem.Global.Validation
{
    public class ValidationFailure
    {
        public string Message { get; }

        public ValidationFailure(string message)
        {
            ArgumentHelper.EnsureNotNullOrEmpty(nameof(message), message);
            
            Message = message;
        }
    }
}