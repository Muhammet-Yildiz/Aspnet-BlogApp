
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class User
    {
        [Key]
        // bu idyi eşsiz yapmak için guid kullanıyoruz
        public Guid Id { get; set; } 

        // null  olabılir '?'  
        [StringLength(50)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(35)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        // şifreyi hashleyip veritabanına kaydederken uzayacak ondan 100 karakter ayırdık
        public string Password { get; set; }

        public bool Locked { get; set; } = false;
        
        // public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(10)]
        [Required]
        public String Role { get; set; } = "user";

    }
}
