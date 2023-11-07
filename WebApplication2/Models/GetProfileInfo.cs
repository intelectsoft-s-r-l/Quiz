namespace WebApplication2.Models
{
    public class GetProfileInfo
    {
        public int ErrorCode { get; set; } //////
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }
}
