using Kaitoss.Web.Models;

namespace Kaitoss.Web.ViewModels
{
    public class TablesVM
    {

        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Goal> Goals { get; set; }
        public IEnumerable<About> About { get; set; }
        public IEnumerable<Information> Information { get; set; }
        public IEnumerable<Blog> Blog { get; set; }


    }
}
