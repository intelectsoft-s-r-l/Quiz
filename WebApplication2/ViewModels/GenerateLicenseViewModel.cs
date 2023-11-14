using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class GenerateLicenseViewModel
    {
       // [Required(ErrorMessage = "Quantity equal 0")]
        public string token { get; set; }
        public int quantity { get; set; }

    }
}
