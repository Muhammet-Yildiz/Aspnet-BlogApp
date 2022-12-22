using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApp.Models
{
    public class UpdateBlogViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Title must be least 50 character")]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }

         [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public string? Image { get; set; }


    }
}
