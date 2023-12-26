namespace ISQuiz.Models
{
    public class GetProfileInfo : BaseResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
