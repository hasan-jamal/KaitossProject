using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IAboutsRepository : IRepository<About>
    {
        void Update(About about);
    }
}
