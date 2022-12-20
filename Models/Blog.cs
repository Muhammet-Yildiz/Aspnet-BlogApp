using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Display(Name = "Title")]
        [Required]
        [MaxLength(18, ErrorMessage = "Title must be least 10 character")]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

         [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public string? Image { get; set; }

        public List<Comment>? Comments { get; set; }


    }
}
