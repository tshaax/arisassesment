using System.ComponentModel.DataAnnotations;

namespace Aris.ServerTest.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

    }
}
