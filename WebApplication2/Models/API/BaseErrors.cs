namespace WebApplication2.Models.API
{
    public class BaseErrors
    {
          public string? errorMessage { get; set; }
          public string? errorName { get; } //ReadOnly
          public int errorCode { get; set; }
    }
}
