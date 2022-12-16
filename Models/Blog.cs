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
        public String Title { get; set; }
        [Required]
        public String Content { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        [ValidateNever]
        public String Image { get; set; }

    }
}
