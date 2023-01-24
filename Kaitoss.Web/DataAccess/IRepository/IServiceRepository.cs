using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IServiceRepository : IRepository<Service>
    {
        void Update(Service service);
    }
}
