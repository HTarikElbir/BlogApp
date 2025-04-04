using BlogApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class PostAddViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Content")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Url are required")]
        [Display(Name = "Url")]
        public string? Url { get; set; }


    }
}
