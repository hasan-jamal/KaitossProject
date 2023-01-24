using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaitoss.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public string? Name { get; set; }
        public string? Country { get; set; }

    }
}
