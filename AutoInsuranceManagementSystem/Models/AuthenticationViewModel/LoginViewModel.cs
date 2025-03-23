using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models.AuthenticationViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is mandatory")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; } = string.Empty;
    }
}
