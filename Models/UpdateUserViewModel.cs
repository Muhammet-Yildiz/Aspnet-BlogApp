using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class UpdateUserViewModel
    {

        [StringLength(50)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(35)]
        public string Email { get; set; }
      

        [StringLength(10)]
        [Required]
        public String Role { get; set; } = "user";


        [DataType(DataType.Upload)]
        public string? ProfileImage { get; set; }



    }
}
