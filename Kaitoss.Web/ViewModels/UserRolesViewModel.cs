using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kaitoss.Web.ViewModels
{
    public class UserRolesViewModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<SelectListItem> Roles { get; set; }
        public List<string>? UserRoles { get; set; }
    }
}
