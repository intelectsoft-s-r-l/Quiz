using ISQuiz.Models.Enum;

namespace ISQuiz.Models
{
    public class BaseResponse
    {
        public EnErrorCode ErrorCode { get; set; } 
        public string ErrorMessage { get; set; }
    }
}
