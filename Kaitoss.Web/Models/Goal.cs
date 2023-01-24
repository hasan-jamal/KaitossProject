using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public class Goal
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        [Display(Name = "Sub Title")]
        [Required(ErrorMessage = "Sub Title is required")]
        public string? SubTitle { get; set; }

        [Display(Name = "Link Video")]
        public string? LinkVideo { get; set; }

        public string? ImageUrl { get; set; }

    }
}
