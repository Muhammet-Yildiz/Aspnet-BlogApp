
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models{
    public class RegisterViewModel { 

     [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
            
        public string Email { get; set; }
       
        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)] 
        [Display(Name = "Password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

         [DataType(DataType.Password)] 
        [Display(Name = "Re-Password")]
        [MinLength(6, ErrorMessage = "Re-Password must be at least 6 characters long")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RePassword { get; set; }

        
    }


}