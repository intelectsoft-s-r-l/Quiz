using ISQuiz.Models.Enum;

namespace ISQuiz.Models.API
{
    public class BaseErrors
    {
        public string errorMessage { get; set; }
        public string errorName { get; set; }
        public EnErrorCode errorCode { get; set; }
    }
}
