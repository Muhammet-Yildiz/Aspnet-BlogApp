
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        // bu soru ısaretı burası null alabılır demek
        public string? Content { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

        public int BlogId { get; set; }
        public Blog? Blog { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }

    }
}
