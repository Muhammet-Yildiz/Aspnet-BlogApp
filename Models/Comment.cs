
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
      
        [Required]
        public string? Content { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

      

    }
}
