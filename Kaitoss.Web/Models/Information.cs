using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public class Information
    {
        public int Id { get; set; }
        [Display(Name = "Logo Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Email Address")]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }
        [Display(Name = "Detailed address")]
        public string? Detailedaddress { get; set; }
        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Schedule Time And Days")]
        public string? Schedule { get; set; }
        [Display(Name = "Office time")]
        public string? OfficeTime { get; set; }


    }
}
