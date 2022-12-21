
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        // [Display(Name = "Author")]
        // [Required(ErrorMessage = "The author is required")] 
        // public string Author { get; set; }
      
        // bu soru ısaretı burası null alabılır demek
        public string? Content { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

    // bu yorumu kım yapmıs sonradan eklenıcek auth ıslemlerınden sonra 
        
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }


        public Guid? UserId { get; set; }

        public User? User { get; set; }

    }
}
