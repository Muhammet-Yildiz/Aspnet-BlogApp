
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } 

        [StringLength(50)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(35)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool Locked { get; set; } = false;
        
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(10)]
        [Required]
        public String Role { get; set; } = "user";

        [DataType(DataType.Upload)]
        public string? ProfileImage { get; set; }


    }
}
