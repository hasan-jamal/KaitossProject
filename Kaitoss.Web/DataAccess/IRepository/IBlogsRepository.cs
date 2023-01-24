using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IBlogsRepository : IRepository<Blog>
    {
        void Update(Blog blog);
    }
}
