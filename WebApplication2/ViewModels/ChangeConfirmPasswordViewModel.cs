using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class ChangeConfirmPasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
