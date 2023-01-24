using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public class Blog
    {
        public int Id { get; set; }
 
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
        [Display(Name = "Writer")]
        [Required(ErrorMessage = "Writer is required")]
        public string? Writer { get; set; }
        public string? ImageUrl { get; set; }
    }
}
