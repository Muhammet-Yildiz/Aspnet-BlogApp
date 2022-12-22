
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models{
    public class UpdateCommentViewModel { 

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        
    }

}