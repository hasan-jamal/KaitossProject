using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public class About
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Display(Name = "Who we are")]
        [Required(ErrorMessage = "Who we are is required")]
        public string? WhoWeAre { get; set; }
  
        [Display(Name = "Vision")]
        [Required(ErrorMessage = "Vision is required")]
        public string? Vision { get; set; }

        [Display(Name = "History")]
        [Required(ErrorMessage = "History is required")]
        public string? History { get; set; }
        public string? ImageUrl { get; set; }

    }
}
