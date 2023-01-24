using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email")]    
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
