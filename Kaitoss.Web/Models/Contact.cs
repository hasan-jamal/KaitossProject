using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kaitoss.Web.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string? Subject { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string? Message { get; set; }
    }
}
