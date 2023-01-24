using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public class Service
    {
        public int Id { get; set; }
 
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        [Display(Name = "Describe ")]
        [Required(ErrorMessage = "Describe is required")]
        public string? Describe { get; set; }
        public string? ImageUrl { get; set; }
    }
}
