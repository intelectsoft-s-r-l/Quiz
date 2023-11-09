using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class ChangeConfirmPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        //[Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
